using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCanvasObjects : Singleton<GameCanvasObjects>
{
	public TextMeshProUGUI winText;
	public TextMeshProUGUI loseText;
	public TextMeshProUGUI scoreText;
	public Animator scoreTextAnimator;
	public PlayerInputBtn playerInputBtn;
	public CanvasGroup gameOverPanel;
}
