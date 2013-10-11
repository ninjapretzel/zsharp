using UnityEngine;
using System.Collections;

public class LerpsToTarget : MonoBehaviour {

	public Transform target;
	public float moveSpeed = 10.0f;
	public float rotateSpeed = 10.0f;
	public bool lerpToPosition = true;
	public bool lerpToRotation = true;
	
	public Vector3 offset = Vector3.zero;
	
	public float proximity { 
		get { return (target.position - transform.position).magnitude; }
	}
	
	void Start() {
		
	}
	
	void FixedUpdate() {
		if(target) {
			if(lerpToPosition) {
				transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.fixedDeltaTime);
			}
			if(lerpToRotation) {
				transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotateSpeed * Time.fixedDeltaTime);
			}
		}
		
	}
	
	
	
	public void Finish() {
		transform.position = target.position + offset;
	}
}
