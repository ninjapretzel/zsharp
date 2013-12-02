using UnityEngine;
using System.Collections;

public class SetDelay : MonoBehaviour {
	public float time;
	
	void Start() {
		transform.SendMessage("SetDelay", time);
		Destroy(this);
	}
}
