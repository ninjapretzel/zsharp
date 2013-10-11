using UnityEngine;
using System.Collections;

public class DrawsGizmo : MonoBehaviour {
	public Vector3 size = Vector3.one;
	public bool sphere = false;
	public bool wireframe = false;
	public Color color = Color.red;
	
	void OnDrawGizmos() {
		Color c = color;
		c.a *= .5f;
		Gizmos.color = c;
		Draw();
	}
	
	void OnDrawGizmosSelected() {
		Gizmos.color = color;
		Draw();
	}
	
	public void Draw() {
		if (sphere) {
			float radius = Mathf.Max(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
			if (wireframe) { Gizmos.DrawWireSphere(transform.position, radius); }
			else { Gizmos.DrawSphere(transform.position, radius); }
		} else {
			if (wireframe) { Gizmos.DrawWireCube(transform.position, size); }
			else { Gizmos.DrawCube(transform.position, size);  }
		}
	}
	
}
