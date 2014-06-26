using UnityEngine;
using System.Collections;

public class FadeAndDestroyListener : MonoBehaviour {
	
	
	void FadeAndDestroy() {
		FakeParticleEmitter fakeEmitter = GetComponent<FakeParticleEmitter>();
		if (fakeEmitter) {
			foreach (FakeParticleEmitter p in GetComponentsInChildren<FakeParticleEmitter>()) {
				FadeDestroyFake(p);
			}
		} else {
			foreach (ParticleEmitter p in GetComponentsInChildren<ParticleEmitter>()) {
				FadeDestroyLegacy(p.transform);
			}
		}
	}
	
	private void FadeDestroyFake(FakeParticleEmitter emitter) {
		//Debug.Log("FadeAndDestroy called on " + emitter.transform.name);
		
		emitter.emit = false;
		emitter.autodestruct = true;
		emitter.transform.parent = null;
	}
	
	private void FadeDestroyLegacy(Transform t) {
		//Debug.Log("FadeAndDestroy called on " + t.name);
		ParticleEmitter emitter = t.GetComponent<ParticleEmitter>();
		ParticleAnimator animator = t.GetComponent<ParticleAnimator>();
		
		
		if (emitter) { emitter.emit = false; }
		if (animator) { animator.autodestruct = true; }
		t.parent = null;
		
		
	}

}