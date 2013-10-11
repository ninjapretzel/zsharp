using UnityEngine;
using System.Collections;

public class FakeParent : MonoBehaviour {
	public Transform target;
	public Vector3 offset = Vector3.zero;
	public bool doLate = false;
	
	void Update() {
		if (doLate) { return; }
		Move();
	}
	
	void LateUpdate() {
		if (!doLate) { return; }
		Move();
	}
	
	void Move() {
		transform.position = target.position + offset;
	}
}
