using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level 0", menuName = "Enemy Waves/Level")]
public class EnemyWaves : ScriptableObject {
	public string levelName;
	public int levelId;
	[SerializeField]
	public List<Wave> waves = new List<Wave> ();
}

[System.Serializable]
public class Wave {
	public List<EnemyType> enemies;
	public float spawnRate = 0.5f;
}

[System.Serializable]
public class EnemyType {
	public Enemy.EnemyTypes enemyType;
	public int amount;
	public GameObject enemy;
}
