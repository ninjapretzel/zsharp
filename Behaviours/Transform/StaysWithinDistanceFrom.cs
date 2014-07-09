using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaysWithinDistanceFrom : MonoBehaviour {
	
	public float distance = 5;
	public Transform target;
	public bool flatY = true;
	public bool doLate = false;
	
	void Start() {
		
	}
	
	void Update() {
		if (!doLate) {
			Move();
		}
	}
	
	void LateUpdate() {
		if (doLate) {
			Move();
		}
	}
	
	void Move() {
		if (target) {
			Vector3 diff = transform.position - target.position;
			if (flatY) { diff.y = 0; }
			Vector3 direction = diff.normalized;
			
			
			
			if (diff.magnitude > distance) { 
				transform.position = target.position + direction * distance;
			}
			
		}
	}
	
}
