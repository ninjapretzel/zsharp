using UnityEngine;
using System.Collections;

public class PushesObjectsAway : MonoBehaviour {
	public float maxRadius = 5;
	public float time;
	public float expandTime = 0.02f;
	public float pushRate = 1.05f;
	public float maxTime = 2.0f;
	public bool destructObject = true;
	
	SphereCollider sphere;
	
	void Start() {
		sphere = GetComponent<SphereCollider>();
		if (sphere != null) { 
			maxRadius = sphere.radius;
			
		} else {
			sphere = gameObject.AddComponent<SphereCollider>();
		}
		
		sphere.radius = 0;
	}
	

	void Update() {
		time += Time.deltaTime;
		sphere.radius = Mathf.Min(maxRadius, time / expandTime * maxRadius);
		if (time >= maxTime) { 
			if (destructObject) {
				Destroy(gameObject); 
			} else {
				Destroy(this);
			}
		}
		
	}
	
	void OnTriggerStay(Collider c) {
		Pushable check = c.GetComponent<Pushable>();
		if (check) {
			//Debug.Log("pushing");
			Vector3 diff = c.transform.position - transform.position;
			diff.y = 0;
			c.transform.position = transform.position + diff * pushRate;
		}
	}
	
	
}
