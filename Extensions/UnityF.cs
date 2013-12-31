using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class UnityF {

	public static T AddComponent<T>(this Component c) where T : Component { return c.gameObject.AddComponent<T>(); }
	
	public static void RemoveFromWorld(this Component c) {
		GameObject.Destroy(c.gameObject);
	}
	
	public static T Require<T>(this Component c) where T : Component {
		Component check = c.GetComponent<T>();
		return (check != null ? check : c.gameObject.AddComponent<T>()) as T;
		
	}
	
	public static void FadeIn(this Component c) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) { r.Require<RendererFader>().FadeIn(); }
	}
	public static void FadeIn(this Component c, float time) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) { r.Require<RendererFader>().FadeIn(time); }
	}
	public static void FadeIn(this Component c, float time, float position) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) { r.Require<RendererFader>().FadeIn(time, position); }
	}
		
	public static void FadeOut(this Component c) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) { r.Require<RendererFader>().FadeOut(); }
	}
	public static void FadeOut(this Component c, float time) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) { r.Require<RendererFader>().FadeOut(time); }
	}
	public static void FadeOut(this Component c, float time, float position) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) { r.Require<RendererFader>().FadeOut(time, position); }
	}
	
	
	public static T GetComponentOnOrAbove<T>(this Component c) where T : Component {
		Transform test = c.transform;
		Component check;
		while (test != null) {
			check = test.GetComponent<T>();
			if (check) { return check as T; }
			test = test.parent;
		}
		return null;
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
