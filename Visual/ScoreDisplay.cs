using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : Singleton<ScoreDisplay>
{
	public delegate void OnPointIncrementDelegate();
	public OnPointIncrementDelegate OnPointIncrementEvents;

	public int score = 0;

	private TextMeshProUGUI scoreText;
	private Animator scoreTextAnimator;

	public Color leftHalfColour;
	public Color rightHalfColour;

	public MatChanger matChanger;
	private int nextColourchangeScore = 0;
	private int minColourChangeScore = 6;
	private int maxColourChangeScore = 9;

	public override void Start()
	{
		base.Start();
		nextColourchangeScore = Random.Range(minColourChangeScore, maxColourChangeScore);
		OnPointIncrementEvents += UpdateColours;
		OnPointIncrementEvents += () => scoreTextAnimator.SetTrigger("Explode");
		OnPointIncrementEvents += PollGameColourUpdate;
		StartCoroutine(DelayedStartCoroutine());
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.P))
		{
			IncrementScore();
		}
	}

	private IEnumerator DelayedStartCoroutine()
	{
		yield return new WaitForEndOfFrame();
		scoreText = GameCanvasObjects.Instance.scoreText;
		scoreTextAnimator = GameCanvasObjects.Instance.scoreTextAnimator;

		OnPointIncrementEvents += AudioMgr.Instance.PlayPointIncrementSound;

		ForceUpdateColours();
	}

	public void ForceUpdateColours()
	{
		UpdateColours();
	}

	private void UpdateColours()
	{
		string endStr = "";
		string scoreStr = score.ToString();
		if (scoreStr.Length > 1)
		{
			int leftHalfEnd = (scoreStr.Length / 2) + ((scoreStr.Length % 2) == 0 ? 0 : 1);
			endStr = "<color=#" + ColorUtility.ToHtmlStringRGBA(leftHalfColour) + ">" + scoreStr.Substring(0, leftHalfEnd) + "</color>";
			endStr += "<color=#" + ColorUtility.ToHtmlStringRGBA(rightHalfColour) + ">" + scoreStr.Substring(leftHalfEnd, scoreStr.Length - leftHalfEnd) + "</color>";
		}
		else
		{
			endStr = "<color=#" + ColorUtility.ToHtmlStringRGBA(rightHalfColour) + ">" + scoreStr + "</color>";
		}

		scoreText.text = endStr;
	}

	private IEnumerator DelayedUpdateTextColoursCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		UpdateColours();
	}

	private void PollGameColourUpdate()
	{
		if ((score % nextColourchangeScore) == 0)
		{
			matChanger.NextGradient();
			nextColourchangeScore = Random.Range(minColourChangeScore, maxColourChangeScore);
		}
	}

	public void IncrementScore()
	{
		score++;
		OnPointIncrementEvents.Invoke();
		StartCoroutine(DelayedUpdateTextColoursCoroutine());
	}

}
