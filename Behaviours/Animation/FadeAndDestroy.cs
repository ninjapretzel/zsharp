using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour {
	
	public Timer timer = new Timer(3);
	public float fadeTime = .5f;
	
	void Update() {
		if (timer.Update()) {
			transform.FadeOut(fadeTime);
			gameObject.AddComponent<DestroyAfterDelay>().time = fadeTime;
		}
	}
	
}
