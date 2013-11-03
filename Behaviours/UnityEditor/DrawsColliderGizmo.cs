using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DrawsColliderGizmo : MonoBehaviour {
	public Color color = Color.white;
	
	void OnDrawGizmosSelected() {
		Gizmos.color = color;
		DrawGizmo();
		
	}
	
	void OnDrawGizmos() {
		Color c = color;
		c.a *= .2f;
		Gizmos.color = c;
		DrawGizmo();
		
	}
	
	
	void DrawGizmo() {
		Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
	}
	
}
