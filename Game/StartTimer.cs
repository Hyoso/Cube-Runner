using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : Singleton<StartTimer>
{
	public float secondsToWait = 1;

	public bool CanStart { get { return canStart; } }
	private bool canStart = false;

	public override void Start()
	{
		base.Start();
		StartCoroutine(StartTimerCoroutine());
	}

	private IEnumerator StartTimerCoroutine()
	{
		yield return new WaitForSeconds(2.0f);			

		canStart = true;
	}
}
