using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class VectorF {
	public static Vector2 TLerp(this Vector2 v, float t) { return v.TLerp(Vector2.zero, t); }
	public static Vector2 TLerp(this Vector2 v, Vector2 target) { return v.TLerp(target, 1); }
	public static Vector2 TLerp(this Vector2 v, Vector2 target, float t) { return v.Lerp(target, Time.deltaTime * t); }
	public static Vector2 Lerp(this Vector2 v, Vector2 target, float t) { return Vector2.Lerp(v, target, t); }

	public static Vector3 TLerp(this Vector3 v, float t) { return v.TLerp(Vector3.zero, t); }
	public static Vector3 TLerp(this Vector3 v, Vector3 target) { return v.TLerp(target, 1); }
	public static Vector3 TLerp(this Vector3 v, Vector3 target, float t) { return v.Lerp(target, Time.deltaTime * t); }
	public static Vector3 Lerp(this Vector3 v, Vector3 target, float t) { return Vector3.Lerp(v, target, t); }

	public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max) {
		return v.ClampX(min.x, max.x).ClampY(min.y, max.y);
	}
	public static Vector2 ClampX(this Vector2 v, float min, float max) { return new Vector2(Mathf.Clamp(v.x, min, max), v.y); }
	public static Vector2 ClampY(this Vector2 v, float min, float max) { return new Vector2(v.x, Mathf.Clamp(v.y, min, max)); }
	
	public static Vector3 ClampX(this Vector3 v, float min, float max) { return new Vector3(Mathf.Clamp(v.x, min, max), v.y, v.z); }
	public static Vector3 ClampY(this Vector3 v, float min, float max) { return new Vector3(v.x, Mathf.Clamp(v.y, min, max), v.z); }
	public static Vector3 ClampZ(this Vector3 v, float min, float max) { return new Vector3(v.x, v.y, Mathf.Clamp(v.z, min, max)); }
	
	public static Vector3 Clamp(this Vector3 v, Bounds bounds) {
		Vector3 c = v;
		if (c.x < bounds.min.x) { c.x = bounds.min.x; }
		if (c.y < bounds.min.y) { c.y = bounds.min.y; }
		if (c.z < bounds.min.z) { c.z = bounds.min.z; }
		
		if (c.x > bounds.max.x) { c.x = bounds.max.x; }
		if (c.y > bounds.max.y) { c.y = bounds.max.y; }
		if (c.z > bounds.max.z) { c.z = bounds.max.z; }
		return c;
	}
	
}
