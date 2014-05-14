using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class TransformF {
	
	public static void SnapParent(this Component c, Component other) { c.transform.SnapParent(other.transform); }
	
	public static void SnapParent(this Transform t, Transform o) {
		Transform p = t.parent;
		Quaternion q = t.rotation.To(o.rotation);
		
		p.rotation *= q;

		
		p.position = o.position;
		//Debug.Log(o.position + " - (" + t.position + " - " + p.position + ")");
		p.position -= (t.position - p.position);
		
		
	}
	
	public static void FlattenZ(this Transform t) {
		Quaternion q = t.rotation;
		Vector3 v = q.eulerAngles;
		v.z = 0;
		q.eulerAngles = v;
		t.rotation = q;
	}
	
	
}