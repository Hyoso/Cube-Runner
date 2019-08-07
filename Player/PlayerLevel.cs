using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : Singleton<PlayerLevel>
{
	public int level = 1;

	private float initialLevelReq = 10;
	private float exp = 0f;
	private float curLevelReq = 0;

	[SerializeField]
	private LevelDisplay levelDisplayScript = null;

	private float basePower = 1.0f;

	public override void Start()
	{
		base.Start();
		Load();
		for (int i = 0; i != level; i++)
		{
			curLevelReq += level + (initialLevelReq * Mathf.Pow(2, level / basePower));
		}
		levelDisplayScript.InitBar(curLevelReq, exp);
		levelDisplayScript.levelUpText.text = exp >= curLevelReq ? "Level Up!" : "Level " + level.ToString();
		levelDisplayScript.button.interactable = exp >= curLevelReq;
	}

	private void CalculateLevelReq()
	{
		// exp = level + (initialLevelReq * mathf.pow(2, level / 7));
		curLevelReq += level + (initialLevelReq * Mathf.Pow(2, level / basePower) * level);
	}

	public void LevelUp()
	{
		if (exp >= curLevelReq)
		{
			PlayerStats.Instance.AddBonusCash(GetCurrentLevelUpBonus());
			level++;
			exp = 0;
			CalculateLevelReq();
			levelDisplayScript.InitBar(curLevelReq, exp);
			levelDisplayScript.SetSliderTargetVal(0);
			Save();

			levelDisplayScript.levelUpText.text = exp >= curLevelReq ? "Level Up!" : "Level " + level.ToString();
			levelDisplayScript.button.interactable = exp >= curLevelReq;
			Debug.Log("new exp:" + curLevelReq);
		}
	}

	public void AddEXP(float amount)
	{
		exp += amount;
		levelDisplayScript.SetSliderTargetVal(exp);
		
		levelDisplayScript.levelUpText.text = exp >= curLevelReq ? "Level Up!" : "Level " + level.ToString();
		levelDisplayScript.button.interactable = exp >= curLevelReq;
	}

	private void OnDestroy()
	{
		Save();
	}

	private void Save()
	{
		PlayerPrefs.SetFloat("PLAYER_EXP", exp);
		PlayerPrefs.SetInt("PLAYER_LEVEL", level);
	}

	private void Load()
	{
		exp = PlayerPrefs.GetFloat("PLAYER_EXP", 0);
		level = PlayerPrefs.GetInt("PLAYER_LEVEL", 1);
	}

	public int GetCurrentLevelUpBonus()
	{
		return (level + 1) * 1000;
	}
}
