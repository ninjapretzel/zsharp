using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SleepsOnNthCollision : MonoBehaviour {
	
	public int collisionNumber = 3;
	int collisions = 0;
	
	void Start() {
		
	}
	
	void Update() {
		
	}
	
	void OnCollisionEnter(Collision c) {
		if (collisions++ >= collisionNumber) {
			rigidbody.Sleep();
			collisions = 0;
		}
		
	}
	
}

public static class SleepsOnNthCollisionHelper {
	public static SleepsOnNthCollision AddComponent<T>(this GameObject g, int n) where T : SleepsOnNthCollision {
		SleepsOnNthCollision sleeper = g.AddComponent<SleepsOnNthCollision>();
		sleeper.collisionNumber = n;
		return sleeper;
	}
	
	public static SleepsOnNthCollision AddComponent<T>(this Component c, int n) where T : SleepsOnNthCollision {
		SleepsOnNthCollision sleeper = c.gameObject.AddComponent<SleepsOnNthCollision>();
		sleeper.collisionNumber = n;
		return sleeper;
	}
	
}