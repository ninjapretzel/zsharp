using UnityEngine;
using System.Collections;

public class BoundPosition : MonoBehaviour {
	public Bounds bounds;
	public bool doLocal = false;
	public bool doLate = true;
	public bool boundx = false;
	public bool boundy = false;
	public bool boundz = false;
	
	void Update() {
		if (!doLate) { Bound(); }
	}
	
	void LateUpdate() {
		if (doLate) { Bound(); }
	}
	
	void Bound() {
		Vector3 min = bounds.center - bounds.extents;
		Vector3 max = bounds.center + bounds.extents;
		Vector3 newPos = transform.position;
		if (doLocal) { newPos = transform.localPosition; }
		
		if (boundx) { newPos.x = Mathf.Clamp(newPos.x, min.x, max.x); }
		if (boundy) { newPos.y = Mathf.Clamp(newPos.y, min.y, max.y); }
		if (boundz) { newPos.z = Mathf.Clamp(newPos.z, min.z, max.z); }
		
		if (doLocal) { transform.localPosition = newPos; }
		else { transform.position = newPos; }
	}
}
