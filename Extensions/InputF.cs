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
	
	public static Touch firstTouch {
		get { return Input.touches[0]; }
	}
	
	public static Touch secondTouch {
		get { return Input.touches[1]; }
	}
	
	public static Vector2 averageTouch {
		get { 
			if (Input.touches.Length == 0) { return -Vector2.one; }
			Vector2 avg = Vector2.zero;
			foreach (Touch t in Input.touches) {
				avg += t.position;
			}
			
			return avg / Input.touches.Length;
		}
		
	}
	
	public static Vector3 mouseDirection { get { return (mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0)).normalized; } }
	
	public static Vector2 TouchScroll(this Vector2 v, Rect area, float sensitivity = 1f) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Moved) {
				Vector2 pos = t.ScreenPosition();
				if (area.Contains(pos)) {
					Vector2 dpos = t.deltaPosition * sensitivity;
					dpos.y *= -1;
					return v - dpos;
				}
			}
		}
		return v;
	}
	
	
	public static Vector2 TouchVelocity(Rect area, float sensitivity = 1f) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Moved) {
				if (area.Contains(t.ScreenPosition())) {
					return t.deltaPosition * sensitivity;
				}
			}
		}
		return Vector2.zero;
	}
	
}
