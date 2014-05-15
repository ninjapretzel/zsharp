using UnityEngine;
using System.Collections;

public static class CameraF {
	
	
	public static Vector3 mouseWorldPosition {
		get {
			RaycastHit rayhit;
			if (Physics.Raycast(mouseRay, out rayhit)) {
				return rayhit.point;
			}
			
			return Vector3.zero;
		}
	}
	
	public static Ray mouseRay {
		get { return Camera.main.ScreenPointToRay(Input.mousePosition); }
	}
	
	public static Collider mouseCollider {
		get {
			RaycastHit rayhit;
			if (Physics.Raycast(mouseRay, out rayhit)) {
				return rayhit.collider;
			}
			return null;
		}
	}
	
	public static bool IsOffscreen(this Component c) { return !c.IsOnscreen(); }
	public static bool IsOnscreen(this Component c) {
		Vector3 pos = c.GetViewPosition();
		if (pos.z < 0) { return false; }
		return pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1;
	}
	
	public static Vector3 GetScreenPosition(this Component c) { return GetScreenPosition(Camera.main, c.transform); }
	public static Vector3 GetScreenPosition(this Transform t) { return GetScreenPosition(Camera.main, t); }
	public static Vector3 GetScreenPosition(this Transform t, Camera cam) { return GetScreenPosition(cam, t); }
	public static Vector3 GetScreenPosition(this Camera cam, Transform t) {
		Vector3 pos = cam.WorldToScreenPoint(t.position);
		pos.y = Screen.height - pos.y;
		return pos;
	}
	
	public static Vector3 GetViewPosition(this Component c) { return GetViewPosition(Camera.main, c.transform); }
	public static Vector3 GetViewPosition(this Transform t) { return GetViewPosition(Camera.main, t); }
	public static Vector3 GetViewPosition(this Transform t, Camera cam) { return GetViewPosition(cam, t); }
	public static Vector3 GetViewPosition(this Camera cam, Transform t) {
		Vector3 pos = cam.WorldToViewportPoint(t.position);
		pos.y = 1.0f - pos.y;
		return pos;
	}
	
}
