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

	private bool xIsHigher, yIsHigher = false;

	public enum EnemyTypes {
		Zero,
		Square,
		Poly
	};

	void Start () {
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

	public void Die () {
		GameObject.Destroy (gameObject);
	}

	public void ReachEnd () {
		GameManager.instance.EnemyReachedEnd ();
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
