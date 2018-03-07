using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower1 : Tower {

	public override void Start () {
		base.Start ();
		bullet = GameObject.Instantiate (bulletPrefab, childTransform);
		bullet.SetActive (false);
	}

	public override void Shoot () {
		if (bullet == null)
			bullet = GameObject.Instantiate (bulletPrefab, childTransform);

		bullet.SetActive (true);
		foreach (RaycastHit2D hit in Physics2D.RaycastAll (childTransform.position, childTransform.up, 20f)) {
			if (hit.collider.tag == "Enemy") {
				hit.collider.GetComponent<Enemy> ().Damage (damagePerBullet);
				GameObject p = GameObject.Instantiate (bulletParticlePrefab);
				p.transform.position = hit.point;
			}
		}
	}

	public override void StopShooting () {
		bullet.SetActive (false);
	}
}
