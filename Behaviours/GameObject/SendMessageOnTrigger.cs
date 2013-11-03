using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SendMessageOnTrigger : MonoBehaviour {
	public string message = "Function";
	public CollisionAction action;
	
	void OnTriggerEnter(Collider c) {
		if (action == CollisionAction.Enter) { c.SendMessage(message, SendMessageOptions.DontRequireReceiver); }
	}
	
	void OnTriggerExit(Collider c) {
		if (action == CollisionAction.Exit) { c.SendMessage(message, SendMessageOptions.DontRequireReceiver); }
	}
	
	void OnTriggerStay(Collider c) {
		if (action == CollisionAction.Stay) { c.SendMessage(message, SendMessageOptions.DontRequireReceiver); }
	}
	
}
