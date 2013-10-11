using UnityEngine;
using System.Collections;

public class GrowCollider : MonoBehaviour {
	public float ratio = 1.5f;
	public float time = 3.0f;
	
	float timeout = 0;
	
	SphereCollider sphere;
	BoxCollider box;
	
	float radius;
	
	Vector3 size;
	
	void Awake() {
		sphere = GetComponent<SphereCollider>();
		box = GetComponent<BoxCollider>();
		if (sphere) { radius = sphere.radius; }
		if (box) { size = box.size; }
	}
	
	void Update () {
		timeout += Time.deltaTime;
		float mult = Mathf.Min(ratio, (timeout / time) * ratio);
		
		if (sphere) { sphere.radius = radius *  mult; }
		else if (box) { box.size = size * mult; }
		
		if (timeout > time) { Destroy(this); }
	}
}
