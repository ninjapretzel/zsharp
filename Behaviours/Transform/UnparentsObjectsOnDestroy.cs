using UnityEngine;
using System.Collections;

public class UnparentsObjectsOnDestroy : MonoBehaviour {
	public Transform[] objects;
	private static bool quitting = false;
	
	void OnApplicationQuit() {
		quitting = true;
	}
	
	void OnDestroy() {
		if (quitting) { return; }
		foreach (Transform t in objects) { 
			t.parent = null;
		}
	}
}
