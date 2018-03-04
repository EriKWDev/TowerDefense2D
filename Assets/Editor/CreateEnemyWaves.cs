using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateEnemyWaves {
	[MenuItem ("Enemy Waves/Create New Wave")]
	public static EnemyWaves Create () {
		EnemyWaves asset = ScriptableObject.CreateInstance<EnemyWaves> ();

		AssetDatabase.CreateAsset (asset, "Assets/Levels/Level 0.asset");
		AssetDatabase.SaveAssets ();
		return asset;
	}
}
