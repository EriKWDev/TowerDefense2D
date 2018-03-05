using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class Map : MonoBehaviour {

	public LineRenderer lineRenderer;
	public List<GameObject> map = new List<GameObject> ();
	public List<Vector3> vectorMap = new List<Vector3> ();

	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		UpdateMapBasedOnChildren ();
	}

	void Update () {

	}

	public void UpdateMapBasedOnChildren () {
		int i = 0;
		map.Clear ();
		vectorMap.Clear ();
		if (lineRenderer == null) {
			lineRenderer = GetComponent<LineRenderer> ();
		}

		foreach (Transform childMapPoint in transform) {
			map.Add (childMapPoint.gameObject);
			vectorMap.Add (childMapPoint.position);
			i++;
		}

		lineRenderer.positionCount = vectorMap.Count;
		lineRenderer.SetPositions (vectorMap.ToArray ());
	}

}
