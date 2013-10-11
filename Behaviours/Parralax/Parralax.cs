using UnityEngine;
using System.Collections;

public class Parralax : MonoBehaviour {
	public float distance = 100;
	public Vector3 scales = new Vector3(1, 0, 1);
	public Vector3 offset = new Vector3(0, -10, 0);		//offset from target
	public Vector3 repeat = new Vector3(20, -1, 20); // Repeat distance of parralax.
	public Transform target;
	public bool doLate = false; //Do on late update?
	public bool grabParralaxInformationOnStart = false;
	
	void Start() {
		if (!target) { target = Camera.main.transform; }
		
		if (grabParralaxInformationOnStart) {
			GrabParralaxInformation();
			
		}
	}
	
	void Update() {
		if (target && !doLate) { Move(); }
	}
	
	void LateUpdate() {
		if (target && doLate) { Move(); }
	}
	
	void GrabParralaxInformation() {
		repeat = -Vector3.one; //We won't repeat.
		offset = transform.position - target.position;
		
	}
	
	void Move() {
		transform.position = target.position + offset;
		
		Vector3 speeds = new Vector3(1.0f/distance, 1.0f/distance, 1.0f/distance);
		speeds = Vector3.Scale(speeds, scales);
		Vector3 parralax = Vector3.Scale(speeds, target.position);
		if (repeat.x > 0) { parralax.x %= repeat.x; }
		if (repeat.y > 0) { parralax.y %= repeat.y; }
		if (repeat.z > 0) { parralax.z %= repeat.z; }
		transform.position -= parralax;
	}
}
