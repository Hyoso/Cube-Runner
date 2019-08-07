using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	[SerializeField]
	public readonly Vector3 playerPosition = new Vector3(0.0f, 0.0f, 0.0f);

	public float targetHeight;
	public const float lerpSpeed = 3.95f;
	public float intialisedWidth = 1.05f; // this removes the zfighting resulting in a more polished feel

	[SerializeField]
	private Section section;

	public Vector3 OriginalPosition { get { return originalPosition; } }
	private Vector3 originalPosition;

	private void Start()
	{
		Vector3 tempScale = transform.localScale;
		tempScale.z = intialisedWidth;
		transform.localScale = tempScale;
		originalPosition = transform.position;
		targetHeight = transform.position.y;
		//transform.position = new Vector3(transform.position.x, section.spawnInHeight, transform.position.z);
		transform.Translate(Vector3.up * section.spawnInHeight);

		section.OnResetEvents += ResetPositions;
	}

	public void ResetPositions()
	{
		targetHeight = transform.position.y;
		Vector3 tempPos = transform.position;
		tempPos.y = originalPosition.y + section.spawnInHeight;
		transform.position = tempPos;
	}

	public void Update()
	{
		if (!StartTimer.Instance.CanStart && LevelManager.Instance.IsGameOver)
		{
			return;
		}

		if (playerPosition.z - transform.position.z < section.distanceToAnimate)
		{
			Vector3 tempPos = transform.position;
			tempPos.y = Mathf.Lerp(tempPos.y, targetHeight, Time.deltaTime * lerpSpeed * LevelManager.Instance.SimulatedTimeScale);
			transform.position = tempPos;
		}
		else
		{
			// do nothing
		}
	}
}
