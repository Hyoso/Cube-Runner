using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
	public bool IsGameOver { get { return isGameOver; } }
	private bool isGameOver = false;

	public delegate void GameStateDelegate();
	public GameStateDelegate OnGameOverEvents;

	public float SimulatedTimeScale { get { return simulatedTimeScale; } }
	private float simulatedTimeScale = 1.0f;

	public override void Start()
	{
		base.Start();
		simulatedTimeScale = 1.0f;
		OnGameOverEvents += () => StartCoroutine(StartSlowMoCoroutine());

		if (UnityAds.Instance)
		{
			if (PlayerPrefs.GetInt("NextAdvertCountdown", Random.Range(2, 5)) == 0)
			{
				PlayerPrefs.SetInt("NextAdvertCountdown", Random.Range(2, 5));
				UnityAds.Instance.ShowRewardedAdWhenReady();
			}
			PlayerPrefs.SetInt("NextAdvertCountdown", PlayerPrefs.GetInt("NextAdvertCountdown", Random.Range(2, 5)) - 1);
		}
	}

	public void SetGameOver()
	{
		isGameOver = true;
		OnGameOverEvents.Invoke();
	}

	private IEnumerator StartSlowMoCoroutine()
	{
		float slowDownSpeed = 2.0f;
		while (simulatedTimeScale > 0.05f)
		{
			simulatedTimeScale = Mathf.Lerp(simulatedTimeScale, 0.0f, slowDownSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		simulatedTimeScale = 0.0f;
	}

	public void ResetGameOver()
	{
		isGameOver = false;
	}
}
