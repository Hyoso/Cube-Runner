using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCube : MonoBehaviour
{
	[SerializeField]
	private Cube myCube;

	[SerializeField]
	private Material goldMat;
	[SerializeField]
	private Material originalMat;

	private float myCubesHeight;
	private float yOffset;

	public MeshRenderer meshRenderer = null;

	private void Start()
	{
		myCubesHeight = myCube.transform.localScale.y * 0.5f;
		yOffset = Mathf.Abs(transform.position.y - myCube.OriginalPosition.y);
	}

	public void Update()
	{
		if (!StartTimer.Instance.CanStart && LevelManager.Instance.IsGameOver)
		{
			return;
		}

		Vector3 tempPos = myCube.transform.position;
		tempPos.y += yOffset;
		transform.position = tempPos;
	}

	public void SetOriginalMat()
	{
		meshRenderer.material = originalMat;
	}

	public void SetCompletedMat()
	{
		meshRenderer.material = goldMat;
	}
}
