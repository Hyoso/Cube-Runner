using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class UnityAds : Singleton<UnityAds>
{
	public UnityEvent onAdFinishedEvents;
	public bool adIsReady = false;

	private Coroutine waitForAdCoroutine;

	public override void Start()
	{
		base.Start();
	}

	public void ShowInterstitialAdWhenReady()
	{

	}

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	public void ShowRewardedAdWhenReady()
	{
		adIsReady = Advertisement.IsReady("rewardedVideo");
		waitForAdCoroutine = StartCoroutine(WaitForAdCoroutine());
	}

	IEnumerator WaitForAdCoroutine()
	{
		while (!(adIsReady = Advertisement.IsReady("rewardedVideo")))
		{
			bool b = Advertisement.isInitialized;
			yield return new WaitForEndOfFrame();
		}

		var options = new ShowOptions { resultCallback = HandleShowResult };
		Advertisement.Show("rewardedVideo", options);
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
			case ShowResult.Finished:
				onAdFinishedEvents.Invoke();
				onAdFinishedEvents.RemoveAllListeners();
				break;
			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
		}
	}
}
