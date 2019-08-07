using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerStats : Singleton<PlayerStats>
{
	[SerializeField]
	private float CurrentCash;
	public float startingCash = 5;

	public TextMeshProUGUI currentCashText;

	private bool cashChanged = true;
	public PlayerLevel level;

	public DateTime lastExitTime;
	public DateTime lastCheckedUTCTime;
	public int maxOfflineEarnings;
	public int maxOfflineEarningsLevel;

	public bool LoadedInitialTimer = false;

	public bool hasAutoUnlockedItem = false;

	public override void Start()
	{
		base.Start();

		LoadStats();
	}

	private void Update()
	{
		if (cashChanged)
		{
			//currentCashText.text = CurrentCash.ToString();
			currentCashText.text = CurrentCash.ToCondensedNumber();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			AddCash(1000000);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			AddCash(100000000000);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			AddCash(float.MaxValue);
		}
	}

	public float GetCurrentCash()
	{
		return CurrentCash;
	}

	public void AddBonusCash(float amount)
	{
		if (CurrentCash + amount > float.MaxValue)
		{
			CurrentCash = float.MaxValue;
		}
		else
		{
			CurrentCash += amount;
		}
		cashChanged = true;
	}

	public void AddCash(float amount)
	{
		if (CurrentCash + amount > float.MaxValue)
		{
			CurrentCash = float.MaxValue;
		}
		else
		{
			CurrentCash += amount;
		}
		cashChanged = true;
		level.AddEXP(amount);
	}

	public void SubCash(float amount)
	{
		CurrentCash -= amount;
		cashChanged = true;
	}

	public void UpdateLastCheckedUTCTime(DateTime newTime)
	{
		lastCheckedUTCTime = newTime;
		SaveStats();
		LoadedInitialTimer = true;
	}

	private void OnDestroy()
	{
#if UNITY_ANDROID
		if (hasAutoUnlockedItem)
		{
			//Assets.SimpleAndroidNotifications.NotificationManager.CancelAll();
			//Assets.SimpleAndroidNotifications.NotificationManager.SendWithAppIcon(new TimeSpan(0, 30, 0), "Best Idle Cooking", "COLLECT YOUR OFFLINE EARNINGS!", Color.white, Assets.SimpleAndroidNotifications.NotificationIcon.Heart);
		}
#endif
		SaveStats();
	}

	private void LoadStats()
	{
		CurrentCash = PlayerPrefs.GetFloat("CURRENT_CASH", startingCash);
		lastExitTime = DateTime.Parse(PlayerPrefs.GetString("LAST_EXIT_TIME", lastCheckedUTCTime.ToString()));
		maxOfflineEarnings = PlayerPrefs.GetInt("MAX_OFFLINE_EARNINGS", 100);
		maxOfflineEarningsLevel = PlayerPrefs.GetInt("MAX_OFFLINE_EARNINGS_LEVEL", 0);
	}

	private void SaveStats()
	{
		PlayerPrefs.SetFloat("CURRENT_CASH", CurrentCash);
		PlayerPrefs.SetString("LAST_EXIT_TIME", lastCheckedUTCTime.ToString());
		PlayerPrefs.SetInt("MAX_OFFLINE_EARNINGS", maxOfflineEarnings);
		PlayerPrefs.SetInt("MAX_OFFLINE_EARNINGS_LEVEL", maxOfflineEarningsLevel);
	}
}
