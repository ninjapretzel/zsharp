using UnityEngine;
using System.Collections;

public static class BoundsF {
	
	public static Vector3 RandomInside(this Bounds b) {
		Vector3 pos = b.center - b.extents;
		pos.x += b.size.x * Random.value;
		pos.y += b.size.y * Random.value;
		pos.z += b.size.z * Random.value;
		return pos;
	}
	
}
