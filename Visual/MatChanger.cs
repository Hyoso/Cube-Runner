using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatChanger : MonoBehaviour
{
	[SerializeField]
	private Material groundMaterial;

	[SerializeField]
	private UIGradient gameBgGradient;

	public List<Gradient> gradientsList = new List<Gradient>();
	private int curGradIndex = 0;

	private void Start()
	{
		StartCoroutine(DelayedStart());
		StartCoroutine(NextGradientCoroutine());
		ResetColours();
	}

	private void OnDisable()
	{
		ResetColours();
	}

	private void ResetColours()
	{
		groundMaterial.SetColor("_OverTopColor", gradientsList[0].colorKeys[0].color);
		groundMaterial.SetColor("_GradientTopColor", gradientsList[0].colorKeys[0].color);

		groundMaterial.SetColor("_BelowBottomColor", gradientsList[0].colorKeys[gradientsList[0].colorKeys.Length - 1].color);
		groundMaterial.SetColor("_GradientBottomColor", gradientsList[0].colorKeys[gradientsList[0].colorKeys.Length - 1].color);
	}

	private IEnumerator DelayedStart()
	{
		yield return new WaitForEndOfFrame();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			NextGradient();
		}
	}
	
	public void NextGradient()
	{
		curGradIndex = curGradIndex + 1 >= gradientsList.Count ? curGradIndex : curGradIndex + 1;
	}

	public IEnumerator NextGradientCoroutine()
	{

		float timer = 0.0f;
		float lerpSpeed = 2.0f;
		while (true)
		{
			Gradient nexGrad = gradientsList[curGradIndex];

			Color topCol = groundMaterial.GetColor("_GradientTopColor");
			Color botCol = groundMaterial.GetColor("_GradientBottomColor");

			topCol = Color.Lerp(topCol, nexGrad.colorKeys[0].color, Time.deltaTime * lerpSpeed);
			botCol = Color.Lerp(botCol, nexGrad.colorKeys[nexGrad.colorKeys.Length - 1].color, Time.deltaTime * lerpSpeed);

			// get the materials colours
			groundMaterial.SetColor("_OverTopColor", topCol);
			groundMaterial.SetColor("_GradientTopColor", topCol);

			groundMaterial.SetColor("_BelowBottomColor", botCol);
			groundMaterial.SetColor("_GradientBottomColor", botCol);

			// setting ui image colours
			gameBgGradient.SetColours(topCol, botCol);

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
