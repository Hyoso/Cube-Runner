using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private PlayerInputBtn playerInputBtn;
	[SerializeField]
	private bool pointerDown = false;

	public Vector3 startPosition;
	public float minJumpHeight = 1.0f;
	public float maxJumpHeight = 3.0f;
	public float jumpScale = 3.0f; // multiplied by elapsedTime and then capped by maxjumpheight
	public Animator characterAnimator;
	public Rigidbody rigidBody3D;
	public float jumpRotations = 1.0f;
	[Range(0.75f, 1.0f)]
	public float jumpRotationPercentage = 0.925f;

	private float moveToStartPosSpeed = 0.0f; // * Time.deltaTime

	private bool initialised = false;

	private float initJumpTime = 0f;
	private float lastInitJumpTime = 0f;

	private bool isJumping = false;

	private Vector3 holdPos;

	private bool doFall = false;
	private bool reducedMaxJumpHeight = false; // this slows down the rate at which the falling rotation is carried out
	private float curMaxJumpHeight = 0.0f;

	private Quaternion targetRotation = Quaternion.identity;

	private void Start()
	{
		StartCoroutine(DelayedStartCoroutine());
		//StartCoroutine(TargetRotationCoroutine());
		moveToStartPosSpeed = Vector3.Distance(transform.position, startPosition) * 0.25f;
	}

	private IEnumerator DelayedStartCoroutine()
	{
		yield return new WaitForEndOfFrame();

		playerInputBtn = GameCanvasObjects.Instance.playerInputBtn;

		playerInputBtn.OnPointerDownEvents += () => pointerDown = true;
		playerInputBtn.OnPointerDownEvents += HoldPlayerJump;

		playerInputBtn.OnPointerUpEvents += () => pointerDown = false;
		playerInputBtn.OnPointerUpEvents += ReleasePlayerJump;

		LevelManager.Instance.OnGameOverEvents += () => playerInputBtn.gameObject.SetActive(false);
	}

	private IEnumerator StartAnimationsCoroutine()
	{
		while (Vector3.Distance(transform.position, startPosition) > 0.005f)
		{
			transform.position = Vector3.Lerp(transform.position, startPosition, moveToStartPosSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		transform.position = startPosition;

		initialised = true;
	}

	private void Update()
	{
		if (!initialised)
		{
			StartCoroutine(StartAnimationsCoroutine());
		}

		if (!StartTimer.Instance.CanStart)
		{
			return;
		}
	}

	private void FixedUpdate()
	{
		if (LevelManager.Instance.IsGameOver)
		{
			rigidBody3D.velocity *= LevelManager.Instance.SimulatedTimeScale;
			rigidBody3D.angularVelocity *= LevelManager.Instance.SimulatedTimeScale;
			if (LevelManager.Instance.SimulatedTimeScale < 1.0f)
			{
				rigidBody3D.useGravity = false;
				rigidBody3D.AddForce(Physics.gravity * rigidBody3D.mass * 0.5f * LevelManager.Instance.SimulatedTimeScale);
			}
		}
		else
		{
			if (isJumping && rigidBody3D.velocity.y > 0.0f)
			{
				doFall = true;
				float normalizedY = transform.position.y / curMaxJumpHeight;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.left * 360.0f * jumpRotations * jumpRotationPercentage * normalizedY), Time.deltaTime * 50.0f);
			}
			else if (isJumping && rigidBody3D.velocity.y <= 0.0f && doFall)
			{
				if (!reducedMaxJumpHeight)
				{
					reducedMaxJumpHeight = true;
					curMaxJumpHeight *= jumpRotationPercentage;
				}

				float normalizedY = (curMaxJumpHeight - (transform.position.y)) / curMaxJumpHeight / jumpRotationPercentage;
				Vector3 euler = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.left * 360.0f * Mathf.Clamp(curMaxJumpHeight, jumpRotationPercentage, 1.0f)), Time.deltaTime * 2.0f);
			}

			if (StartTimer.Instance.CanStart && LevelManager.Instance.SimulatedTimeScale > 0.0f && initialised)
			{
				Vector3 tempPos = transform.position;
				tempPos.z = startPosition.z;
				transform.position = tempPos;
			}
		}
	}

	private IEnumerator TargetRotationCoroutine()
	{
		while (true)
		{
			if (isJumping)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 500.0f);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public void HoldPlayerJump()
	{
		// removed to give player more responsive gameplay
		//if (isJumping)
		//{
		//	Debug.Log("player is jumping");
		//	return;
		//}

		// jump height based on time
		initJumpTime = Time.time;
		characterAnimator.SetTrigger("Hold");
	}
	
	public void ReleasePlayerJump()
	{
		if (isJumping || initJumpTime == lastInitJumpTime)
		{
			return;
		}

		lastInitJumpTime = initJumpTime;

		// jump height is capped
		float elapsedTime = Time.time - initJumpTime;
		float jumpForce = elapsedTime * jumpScale;
		float clampedJumpForce = Mathf.Clamp(jumpForce, minJumpHeight, maxJumpHeight);
		//SetAnimationSpeeds(clampedJumpForce);
		CalculateMaxJumpHeight(clampedJumpForce);
		reducedMaxJumpHeight = false;
		doFall = false;

		StartCoroutine(JumpCoroutine(clampedJumpForce));
	}

	private IEnumerator JumpCoroutine(float clampedJumpForce)
	{
		isJumping = true;
		//characterAnimator.SetBool("Jump", true);
		rigidBody3D.AddForce(Vector3.up * clampedJumpForce, ForceMode.Impulse);

		yield return null;
	}

	// reference: https://answers.unity.com/questions/1075046/predicting-max-jump-height-of-a-force-impulse.html
	void CalculateMaxJumpHeight(float clampedJumpForce)
	{
		float g = Physics.gravity.magnitude;
		float v0 = clampedJumpForce / rigidBody3D.mass; // converts the jumpForce to an initial velocity
		curMaxJumpHeight = transform.position.y + (v0 * v0) / (2 * g);
	}

	public float animationSpeedScale = 0.5f;
	private void SetAnimationSpeeds(float jumpForce)
	{
		characterAnimator.speed = (maxJumpHeight - jumpForce) * animationSpeedScale;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Platform"))
		{
			isJumping = false;
		}
		else if (collision.gameObject.CompareTag("Trap"))
		{
			LevelManager.Instance.SetGameOver();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Point"))
		{
			ScoreDisplay.Instance.IncrementScore();
			other.gameObject.GetComponent<PointBlock>().SetTrapCubeGold();
		}
	}
}
