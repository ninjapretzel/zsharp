using UnityEngine;
using System.Collections;

public class LerpsToTarget : MonoBehaviour {
	public Transform target;
	public float moveSpeed = 10.0f;
	public float rotateSpeed = 10.0f;
	public bool lerpToPosition = true;
	public bool lerpToRotation = true;
	public bool startAtTarget = true;
	
	public UpdateType updateType = UpdateType.FixedUpdate;
	
	public Vector3 offset = Vector3.zero;
	
	public float proximity { 
		get { return (target.position - transform.position).magnitude; }
	}
	public float distance {
		get { return proximity; }
	}
	
	void Start() {
		if (startAtTarget) { Finish(); }
	}
	
	void Update() {
		if (updateType == UpdateType.Update) {
			Move(Time.deltaTime);
		}
	}
	
	void LateUpdate() {
		if (updateType == UpdateType.LateUpdate) {
			Move(Time.deltaTime);
		}
	}
	
	void FixedUpdate() {
		if (updateType == UpdateType.FixedUpdate) {
			Move(Time.fixedDeltaTime);
		}
	}
	
	
	public void Move(float time) {
		if (target != null) {
			if (lerpToPosition) {
				transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * time);
			}
			if (lerpToRotation) {
				transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotateSpeed * time);
			}
		}
	}
	
	
	public void Finish() {
		transform.position = target.position + offset;
	}
}
