using UnityEngine;
using System.Collections;

public class DelayedAction : MonoBehaviour {
	public float time = 1;
	float timeout = 0;
	
	
	void Update() { 
		timeout += Time.deltaTime;
		Frame();
		if (timeout > time) {
			Action();
			Destroy(this);
		}
		
		
	}
	
	public virtual void Frame() {
		
	}
	
	public virtual void Action() {
		Destroy(this);
		
	}
	
	void SetDelay(float f) {
		time = f;
	}
	
}
