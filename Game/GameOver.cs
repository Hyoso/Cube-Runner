using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
	public float showSpeed = 2.0f;

	public PPBlurController blurController;

	private CanvasGroup gameOverPanel;

	private void Start()
	{
		StartCoroutine(DelayedStartCoroutine());
	}

	private IEnumerator DelayedStartCoroutine()
	{
		yield return new WaitForEndOfFrame();

		gameOverPanel = GameCanvasObjects.Instance.gameOverPanel;

		LevelManager.Instance.OnGameOverEvents += () => StartCoroutine(DelayedShowGameOverPanelCoroutine());
	}

	private IEnumerator DelayedShowGameOverPanelCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(ShowGameOverPanelCoroutine());
		blurController.StartBlurAnimation();
	}

	private IEnumerator ShowGameOverPanelCoroutine()
	{
		gameOverPanel.enabled = true;
		gameOverPanel.blocksRaycasts = true;
		gameOverPanel.interactable = true;

		while (gameOverPanel.alpha < 1.0f)
		{
			gameOverPanel.alpha += Time.deltaTime * showSpeed;
			yield return new WaitForEndOfFrame();
		}

		gameOverPanel.alpha = 1.0f;
	}
}
