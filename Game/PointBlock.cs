using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBlock : MonoBehaviour
{
	public List<TrapCube> myTrapCubes = new List<TrapCube>();

	public void SetTrapCubeGold()
	{
		myTrapCubes.ForEach(x => x.SetCompletedMat());
	}
}
