using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour {

	public Rigidbody2D projectile;
	public float resetSpeed = 0.025f;

	private float resetSpeedSqr;
	private SpringJoint2D spring;

	void Start () {
		resetSpeed = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D> ();
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Reset ();
		}

		if(spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr){
			Reset ();
		}
	}

	void OnTriggerExit2D(Collider2D collider2D){
		if (collider2D.GetComponent <Rigidbody2D> () == projectile) {
			Reset ();
		}
	}

	void Reset(){
		SceneManager.LoadScene ("Game");
	}
}
