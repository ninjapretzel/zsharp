using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {
	private Vector3 center;
	public Oscillator[] oscis;
	public Vector3[] offsets;
	
	public bool	doLocal = false;
	public bool doLate = false;
	void Start () {
		if (doLocal) { center = transform.localPosition; }
		else { center = transform.position; }
	}
	
	void Update() {
		if (!doLate) { UpdateHover(); }
	}
	
	void LateUpdate() {
		if (doLate) { UpdateHover(); }
	}	
	
	void UpdateHover() {
		if (doLocal) { transform.localPosition = center; }
		else { transform.position = center; }
		
		for (int i = 0; i < oscis.Length; i++) {
			if (doLocal) { transform.localPosition += offsets[i] * oscis[i].Update(); }
			else { transform.position += offsets[i] * oscis[i].Update(); }
		}	
	}	
}
