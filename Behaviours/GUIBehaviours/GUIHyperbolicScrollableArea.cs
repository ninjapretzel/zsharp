using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIHyperbolicScrollableArea : MonoBehaviour {
	Dictionary<Transform, float> angles;
	public float initialAngleScale = 1;
	
	public float min = 0;
	public float max = 90;
	public float offset = 0;
	public float target = 0;
	public float dampening = 5;
	public Vector3 scales = Vector3.one;
	
	void Start() {
		angles = new Dictionary<Transform, float>();
		foreach (Component c in GetComponentsInChildren<FindMe>()) {
			angles.Add(c.transform, c.transform.localPosition.y * initialAngleScale);
			Destroy(c);
		}
		
	}
	
	void Update() {
		target = target.Clamp(min, max);
		offset = offset.TLerp(target, dampening);
	
		foreach (Transform t in angles.Keys) {
			if (t != null) {
				
				Vector3 pos = Vector3.zero;
				float angle = (offset + angles[t]).Clamp(-355, 355);
				
				pos.y = scales.y * (float)System.Math.Sinh(angle * Mathf.Deg2Rad);
				pos.z = scales.z * (float)System.Math.Cosh(angle * Mathf.Deg2Rad);
				
				t.localPosition = pos;
			}
		}
	}
}
