using UnityEngine;
using System.Collections;

public class ReplaceWithPrefab : MonoBehaviour {
	public Transform target;
	
	void Awake () {
		if (target) { 
			Transform obj = Instantiate(target, transform.position, transform.rotation) as Transform;
			obj.parent = transform.parent;
			Destroy(gameObject);
		}
	}
	
}
