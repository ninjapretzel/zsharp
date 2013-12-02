using UnityEngine;
using System.Collections;

public class MovesForATime : DelayedAction {
	public Vector3 velocity = Vector3.forward;
	public Vector3 randVelocity = Vector3.zero;
	
	void Start() {
		velocity += Vector3.Scale(randVelocity, Random.insideUnitSphere);
	}
	
	public override void Frame() {
		transform.Translate(velocity * Time.deltaTime);
	}
	
	
}
