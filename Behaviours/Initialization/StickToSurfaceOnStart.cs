using UnityEngine;
using System.Collections;

public class StickToSurfaceOnStart : MonoBehaviour {
	public Vector3 direction = -Vector3.up;
	public Vector3 offset = Vector3.zero;
	public float maxDistance = 20;
	public bool changeRotation = false;
	
	void Start() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, maxDistance)) {
			transform.position = hit.point + offset;
			if (changeRotation) { transform.up = hit.normal; }
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
