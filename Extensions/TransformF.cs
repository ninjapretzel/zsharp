using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class TransformF {
	
	public static Vector3 DirectionTo(this Component c, Vector3 position) { return position - c.transform.position; }
	public static Vector3 DirectionTo(this Component c, Component other) { return other.transform.position - c.transform.position; }
	
	public static float DistanceTo(this Component c, Vector3 position) { return c.DirectionTo(position).magnitude; }
	public static float DistanceTo(this Component c, Component other) { return c.DirectionTo(other).magnitude; }
	public static float FlatDistanceTo(this Component c, Component other) { 
		Vector3 dir = c.DirectionTo(other);
		dir.y = 0;
		return dir.magnitude;
	}
	
	public static void SortChildrenByName(this Transform root) {
		Dictionary<string, List<Transform>> children = new Dictionary<string, List<Transform>>();
		List<string> nameList = new List<string>();
		
		foreach (Transform t in root.GetChildren()) { 
			string name = t.gameObject.name;
			if (!nameList.Contains(name)) { nameList.Add(name); }
			if (children.ContainsKey(name)) {
				children[name].Add(t);
			} else {
				List<Transform> newList = new List<Transform>();
				newList.Add(t);
				children.Add(name, newList);
			}
		}
		
		nameList.Sort();
		
		int i = 0;
		foreach (string name in nameList) {
			List<Transform> transforms = children[name];
			foreach (Transform t in transforms) {
				t.SetSiblingIndex(i++);
			}
			
		}
		
		
	}
	
	public static void StretchFrom(this Transform t, Vector3 from, Vector3 to) { t.StretchFrom(from, to, 1); }
	public static void StretchFrom(this Transform t, Vector3 from, Vector3 to, float scale) {
		Vector3 center = (from + to) / 2f;
		Vector3 dir = from - to;
		float dist = dir.magnitude;
		//angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
		
		t.position = center;
		t.forward = dir;
		
		Vector3 localScale = t.localScale;
		localScale.z = dist * scale;
		t.localScale = localScale;
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
	
	public static Transform[] GetChildren(this Transform t) {
		int num = t.childCount;
		Transform[] list = new Transform[num];
		for (int i = 0; i < num; i++) {
			list[i] = t.GetChild(i);
		}
		return list;
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