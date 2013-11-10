using UnityEngine;
using System.Collections;

public static class InputF {
	
	public static Vector2 TouchScroll(this Vector2 v, Rect area) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Moved) {
				if (area.Contains(t.position)) {
					return v - t.deltaPosition;
				}
			}
		}
		return v;
	}
	
	public static Vector2 TouchVelocity(Rect area) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Moved) {
				if (area.Contains(t.position)) {
					return t.deltaPosition;
				}
			}
		}
		return Vector2.zero;
	}
	
}
