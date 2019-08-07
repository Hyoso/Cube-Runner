using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoCube : MonoBehaviour
{
	public List<Material> materials = new List<Material>();
	public MeshRenderer meshRenderer;

	private void Start()
	{
		int chosenMat = Random.Range(0, materials.Count);
		meshRenderer.material = materials[chosenMat];
	}
}
