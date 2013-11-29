using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class UnityF {
	
	public static void RemoveFromWorld(this Component c) {
		GameObject.Destroy(c.gameObject);
	}
	
	public static T Require<T>(this Component c) where T : Component {
		Component check = c.GetComponent<T>();
		return (check != null ? check : c.gameObject.AddComponent<T>()) as T;
		
	}
	
	public static T GetComponentAbove<T>(this Component c) where T : Component {
		Transform test = c.transform;
		Component check;
		while (test.parent != null) {
			test = test.parent;
			check = test.GetComponent<T>();
			if (check) { return check as T; }
		}
		return null;
	}
	
	

}
