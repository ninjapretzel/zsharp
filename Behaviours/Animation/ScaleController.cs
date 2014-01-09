using UnityEngine;
using System.Collections;

public class ScaleController : MonoBehaviour {
	public float targetSize = 1;
	public float speed = 5;
	
	Vector3 initialScale;
	float size = 1;
	
	void Awake() {
		initialScale = transform.localScale;
	}
	
	void Update() {
		size = size.TLerp(targetSize, speed);
		transform.localScale = initialScale * size;
	}
}
