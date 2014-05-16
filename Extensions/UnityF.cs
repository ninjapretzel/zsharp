using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class UnityF {
	
	public static void TryDeactivate(this Component c) {
		if (c != null) {
			if (c.GetType().IsSubclassOf(typeof(Behaviour))) {
				Behaviour b = c as Behaviour;
				//Debug.Log("Target: " + target + " found.");
				b.enabled = false;
			}
			
			if (c.GetType().IsSubclassOf(typeof(Renderer))) {
				Renderer r = c as Renderer;
				//Debug.Log("Target: " + target + " found.");
				r.enabled = false;
			}
		}
	}
	
	public static void TryActivate(this Component c) {
		if (c != null) {
			if (c.GetType().IsSubclassOf(typeof(Behaviour))) {
				Behaviour b = c as Behaviour;
				//Debug.Log("Target: " + target + " found.");
				b.enabled = true;
			}
			
			if (c.GetType().IsSubclassOf(typeof(Renderer))) {
				Renderer r = c as Renderer;
				//Debug.Log("Target: " + target + " found.");
				r.enabled = true;
			}
		}
	}
	
	public static GameObject Duplicate(this Component c) { 
		GameObject g = (GameObject)GameObject.Instantiate(c.gameObject, c.transform.position, c.transform.rotation);
		g.transform.parent = c.transform.parent;
		return g;
	}
	public static T DuplicateAs<T>(this Component c) where T : Component { 
		return c.Duplicate().GetComponent<T>() as T;
	}
	
	public static void RemoveFromWorld(this Component c) {
		GameObject.Destroy(c.gameObject);
	}
	
	public static T GrabFromChild<T>(this Component c, string childName) where T : Component {
		Transform t = c.transform.Find(childName);
		if (t != null) { return t.GetComponent<T>(); }
		return null;
	}
	
	public static void Broadcast(this Component c, string message) {
		c.SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
	
	public static void BroadcastAll(this Component c, string message) {
		c.BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
	}
	
	public static T AddComponent<T>(this Component c) where T : Component { return c.gameObject.AddComponent<T>(); }
	
	public static T Require<T>(this GameObject o) where T : Component { return o.transform.Require<T>(); }
	public static T Require<T>(this Component c) where T : Component {
		Component check = c.GetComponent<T>();
		return (check != null ? check : c.gameObject.AddComponent<T>()) as T;
		
	}
	
	public static void SetColor(this Component c, string property, Color color) {
		foreach (Renderer r in c.GetComponentsInChildren<Renderer>() as Renderer[]) {
			if (r.material.HasProperty(property)) {
				r.material.SetColor(property, color);
			}
		}
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
