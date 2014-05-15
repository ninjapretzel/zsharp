using UnityEngine;
using System.Collections;

public class AppliesRandomForce : MonoBehaviour {
	public float force = 2;
	public ForceMode forceMode = ForceMode.Impulse;
	
	void Start() {
		transform.Require<Rigidbody>();
		rigidbody.AddForceAtPosition(Random.insideUnitSphere * force, Random.insideUnitSphere, forceMode);
		rigidbody.AddForceAtPosition(Random.insideUnitSphere * force, Random.insideUnitSphere, forceMode);
		rigidbody.AddForceAtPosition(Random.insideUnitSphere * force, Random.insideUnitSphere, forceMode);
		Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
