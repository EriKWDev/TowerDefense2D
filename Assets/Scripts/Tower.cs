using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public int level = 0;
	public enum TargetingMode {
		Closest,
		Furthest,
		LeastDamaged,
		MostDamaged,
		LeastDistanceTravelled,
		FurthestDistanceTravelled,
		SmartClosest,
		SingleTarget,
		Random
	}

	public TargetingMode targetingMode = TargetingMode.Closest;
	public float range = 12f;
	public int price;
	public bool selected = false;

	public GameObject currentTarget;
	public bool hasTarget = false;
	public GameObject bulletPrefab;

	public float delayBetweenShots = 0.5f;
	public float canonTurnSpeed = 20f;
	public float shootDuration = 0.01f;
	public float damagePerBullet = 2f;
	public GameObject bulletParticlePrefab;
	float t1 = 0f;
	float t2 = 0f;
	[HideInInspector]
	public GameObject bullet;
	Color gizmosColor;

	public Transform childTransform;
	Vector3 smoothDampVector;

	public virtual void Start () {
		LineDrawer = GetComponent<LineRenderer> ();
		LineDrawer.material.color = GetComponent<Renderer> ().sharedMaterial.color;
		gizmosColor = GetComponent<Renderer> ().sharedMaterial.color;
		LineDrawer.startWidth = thickness;
		LineDrawer.endWidth = thickness;
		t1 = delayBetweenShots;
		t2 = shootDuration;
		if (childTransform == null) {
			childTransform = gameObject.GetComponentInChildren<Transform> ();
		}
	}

	private bool drawOnce = false;

	void Update () {
		ShootMechanics ();
		if (selected) {
			if (!drawOnce) {
				DrawRangeCircle ();
				drawOnce = true;
			}
		} else {
			LineDrawer.positionCount = 0;
			drawOnce = false;
		}
	}

	private float ThetaScale = 0.01f;
	private int Size;
	private LineRenderer LineDrawer;
	private float Theta = 0f;
	private float thickness = 0.02f;

	public void DrawRangeCircle () {
		Theta = 0f;
		Size = (int)((1f / ThetaScale) + 1f);
		LineDrawer.positionCount = Size;
		for (int i = 0; i < Size; i++) {
			Theta += (2.0f * Mathf.PI * ThetaScale);
			float x = range * Mathf.Cos (Theta);
			float y = range * Mathf.Sin (Theta);
			LineDrawer.SetPosition (i, new Vector3 (x, y, -1f));
		}
	}

	public void OnDrawGizmos () {
		Gizmos.color = new Color (gizmosColor.r, gizmosColor.g, gizmosColor.b, 0.05f);
		Gizmos.DrawWireSphere (transform.position, range);
		if (hasTarget && currentTarget != null)
			Gizmos.DrawWireSphere (currentTarget.transform.position, 0.5f);
	}

	public void ShootMechanics () {
		Aim ();
		if (hasTarget) {
			childTransform.up = Vector3.SmoothDamp (childTransform.up, currentTarget.transform.position - childTransform.position, ref smoothDampVector, Time.deltaTime * canonTurnSpeed);

			t1 -= Time.deltaTime;
			if (t1 <= 0f) {
				Shoot ();
				t2 -= Time.deltaTime;
				if (t2 <= 0f) {
					t1 = delayBetweenShots;
					t2 = shootDuration;
					StopShooting ();
				}
			}
		} else {
			StopShooting ();
		}
	}

	public virtual void StopShooting () {

	}

	public virtual void Shoot () {
		bullet = GameObject.Instantiate (bulletPrefab);
		bullet.transform.position = transform.position;
		bullet.GetComponent<Bullet> ().tower = this.gameObject;
		//bullet.GetComponent<Bullet> ().SetTarget (currentTarget);
		bullet.GetComponent<Bullet> ().targetDirection = childTransform.up;
	}

	int enemiesInRange = 0;
	bool wasZero = false;

	public void Aim () {
		float maxValue = float.MaxValue;
		float minValue = float.MinValue;
		GameObject currentPotentialTarget = null;

		wasZero = enemiesInRange == 0;
		enemiesInRange = 0;

		switch (targetingMode) {
			case TargetingMode.Closest:
			default:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						enemiesInRange++;
						float tmpDist = (transform.position - c.transform.position).sqrMagnitude;
						if (tmpDist < maxValue) {
							maxValue = tmpDist;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.Furthest:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						enemiesInRange++;
						float tmpDist = (transform.position - c.transform.position).sqrMagnitude;
						if (tmpDist > minValue) {
							minValue = tmpDist;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.LeastDamaged:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						enemiesInRange++;
						float tmpLife = c.GetComponent<Enemy> ().life;
						if (tmpLife < maxValue) {
							maxValue = tmpLife;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.MostDamaged:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						enemiesInRange++;
						float tmpLife = c.GetComponent<Enemy> ().life;
						if (tmpLife > minValue) {
							minValue = tmpLife;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.LeastDistanceTravelled:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						enemiesInRange++;
						float tmpDist = c.GetComponent<Enemy> ().arbitraryDistanceTravelled;
						if (tmpDist < maxValue) {
							maxValue = tmpDist;
							currentPotentialTarget = c.gameObject;
						}
					}
				}
				break;

			case TargetingMode.FurthestDistanceTravelled:
				foreach (Collider2D c in Physics2D.OverlapCircleAll (transform.position, range)) {
					if (c.tag == "Enemy") {
						enemiesInRange++;
						float tmpDist = c.GetComponent<Enemy> ().arbitraryDistanceTravelled;
						if (tmpDist > minValue) {
							minValue = tmpDist;
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
			if (enemiesInRange > 0 && wasZero) {
				t1 = 0f;
			}
		}
	}
}
