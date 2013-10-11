using UnityEngine;
using System.Collections;

public class ScaleParticles : MonoBehaviour {
	
	float initialMaxEmission;
	float initialMinEmission;
	
	float initialMaxSize;
	float initialMinSize;
	
	Vector3 initialArea;
	
	Vector3 initialRndVelocity;
	
	public bool scaleEmission = true;
	public bool scaleSize = false;
	public bool scaleArea = false;
	public bool scaleRndVelocity = false;
	
	ParticleEmitter emitter;
	FakeParticleEmitter fakeEmitter;
	
	public bool startOnly = false;
	public float scale = 1.0f;
	
	public bool randomize = false;
	public float minScale = 1.0f;
	public float maxScale = 1.0f;
	
	public bool burstOnStart = false;
	
	void Awake() {
		emitter = GetComponent<ParticleEmitter>();
		fakeEmitter = GetComponent<FakeParticleEmitter>();
		
		if (emitter) { PullFromLegacyEmitter(emitter); }
		if (fakeEmitter) { PullFromFakeEmitter(fakeEmitter); }
		
	}
	
	void Update() {
		Scale();
	}
	
	void PullFromFakeEmitter(FakeParticleEmitter emitter) {
		initialMaxEmission = emitter.maxEmission;
		initialMinEmission = emitter.minEmission;
		
		initialMaxSize = emitter.maxSize;
		initialMinSize = emitter.minSize;
		
		initialRndVelocity = emitter.randomVelocity;
		
		if (randomize) { scale = Random.Range(minScale, maxScale); }
		if (burstOnStart) { emitter.Emit(1); }
		if (startOnly) { 
			Scale(); 
			Destroy(this); 
		}
		
	}
	
	void PullFromLegacyEmitter(ParticleEmitter emitter) {
		initialMaxEmission = emitter.maxEmission;
		initialMinEmission = emitter.minEmission;
		
		initialMaxSize = emitter.maxSize;
		initialMinSize = emitter.minSize;
		
		initialRndVelocity = emitter.rndVelocity;
		
		if (randomize) { scale = Random.Range(minScale, maxScale); }
		if (burstOnStart) { emitter.Emit(1); }
		if (startOnly) { 
			Scale(); 
			Destroy(this); 
		}
		
	}
	
	void Scale() {
		if (emitter) { Scale(emitter); }
		else if (fakeEmitter) { Scale(fakeEmitter); }
	}
	
	void Scale(ParticleEmitter emitter) {
		if (scaleEmission) {
			emitter.maxEmission = initialMaxEmission * scale;
			emitter.minEmission = initialMinEmission * scale;
		}
		
		if (scaleSize) {
			emitter.maxSize = initialMaxSize * scale;
			emitter.minSize = initialMinSize * scale;
		}
		
		if (scaleRndVelocity) {
			emitter.rndVelocity = initialRndVelocity * scale;
		}

	}
	
	
	void Scale(FakeParticleEmitter emitter) {
		if (scaleEmission) {
			emitter.maxEmission = initialMaxEmission * scale;
			emitter.minEmission = initialMinEmission * scale;
		}
		
		if (scaleSize) {
			emitter.maxSize = initialMaxSize * scale;
			emitter.minSize = initialMinSize * scale;
		}
		
		if (scaleRndVelocity) {
			emitter.randomVelocity = initialRndVelocity * scale;
		}

	}
	
}




















