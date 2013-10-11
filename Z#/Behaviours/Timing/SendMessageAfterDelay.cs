using UnityEngine;
using System.Collections;

public class SendMessageAfterDelay : MonoBehaviour {
	public float time = 3;
	private float timeout = 0;
	public string message = "";
	public bool repeat = false;
	
	void Start() {
		
	}
	
	void Update() {
		timeout += Time.deltaTime;
		if (timeout >= time) {
			timeout -= time;
			SendMessage(message, SendMessageOptions.DontRequireReceiver);
			if (!repeat) { Destroy(this); }
		}
	}
}
