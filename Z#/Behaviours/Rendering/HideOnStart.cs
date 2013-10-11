using UnityEngine;
using System.Collections;

public class HideOnStart : MonoBehaviour {

	void Start() {
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = false;
		}
	}
	
}
