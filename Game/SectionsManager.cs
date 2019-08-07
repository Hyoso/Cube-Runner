using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionsManager : Singleton<SectionsManager>
{
	public List<Section> sectionsList = new List<Section>();

	private int lastSection = 1;
	public override void Start()
	{
		base.Start();
		sectionsList[0].isAnimating = true;
		sectionsList[1].isAnimating = true;
	}

	public void SelectNewSection()
	{
		Debug.Log("new section: " + (lastSection + 1));
		SetupNextSection(lastSection + 1 >= sectionsList.Count ? 1 : (lastSection + 1));
	}

	private void SetupNextSection(int nextSectionId)
	{
		sectionsList[nextSectionId].isAnimating = true;
		Vector3 newPos = sectionsList[nextSectionId].transform.position;
		newPos.z = sectionsList[lastSection].transform.position.z - sectionsList[lastSection].sectionLength;
		Debug.Log("newpos: " + newPos);

		sectionsList[nextSectionId].transform.position = newPos;
		sectionsList[nextSectionId].ResetTrapCubes();
		lastSection = nextSectionId;
	}
}
