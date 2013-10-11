using UnityEngine;
using System.Collections;

public class LookAcrossGravity : MonoBehaviour {
	Vector3 rotationAfter = Vector3.zero;
	
	void Awake() {
		transform.forward = -Physics.gravity.normalized;
		transform.Rotate(rotationAfter);
	}
	
}