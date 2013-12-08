using UnityEngine;
using System.Collections;

public class RepeatedAction : MonoBehaviour {
	public float time;
	private float timeout;
	
	void Update() {
		timeout += Time.deltaTime;
		if (timeout > time) {
			Action();
			timeout -= time;
		}
	}
	
	
	public virtual void Action() { }
	
}
