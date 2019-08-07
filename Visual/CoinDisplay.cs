using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
	public TextMeshProUGUI text;

	private void Start()
	{
		UpdateCoinDisplay();
	}

	public void UpdateCoinDisplay()
	{
		text.text = PlayerPrefs.GetInt("Coins", 0).ToString();
	}
}
