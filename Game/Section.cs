using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
	public float sectionLength = 0f;
	public const float velocity = 5.0f;
	[Tooltip("change this depending on the last sections length")]
	public float distanceToAnimate = 7.0f;
	public float spawnInHeight = -10.0f;

	public bool isAnimating = false;

	[SerializeField]
	private Transform floorCubes = null;
	public delegate void OnResetDelegate();
	public OnResetDelegate OnResetEvents;

	private Vector3 initialPosition;
	private bool canSelectNewSection = true;

	public List<TrapCube> trapsList = new List<TrapCube>();

	private void Start()
	{
		initialPosition = transform.position;
		//OnResetEvents += SectionsManager.Instance.SelectNewSection;
	}

	// todo: Change this into coroutine?
	private void Update()
	{
		if (!StartTimer.Instance.CanStart)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			transform.position = initialPosition;
		}

		if (isAnimating)
		{
			transform.Translate(Vector3.forward * velocity * Time.deltaTime * LevelManager.Instance.SimulatedTimeScale);
			if (transform.position.z > 12.0f + sectionLength * 0.5f && canSelectNewSection)
			{
				// calling select new section before isAnimating is set prevents this section from being used again
				//OnResetEvents.Invoke();

				canSelectNewSection = false;
				SectionsManager.Instance.SelectNewSection();
			}
			if (transform.position.z > 12.0f + sectionLength)
			{
				SetPosition(-50.0f);
				OnResetEvents.Invoke();
				isAnimating = false;
				canSelectNewSection = true;
			}
		}
	}

	public void SetPosition(float z)
	{
		Vector3 tempPos = transform.position;
		tempPos.z = z;
		transform.position = tempPos;
	}

	public void ResetTrapCubes()
	{
		trapsList.ForEach(x => x.SetOriginalMat());
	}
}
