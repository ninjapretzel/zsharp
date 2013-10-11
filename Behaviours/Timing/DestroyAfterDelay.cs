using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : MonoBehaviour {
	public float time = 3;
	private float timeout = 0;
	
	void Update() {
		timeout += Time.deltaTime;
		if (timeout > time) { Destroy(gameObject); }
	}
}
