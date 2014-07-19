using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
	public bool flip = false;
	public float zrotation = 0;
	public float dzrotation = 0;
	
	void LateUpdate() {
		if (renderer != null && !renderer.enabled) { return; }
		transform.LookAt(Camera.main.transform);
		if (flip) { transform.Rotate(0, 180, 0); }
		zrotation += dzrotation * Time.deltaTime;
		if (zrotation > 0) { transform.Rotate(0, 0, zrotation); }
	}
}
