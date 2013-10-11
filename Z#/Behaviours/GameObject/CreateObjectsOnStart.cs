using UnityEngine;
using System.Collections;

public class CreateObjectsOnStart : MonoBehaviour {
	public Transform[] targets;
	public Vector3 offset;
	
	void Awake() {
		foreach (Transform target in targets) {
			Instantiate(target, transform.position + offset, transform.rotation);
		}
		Destroy(this);
	}
	
}
