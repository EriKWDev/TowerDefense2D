﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField]
	private int currentMapPointIndex = 0;
	public Vector3 nextMapPoint;
	public Vector3 direction;
	public float life;
	[Range (1f, 100f)]
	public float speed;
	public GameObject deathParticlePrefab;
	[HideInInspector]
	public float realSpeed;

	private bool xIsHigher, yIsHigher = false;

	public enum EnemyTypes {
		Zero,
		Square,
		Poly
	};

	void Start () {
		realSpeed = speed;
		transform.position = GameManager.instance.mapReference.vectorMap[currentMapPointIndex];
		GetNextMapPoint ();
	}

	void Update () {
		transform.Translate (direction * (speed / 100f) * Time.deltaTime);

		if (xIsHigher == (nextMapPoint.x < transform.position.x) && yIsHigher == (nextMapPoint.y < transform.position.y)) {
			GetNextMapPoint ();
		}
	}

	public void Damage (float damage) {
		if (life - damage > 0f) {
			life -= damage;
		} else {
			Die ();
		}
	}

	int timesDelayed = 0;

	public IEnumerator SlowDown (float amount, float delay) {
		timesDelayed++;
		float t = delay;
		float minSpeed = 0.3f;
		speed = (speed - amount > minSpeed ? speed - amount : minSpeed);
		while (t > 0f) {
			yield return new WaitForEndOfFrame ();
			t -= Time.deltaTime;
		}
		timesDelayed--;
		if (timesDelayed <= 0) {
			timesDelayed = 0;
			speed = realSpeed;
		}
	}

	public void Die () {
		deathParticlePrefab.transform.position = transform.position;
		ParticleSystem.MainModule explosionParticleMain = deathParticlePrefab.GetComponent<ParticleSystem> ().main;
		explosionParticleMain.startColor = GetComponent<Renderer> ().material.color;
		GameObject.Instantiate (deathParticlePrefab);
		GameObject.Destroy (gameObject);
	}

	public void ReachEnd () {
		GameManager.instance.EnemyReachedEnd (GetComponent<Renderer> ().material.color);
		Die ();
	}

	public void GetNextMapPoint () {
		if (currentMapPointIndex < GameManager.instance.mapReference.map.Count) {
			nextMapPoint = GameManager.instance.mapReference.vectorMap[currentMapPointIndex];
			direction = (nextMapPoint - transform.position).normalized * 5f;

			xIsHigher = nextMapPoint.x > transform.position.x;
			yIsHigher = nextMapPoint.y > transform.position.y;

			currentMapPointIndex++;
		} else {
			currentMapPointIndex++;
			ReachEnd ();
		}
	}
}
