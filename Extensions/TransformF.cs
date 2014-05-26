using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class TransformF {
	
	public static Vector3 DirectionTo(this Component c, Component other) { return other.transform.position - c.transform.position; }
	public static float DistanceTo(this Component c, Component other) { return c.DirectionTo(other).magnitude; }
	public static float FlatDistanceTo(this Component c, Component other) { 
		Vector3 dir = c.DirectionTo(other);
		dir.y = 0;
		return dir.magnitude;
	}
	
	public static void SnapParent(this Component c, Component other) { c.transform.SnapParent(other.transform); }
	public static void SnapParent(this Transform t, Transform o) {
		Transform p = t.parent;
		Quaternion q = t.rotation.To(o.rotation);
		
		p.rotation *= q;

		
		p.position = o.position;
		//Debug.Log(o.position + " - (" + t.position + " - " + p.position + ")");
		p.position -= (t.position - p.position);
		
		
	}
	
	public static void FlattenRotationZ(this Component c) { c.transform.FlattenZ(); }
	public static void FlattenZ(this Transform t) {
		Quaternion q = t.rotation;
		Vector3 v = q.eulerAngles;
		v.z = 0;
		q.eulerAngles = v;
		t.rotation = q;
	}
	
	
}