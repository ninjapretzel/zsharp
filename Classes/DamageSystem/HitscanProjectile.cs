
using UnityEngine;
using System.Collections;

public class HitscanProjectile : DealsDamage {
	public bool displayLine = false;
	public Transform decal;
	LineRenderer lineRenderer;
	float time = 0;
	public bool done { get { return time > .02; } }
	
	public static HitscanProjectile Factory() {
		GameObject gob = new GameObject("HitscanProjectile");
		return gob.AddComponent<HitscanProjectile>();
	}
	
	void Start() {
		if (displayLine) {
			lineRenderer = transform.Require<LineRenderer>();
			lineRenderer.useWorldSpace = false;
		}
		
		
		RaycastHit rayhit;
		float dist = 100;
		if (Physics.Raycast(transform.position, transform.forward, out rayhit)) {
			dist = (transform.position - rayhit.point).magnitude;
			if (decal != null) {
				Transform t = Instantiate(decal, rayhit.point, Quaternion.identity) as Transform;
				t.forward = rayhit.normal;
				t.parent = rayhit.collider.transform;
				t.Rotate(0, 0, RandomF.Range(0, 360));
				
			}
			
			Unit unit = rayhit.collider.GetComponentOnOrAbove<Unit>();
			if (unit != null) {
				unit.HitEffect(rayhit.point);
			}
			
		}
		if (lineRenderer != null) { lineRenderer.SetPosition(1, new Vector3(0, 0, dist)); }
		//Move to IgnoreRaycast layer
		gameObject.layer = 2;
		
		BoxCollider b = gameObject.AddComponent<BoxCollider>();
		b.center = new Vector3(0, 0, dist/2f);
		b.size = new Vector3(.0001f, .0001f, dist * 1.01f);
		b.isTrigger = true;
		
		gameObject.AddComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		
		
	}
	
	void Update() {
		time += Time.deltaTime;
		if (done) { Destroy(gameObject); } 
		
	}
	
	void HitATarget() {
		Destroy(gameObject);
	}
	
}
