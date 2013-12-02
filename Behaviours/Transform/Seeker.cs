using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Seeker : MonoBehaviour {
	public Transform target;
	public float speed = 1;
	public float induction = 30;
	public bool stopWhenDone = true;
	public bool startAtTarget = false;
	
	
	public float startWaitTime = 3;
	float waitTime = 3;
	
	void Start() {
		waitTime = startWaitTime;
		if (startAtTarget) {
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
	}
	
	void Restart() {
		target.SendMessage("Restart", SendMessageOptions.DontRequireReceiver);
		waitTime = startWaitTime;
		if (startAtTarget) {
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
		
	}
	
	void Update() {
		waitTime -= Time.deltaTime;
		
		Quaternion currentRotation = transform.rotation;
		transform.LookAt(target);
		transform.rotation = Quaternion.RotateTowards(currentRotation, transform.rotation,induction * Time.deltaTime);
		transform.Translate(0, 0, speed * Time.deltaTime);
		
		if (stopWhenDone && waitTime <= 0) {
			if ((transform.position - target.position).magnitude < speed * Time.deltaTime) {
				enabled = false;
			}
		}
		
	}
}
