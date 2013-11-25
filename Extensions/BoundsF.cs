using UnityEngine;
using System.Collections;

public static class BoundsF {
	
	public static bool IsXout(this Bounds b, Vector3 v) { return v.x < b.min.x || v.x > b.max.x; }
	public static bool IsYout(this Bounds b, Vector3 v) { return v.y < b.min.y || v.y > b.max.y; }
	public static bool IsZout(this Bounds b, Vector3 v) { return v.z < b.min.z || v.z > b.max.z; }
	
	public static float	Xout(this Bounds b, Vector3 v) { return v.x.Outside(b.min.x, b.max.x); }
	public static float	Yout(this Bounds b, Vector3 v) { return v.y.Outside(b.min.y, b.max.y); }
	public static float	Zout(this Bounds b, Vector3 v) { return v.z.Outside(b.min.z, b.max.z); }
	
	
	public static Vector3 RandomInside(this Bounds b) {
		Vector3 pos = b.center - b.extents;
		pos.x += b.size.x * Random.value;
		pos.y += b.size.y * Random.value;
		pos.z += b.size.z * Random.value;
		return pos;
	}
	
}
