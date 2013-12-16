using UnityEngine;
using System.Collections;

public static class InputF {
	
	public static Vector3 mousePosition { 
		get {
			Vector3 pos = Input.mousePosition;
			pos.y = Screen.height - pos.y;
			return pos;
		}
	}
	public static Vector3 mouseDirection { get { return (mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0)).normalized; } }
	
	public static Vector2 TouchScroll(this Vector2 v, Rect area) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Moved) {
				Vector2 pos = t.position;
				pos.y = Screen.height - pos.y;
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
