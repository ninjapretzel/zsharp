using UnityEngine;
using System.Collections;

public class HideOnStart : MonoBehaviour {
	
	public bool onAwake = false;
	
	void Awake() {
		if (onAwake) { 
			Disable();
		}
	}	
	
	void Start() {
		Disable();
	}
	
	void Disable() {
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = false;
		}
		Destroy(this);
	}
	
}
