using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

	public float time = 10f;
	public float t;
	public static GameManager instance;
	public Map mapReference;
	[SerializeField]
	public List<EnemyType> enemies = new List<EnemyType> ();
	public GameObject endParticles;
	public GameObject livesPrefab;
	public int lives = 10;
	private GameObject firstMapPoint;
	private GameObject lastMapPoint;
	private GameObject livesText;
	public EnemyWaves enemyWaves;
	public int currentWave = 0;

	void Awake () {
		instance = GetComponent<GameManager> ();
	}

	void Start () {
		t = time;
		if (mapReference == null) {
			mapReference = GameObject.Find ("Map").GetComponent<Map> ();
		}

		firstMapPoint = mapReference.map[0];
		lastMapPoint = mapReference.map[mapReference.map.Count - 1];

		livesText = GameObject.Instantiate (livesPrefab, lastMapPoint.transform);
		StartCoroutine (SpawnWave ());
	}

	void Update () {
		t -= Time.deltaTime;
		if (t <= 0f) {
			t = time;
			StartCoroutine (SpawnWave ());
		}
	}

	public IEnumerator SpawnWave () {
		int cw = currentWave;
		if (currentWave < enemyWaves.waves.Count) {
			foreach (EnemyType enemyType in enemyWaves.waves[cw].enemies) {
				for (int i = 0; i < enemyType.amount; i++) {
					GameObject enemy = Instantiate (FindEnemyObjectOfType (enemyType.enemyType), this.transform);
					enemy.transform.position = new Vector3 (firstMapPoint.transform.position.x, firstMapPoint.transform.position.y, 0f);
					yield return new WaitForSeconds (enemyWaves.waves[cw].spawnRate);
				}
			}

			currentWave++;
		}
	}

	public GameObject FindEnemyObjectOfType (Enemy.EnemyTypes enemyType) {
		foreach (EnemyType et in enemies) {
			if (et.enemyType == enemyType) {
				return et.enemy;
			}
		}

		Debug.LogError ("Could not find Enemy of type : " + enemyType.ToString () + ". Will spawn default enemy instead.");
		return enemies[0].enemy;
	}

	public void EnemyReachedEnd (Color color) {
		if (lives > 0) {
			lives -= 1;
		}

		livesText.GetComponent<TextMeshPro> ().text = "" + lives;

		ParticleSystem.MainModule explosionParticleMain = endParticles.GetComponent<ParticleSystem> ().main;
		explosionParticleMain.startColor = color;
		GameObject.Instantiate (endParticles, lastMapPoint.transform);
	}
}
