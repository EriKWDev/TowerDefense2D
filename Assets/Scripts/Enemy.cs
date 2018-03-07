using System.Collections;
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
	//[HideInInspector]
	public float arbitraryDistanceTravelled = 0f;

	public bool xIsHigher, yIsHigher = false;
	Vector3 lastPosition;

	public enum EnemyTypes {
		Zero,
		Square,
		Poly
	};

	void Start () {
		realSpeed = speed;
		transform.position = GameManager.instance.mapReference.vectorMap[currentMapPointIndex];
		lastPosition = transform.position;
		GetNextMapPoint ();
	}

	void Update () {
		arbitraryDistanceTravelled += Vector3.Distance(lastPosition, transform.position);
		lastPosition = transform.position;
		transform.Translate (direction * (speed / 100f) * Time.deltaTime);

		if ((xIsHigher == (nextMapPoint.x < transform.position.x)) && (yIsHigher == (nextMapPoint.y < transform.position.y))) {
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

	public IEnumerator SlowDown (float amount, float delay, float minSpeed) {
		timesDelayed++;
		float t = delay;
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
			Vector3 tmpLastPoint = nextMapPoint;

			if (currentMapPointIndex > 0)
				transform.position = tmpLastPoint;

			nextMapPoint = GameManager.instance.mapReference.vectorMap[currentMapPointIndex];
			direction = (nextMapPoint - transform.position).normalized * 5f;

			xIsHigher = (nextMapPoint.x > tmpLastPoint.x);
			yIsHigher = (nextMapPoint.y > tmpLastPoint.y);

			currentMapPointIndex++;
		} else {
			currentMapPointIndex++;
			ReachEnd ();
		}
	}
}
