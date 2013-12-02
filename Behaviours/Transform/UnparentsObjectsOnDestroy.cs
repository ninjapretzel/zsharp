using UnityEngine;
using System.Collections;

public class UnparentsObjectsOnDestroy : MonoBehaviour {
	public Transform[] objects;
	public float killChildrenAfter = 0;
	private static bool quitting = false;
	
	void OnApplicationQuit() {
		quitting = true;
	}
	
	void OnDestroy() {
		if (quitting) { return; }
		if (!Application.isPlaying) { return; }

		foreach (Transform t in objects) { 
			t.parent = null;
			if (killChildrenAfter > 0) { 
				DestroyAfterDelay destructor = t.gameObject.AddComponent<DestroyAfterDelay>();
				destructor.time = killChildrenAfter;
			}
			
		}
	}
}
