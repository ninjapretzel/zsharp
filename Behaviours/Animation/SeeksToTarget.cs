using UnityEngine;
using System.Collections;

public class SeeksToTarget : MonoBehaviour {
	public Transform target;
	
	public Vector3 velocity;
	
	public float speed = 2;
	public float turning = 150;
	
	public float timeout = 0;
	public float maxTime = 5;
	
	public bool diesOnCollision = false;
	public bool lookInDirection = false;
	
	void Start() {
		
	}
	
	void Update() {
		timeout += Time.deltaTime;
		if (timeout > maxTime) { Destroy(gameObject); }
		
		if (target) {
			Vector3 direction = target.position - transform.position;
			float dturning = turning * Mathf.Deg2Rad * Time.deltaTime;
			velocity = Vector3.RotateTowards(velocity, direction, dturning, 0);
		}
		
		transform.position += velocity * Time.deltaTime;
		if (lookInDirection) { transform.forward = velocity.normalized; }
		
	}
	
}
















