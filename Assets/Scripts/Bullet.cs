using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject tower;
	public GameObject currentTarget;
	public Vector3 targetDirection = Vector3.zero;
	public GameObject explosionParticle;
	public float lifeSpan = 10f;

	void Start () {
		explosionParticle.GetComponent<ParticleSystem> ().startColor = GetComponent<Renderer> ().material.color;
	}

	public void SetTarget (GameObject newTarget) {
		currentTarget = newTarget;
		targetDirection = currentTarget.transform.position - transform.position;
		targetDirection.Normalize ();
	}
		
	void Update () {
		transform.Translate (targetDirection * Time.deltaTime * 4f);
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0f) {
			GameObject.Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			other.GetComponent<Enemy> ().Damage (tower.GetComponent<Tower> ().damagePerBullet);
			Explode ();
		}
	}

	void Explode () {
		GameObject explosion = GameObject.Instantiate (explosionParticle);
		explosion.transform.position = transform.position;
		Die ();
	}

	void Die () {
		GameObject.Destroy (this.gameObject);
	}
}
