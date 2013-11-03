using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class Vector3F {
	
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
