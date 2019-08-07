using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotator : MonoBehaviour
{
	public Vector3 rotateVector = Vector3.zero;
	public float rotateSpeed = 2.0f;
	public Section section = null;

	private void Start()
	{
		
	}

	private IEnumerator RotateAnimationCoroutine()
	{
		while (true)
		{
			if (!StartTimer.Instance.CanStart && LevelManager.Instance.IsGameOver)
			{
				transform.Rotate(rotateVector, Time.deltaTime * rotateSpeed);
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
