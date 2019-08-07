using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrail : MonoBehaviour
{
	public const float velocity = 5.0f;

	private void Update()
	{
		if (!StartTimer.Instance.CanStart && !LevelManager.Instance.IsGameOver)
		{
			return;
		}
		transform.Translate(Vector3.forward * velocity * Time.deltaTime * LevelManager.Instance.SimulatedTimeScale);
	}
}
