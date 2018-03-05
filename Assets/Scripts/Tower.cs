using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public int level = 0;
	public enum TargetingMode {
		Closest,
		Furthest,
		MostDamaged,
		LeastDamaged,
		SingleTarget,
		Random
	}

	public TargetingMode targetingMode = TargetingMode.Closest;
	public float range = 12f;
	public int price;

	public GameObject currentTarget;
	public bool hasTarget = false;
	public GameObject bulletPrefab;

	public float delayBetweenShots = 0.5f;
	public float canonTurnSpeed = 20f;
	public float shootDuration = 0.01f;
	public float damagePerBullet = 2f;
	float t1 = 0f;
	float t2 = 0f;
	Color gizmosColor;

	public Transform childTransform;

	void Start () {
		gizmosColor = GetComponent<Renderer> ().sharedMaterial.color;
		t1 = delayBetweenShots;
		t2 = shootDuration;
	}

	void Update () {
		ShootMechanics ();
	}

	public void OnDrawGizmos () {
		Gizmos.color = new Color (gizmosColor.r, gizmosColor.g, gizmosColor.b, 0.2f);
		Gizmos.DrawWireSphere (transform.position, range);
		if (hasTarget && currentTarget != null)
			Gizmos.DrawWireSphere (currentTarget.transform.position, 0.5f);
	}

	public void ShootMechanics () {
		Aim ();
		if (hasTarget) {
			childTransform.up = Vector3.Lerp (childTransform.up, currentTarget.transform.position - childTransform.position, Time.deltaTime * canonTurnSpeed);

			t1 -= Time.deltaTime;
			if (t1 <= 0f) {
				Shoot ();
				t2 -= Time.deltaTime;
				if (t2 <= 0f) {
					t1 = delayBetweenShots;
					t2 = shootDuration;
				}

			}
		}
	}

	public void Shoot () {
		// if (!Application.isEditor) {
		GameObject bullet = GameObject.Instantiate (bulletPrefab);
		bullet.transform.position = transform.position;
		bullet.GetComponent<Bullet> ().tower = this.gameObject;
		//bullet.GetComponent<Bullet> ().SetTarget (currentTarget);
		bullet.GetComponent<Bullet> ().targetDirection = childTransform.up;
		// }
	}

	public void Aim () {
		float maxDist = float.MaxValue;
		float minDist = float.MinValue;
		GameObject currentPotentialTarget = null;

		switch (targetingMode) {
			case TargetingMode.Closest:
			default:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						float tmpDist = (transform.position - c.transform.position).sqrMagnitude;
						if (tmpDist < maxDist) {
							maxDist = tmpDist;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.Furthest:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						float tmpDist = (transform.position - c.transform.position).sqrMagnitude;
						if (tmpDist > minDist) {
							minDist = tmpDist;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.LeastDamaged:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						float tmpLife = c.GetComponent<Enemy> ().life;
						if (tmpLife < maxDist) {
							minDist = tmpLife;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.MostDamaged:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						float tmpLife = c.GetComponent<Enemy> ().life;
						if (tmpLife > minDist) {
							minDist = tmpLife;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

		}

		if (currentPotentialTarget == null) {
			hasTarget = false;
			currentTarget = null;
		} else {
			currentTarget = currentPotentialTarget;
			hasTarget = true;
		}
	}
}
