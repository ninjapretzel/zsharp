using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class GLF {
	public static Material material;
	
	public static void DrawShit() {
		material.SetPass(0);
		GL.Color(new Color(1, 1, 1, .5f));
		Vector3[] blah = new Vector3[6];
		blah[0] = new Vector3(0, 0, 0);
		blah[1] = new Vector3(0, 0, 1);
		blah[2] = new Vector3(1, 0, 1);
		blah[3] = new Vector3(1, 0, 1);
		blah[4] = new Vector3(1, 0, 0);
		blah[5] = new Vector3(0, 0, 0);
		DrawTriangles(blah);
		Finish();
	}
	
	public static void VertexColor(Vector3 v, Color c) { GL.Color(c); Vertex(v); }
	public static void Vertex(Vector3 v) { GL.Vertex3(v.x, v.y, v.z); }
	
	public static void DrawTriangles(Vector3[] vs) {
		GL.Begin(GL.TRIANGLES);
		DrawPoints(vs);
		GL.End();
	}
	
	public static void DrawLines(Vector3[] vs) {
		GL.Begin(GL.LINES);
		DrawPoints(vs);
		GL.End();
	}
	
	public static void DrawPoints(Vector3[] vs) { for (int i = 0; i < vs.Length; i++) { Vertex(vs[i]); } }
	
	public static void Ortho() {
		GL.PushMatrix();
		GL.LoadOrtho();
	}
	
	public static void DrawArrow(Vector3 point, Vector3 direction, float size) {
		Vector3[] points = new Vector3[6];
		points[0] = point + direction.normalized;
		points[1] = point + Vector3.Cross(direction, point);
		
		DrawLines(points);
		
	}
	
	public static void Finish() { GL.PopMatrix(); }
	
}
