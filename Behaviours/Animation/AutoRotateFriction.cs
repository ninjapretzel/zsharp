using UnityEngine;
using System.Collections;

public class AutoRotateFriction : MonoBehaviour {
	public float friction;
	private AutoRotate rotator;
	
	void Start() {
		rotator = GetComponent<AutoRotate>();
		if (rotator == null) { Debug.Log("AutoRotate not found on " + name); Destroy(this); }
	}
		
		
	void Update() {
		float magnitude = rotator.speed.magnitude;
		magnitude = Mathf.Max(magnitude - friction * Time.deltaTime, 0);
		rotator.speed = rotator.speed.normalized * magnitude;
	}
	
}
