using UnityEngine;
using System.Collections;

public class MovesForATime : DelayedAction {
	public bool local = true;
	public float friction = 0;
	public Vector3 velocity = Vector3.forward;
	public Vector3 randVelocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;
	public Vector3 randAcceleration = Vector3.zero;
	
	void Start() {
		velocity += Vector3.Scale(randVelocity, Random.insideUnitSphere);
		acceleration += Vector3.Scale(randAcceleration, Random.insideUnitSphere);
	}
	
	public override void Frame() {
		velocity += acceleration * Time.deltaTime;
		float mag = velocity.magnitude - friction * Time.deltaTime;
		velocity = velocity.normalized * Mathf.Max(0, mag);
		
		if (local) {
			transform.Translate(velocity * Time.deltaTime);
		} else {
			transform.position += velocity * Time.deltaTime;
		}
	}
	
	
}
