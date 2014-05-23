using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class MeshF {

	public static Mesh Make(Vector3[] vertices, Vector2[] uv, int[] triangles) {
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		return mesh;
	}
	
	public static Mesh Make(Vector3[] vertices, Vector2[] uv, int[] triangles, Vector3[] normals) {
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		return mesh;
	}
	
	public static void RecalculateTangents(this Mesh mesh) {
		Vector4[] tans = new Vector4[mesh.normals.Length];
		
		Vector4 t1;
		Vector4 t2;
		
		for (int i = 0; i < tans.Length; i++) {
			t1 = Vector3.Cross(mesh.normals[i], Vector3.forward);
			t2 = Vector3.Cross(mesh.normals[i], Vector3.up);
			
			if (t1.magnitude > t2.magnitude) { tans[i] = t1; }
			else { tans[i] = t2; }
			
			tans[i].w = 0;
			tans[i].Normalize();
			tans[i].w = 1;
		}
		
		mesh.tangents = tans;
	}
	
}