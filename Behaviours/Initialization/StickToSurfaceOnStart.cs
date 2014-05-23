using UnityEngine;
using System.Collections;

public class StickToSurfaceOnStart : MonoBehaviour {
	public Vector3 direction = -Vector3.up;
	public Vector3 offset = Vector3.zero;
	public float maxDistance = 20;
	public bool changeRotation = false;
	public bool useZUp = false;
	public LayerMask layers = Physics.DefaultRaycastLayers;
	
	public bool waitForFixedUpdate = false;
	
	void Start() {
		if (!waitForFixedUpdate) {
			Stick();
		}
		
	}
	
	void FixedUpdate() {
		Stick();
	}
	
	void Stick() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, maxDistance, layers)) {
			transform.position = hit.point + offset;
			if (changeRotation) {
				if (useZUp) {
					transform.forward = hit.normal;
				
				} else {
					transform.up = hit.normal;
				}
			}
		}	
		Destroy(this);
	}
	
	
}
