using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Map))]
public class MapEditor : Editor {

	public override void OnInspectorGUI () {

		Map targetMap = (Map)target;

		if (GUILayout.Button ("Update Map")) {
			targetMap.UpdateMapBasedOnChildren ();
		}

		DrawDefaultInspector ();
	}

}
