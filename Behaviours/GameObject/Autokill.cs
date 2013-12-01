using UnityEngine;
using System.Collections;

public class Autokill : MonoBehaviour {
	public Bounds bounds;
	
	void Update() {
		if (!bounds.Contains(transform.position)) { Destroy(gameObject); }
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 0, 0, .2f);
		Gizmos.DrawCube(bounds.center, bounds.size);
	}
	
}
