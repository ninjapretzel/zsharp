using UnityEngine;
using System.Collections;

public static class CameraF {
	
	
	public static Vector3 mouseWorldPosition {
		get {
			RaycastHit rayhit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit)) {
				return rayhit.point;
			}
			
			return Vector3.zero;
		}
	}
	
	public static Vector2 GetScreenPosition(this Transform t) { return GetScreenPosition(Camera.main, t); }
	public static Vector2 GetScreenPosition(this Transform t, Camera cam) { return GetScreenPosition(cam, t); }
	public static Vector2 GetScreenPosition(this Camera cam, Transform t) {
		Vector2 pos = cam.WorldToScreenPoint(t.position);
		pos.y = Screen.height - pos.y;
		return pos;
	}
	
	public static Vector2 GetViewPosition(this Transform t) { return GetViewPosition(Camera.main, t); }
	public static Vector2 GetViewPosition(this Transform t, Camera cam) { return GetViewPosition(cam, t); }
	public static Vector2 GetViewPosition(this Camera cam, Transform t) {
		Vector2 pos = cam.WorldToViewportPoint(t.position);
		pos.y = 1.0f - pos.y;
		return pos;
	}
	
}
