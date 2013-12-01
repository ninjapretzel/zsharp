using UnityEngine;
using System.Collections;

public class OscillateRotation : MonoBehaviour {
	private Quaternion initial;
	public Oscillator[] oscis;
	public Vector3[] rotations;
	
	public bool	doLocal = false;
	public bool doLate = false;

	void Start() {
		if (doLocal) { initial = transform.localRotation; }
		else { initial = transform.rotation; }
	}
	
	void Update() {
		if (!doLate) { UpdateRotation(); }
	}
	
	void LateUpdate() {
		if (doLate) { UpdateRotation(); }
	}	
	
	void UpdateRotation() {
		if (doLocal) { transform.localRotation = initial; }
		else { transform.rotation = initial; }
		
		for (int i = 0; i < oscis.Length; i++) {
			transform.Rotate(rotations[i] * oscis[i].Update());
		}
	}
}
