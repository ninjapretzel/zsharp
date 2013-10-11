using UnityEngine;
using System.Collections;

public class FakeParticle : MonoBehaviour {
	public FakeParticleEmitter system;
	public Billboard billboard;
	
	public bool live = false;
	
	public float size;
	public float energy;
	
	public Vector3 localVelocity;
	public Vector3 force;
	public Vector3 velocity;
	
	public float angularVelocity;
	public float rotation;
	
	
	
	void Start() {
		if (system.simulateInWorldSpace) { transform.parent = system.worldSpace; }
		else { transform.parent = system.transform; }
	}
	
	public void Tick() {
		if (!live) { return; }
		energy -= Time.deltaTime;
		if (energy <= 0) { Die(); return; }
		
		Vector3 offset = velocity + system.transform.TransformDirection(localVelocity);
		
		transform.position += offset * Time.deltaTime;
		
		velocity += force * Time.deltaTime;
		
		rotation += angularVelocity * Time.deltaTime;
		billboard.zrotation = rotation;
	}
	
	void Die() {
		if (!live) { return; }
		//system.deadParticles.Enqueue(this);
		system.liveParticles -= 1;
		live = false;
		renderer.enabled = false;
		
		
	}
	
}
