using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MovesToPositionOnScreen : MonoBehaviour {
	public Vector3 point;
	
	void Update() {
		transform.position = Camera.main.transform.position + Camera.main.transform.TransformDirection(point);
		
	}
	
}
