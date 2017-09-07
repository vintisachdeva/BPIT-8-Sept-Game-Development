using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBirdScript : MonoBehaviour {

	public float maxStretch = 3f;
	public LineRenderer catapultPositionFront;
	public LineRenderer catapultPositionBack;

	private SpringJoint2D spring;
	private Transform catapultPosition;
	private Ray rayToMouse;
	private float maxStretchSqr;
	private Ray leftcatapultPositionRay;
	private float circleRadius;
	private bool pressedOn;
	private Vector2 preVelocity;


	void Awake(){
		spring = GetComponent<SpringJoint2D> ();
		catapultPosition = spring.connectedBody.transform;
	}

	// Use this for initialization
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray (catapultPosition.position, Vector3.zero);
		leftcatapultPositionRay = new Ray (catapultPositionFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
		circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (pressedOn) {
			Dragging ();
		}
		if (spring != null) {
			if (!GetComponent<Rigidbody2D> ().isKinematic && preVelocity.sqrMagnitude > GetComponent<Rigidbody2D> ().velocity.sqrMagnitude) {
				Destroy (spring);
				GetComponent <Rigidbody2D> ().velocity = preVelocity;
			}
			if (!pressedOn)
				preVelocity = GetComponent<Rigidbody2D> ().velocity;
			
			LineRendererUpdate ();

		} else {
				catapultPositionFront.enabled = false;
				catapultPositionBack.enabled = false;
			}
	}

	void LineRendererSetup(){
		catapultPositionFront.SetPosition (0, catapultPositionFront.transform.position);
		catapultPositionBack.SetPosition (0, catapultPositionBack.transform.position);

		catapultPositionFront.sortingLayerName = "Foreground";
		catapultPositionBack.sortingLayerName = "Foreground";
	
		catapultPositionFront.sortingOrder = 3;
		catapultPositionBack.sortingOrder = 1;
	}

	void OnMouseDown(){
		spring.enabled = false;
		pressedOn = true;
	}

	void OnMouseUp(){
		spring.enabled = true;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		pressedOn = false;
	}

	void Dragging(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultPositionToMouse = mouseWorldPoint - catapultPosition.position;

		if (catapultPositionToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultPositionToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	
	}

	void LineRendererUpdate(){
	
		Vector2 catapultPositionToProjectile = transform.position - catapultPositionFront.transform.position;
		leftcatapultPositionRay.direction = catapultPositionToProjectile;
		Vector3 holdPoint = leftcatapultPositionRay.GetPoint (catapultPositionToProjectile.magnitude + circleRadius);
		catapultPositionFront.SetPosition (1, holdPoint);
		catapultPositionBack.SetPosition (1, holdPoint);
	}
}
