using UnityEngine;
using System.Collections;

public class AppliesUpwardForce : MonoBehaviour {
	public float force = 4;
	public ForceMode forceMode = ForceMode.Impulse;
	
	void Start() {
		transform.Require<Rigidbody>();
		rigidbody.AddForce(Vector3.up * force, forceMode);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
