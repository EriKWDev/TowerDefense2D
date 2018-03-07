using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2 : Tower {

	public float stunnedTime = 3f;

	public override void Shoot () {
		GameObject.Instantiate (bulletParticlePrefab, this.transform);
		foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
			if (c.tag == "Enemy") {
				StartCoroutine(c.GetComponent<Enemy> ().SlowDown (damagePerBullet * (c.GetComponent<Enemy> ().speed / 3f), stunnedTime));
			}
		}
	}
}
