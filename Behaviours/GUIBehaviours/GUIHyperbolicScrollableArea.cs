using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIHyperbolicScrollableArea : MonoBehaviour {
	Dictionary<Transform, float> angles;
	public float initialAngleScale = 1;
	
	public float selectionGrow = 3;
	
	public float min = 0;
	public float max = 90;
	public float offset = 0;
	public float target = 0;
	public float dampening = 5;
	public Vector3 scales = Vector3.one;
	public Transform selected;
	
	public float scrollSpeed = 5;
	
	public bool enableTouchScrolling;
	public float touchScrollSpeed = 5;
	public float scrollVelocity;
	public float velocityDampening = 8;
	
	
	void Awake() {
		angles = new Dictionary<Transform, float>();
		foreach (Component c in GetComponentsInChildren<FindMe>()) {
			angles.Add(c.transform, c.transform.localPosition.y * initialAngleScale);
			Destroy(c);
		}
		selected = null;
		
		#if UNITY_ANDROID || UNITY_IOS
		if (Application.platform != RuntimePlatform.WindowsEditor) {
			scrollSpeed = touchScrollSpeed;
		}
		#endif
		
	}
	
	void Update() {
		target = target.Clamp(min, max);
		offset = offset.TLerp(target, dampening);
		
		#if UNITY_ANDROID || UNITY_IOS
		if (enableTouchScrolling) {
			scrollVelocity += InputF.TouchVelocity(ScreenF.all).y * scrollSpeed;
		}
		#endif
		
		scrollVelocity += InputWrapper.GetAxis("Mouse ScrollWheel") * -scrollSpeed;
		
		
		
		target += scrollVelocity * Time.deltaTime;
		scrollVelocity = scrollVelocity.TLerp(0, velocityDampening);
		
		foreach (Transform t in angles.Keys) {
			if (t != null) {
				
				Vector3 pos = Vector3.zero;
				float angle = (offset + angles[t]).Clamp(-355, 355);
				
				float absAngle = angle.Abs();
				t.localScale = Vector3.one * (1 + selectionGrow * (1-absAngle/355));
				
				/*
				if (absAngle < 5) {
					if (selected != t) {
						t.SetColor("_Color", Color.red);
						if (selected != null) {
							selected.SetColor("_Color", Color.white);
						}
						selected = t;
					}
					
				}
				//*/
				
				pos.y = scales.y * (float)System.Math.Sinh(angle * Mathf.Deg2Rad);
				pos.z = scales.z * (float)System.Math.Cosh(angle * Mathf.Deg2Rad);
				
				t.localPosition = pos;
			}
			
		}
		
	}
	
	public void Add(Transform t) {
		t.parent = transform;
		t.localPosition = new Vector3(0, t.localPosition.y, 0);
		
		angles.Add(t, t.localPosition.y * initialAngleScale);
		
		
		
		
	}
	
	public void Remove(Transform t) {
		if (angles.ContainsKey(t)) {
			angles.Remove(t);
		}
	}
	
}
