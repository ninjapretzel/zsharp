using UnityEngine;
using System.Collections;

public class FadeAfter : MonoBehaviour {
	
	public Timer timer = new Timer(3);
	public float fadeTime = .5f;
	
	void Update() {
		if (timer.Update()) {
			transform.FadeOut(fadeTime);
		}
	}
	
}
