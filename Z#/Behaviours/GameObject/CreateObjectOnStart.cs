using UnityEngine;
using System.Collections;

public class CreateObjectOnStart : MonoBehaviour {
	public Transform target;
	public Vector3 offset;
	public bool parentIt = false;
	
	void Start() {
		Transform obj = Instantiate(target, transform.position + offset, transform.rotation) as Transform;
		if (parentIt) { obj.parent = transform; }
		Destroy(this);
	}
	
}
