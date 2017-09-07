using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyScript : MonoBehaviour {

	public int hitTimes = 2;
	public Sprite damagedSprite;
	public float damageImpactSpeed;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentHitPoints = hitTimes;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	}


	void OnCollisionEnter2D(Collision2D collision2D){
		if (collision2D.collider.tag != "Damage") {
			return;
		}
		if (collision2D.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) {
			return;
		}

		spriteRenderer.sprite = damagedSprite;
		currentHitPoints--;

		if (currentHitPoints <= 0) {
			Kill ();
		}
	}

	void Kill(){
		spriteRenderer.enabled = false;
		GetComponent<Collider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().isKinematic = true;
	}
}
