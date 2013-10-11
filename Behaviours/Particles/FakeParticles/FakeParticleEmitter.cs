using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FakeParticleEmitter : MonoBehaviour {
	public bool emit = true;
	public bool autodestruct = false;
	public int maxParticles = 100;
	public int liveParticles = 0;
	public int deadParticles = 0;
	
	public float minEmission = 10;
	public float maxEmission = 10;
	
	public float minSize = .1f;
	public float maxSize = 1;
	//public int scaleSteps = 5;
	
	public float minEnergy = 1;
	public float maxEnergy = 1;
	
	public Vector3 worldVelocity = Vector3.zero;
	public Vector3 randomVelocity = Vector3.zero;
	
	public Vector3 localVelocity = Vector3.zero;
	public Vector3 randomLocalVelocity = Vector3.zero;
	
	public Vector3 force = Vector3.zero;
	public Vector3 randomForce = Vector3.zero;
	
	//public float emitterVelocityScale = 0;
	
	public float angularVelocity = 0;
	public float randomAngularVelocity = 0;
	
	public bool randomRotation = false;
	
	public bool simulateInWorldSpace = false;
	public bool oneShot = false;
	
	public Vector3 area = Vector3.one;
	public float minEmitterRange = 0;
	
	public Mesh mesh;
	public Material material;
	
	private FakeParticle[] particles;
	//public Queue<FakeParticle> deadParticles;
	
	public Transform worldSpace;
	private static Transform fx;
	
	private int nextParticle;
	private float emitTime;
	
	void Awake() {
		if (fx == null) { 
			if (GameObject.Find("FX")) { fx = GameObject.Find("FX").transform; }
			else { fx = new GameObject("FX").transform; }
		}
		worldSpace = fx;
	}
	
	void Start() {
		//deadParticles = new Queue<FakeParticle>();
		if (particles == null || particles.Length == 0) {
			particles = new FakeParticle[maxParticles];
			for (int i = 0; i < maxParticles; i++) {
				particles[i] = MakeParticle();
			}
		}
		emitTime = 1.0f / Random.Range(minEmission, maxEmission);
		
	}
	
	void Update() {
		foreach (FakeParticle particle in particles) { particle.Tick(); }
		deadParticles = maxParticles - liveParticles;
		
		if (deadParticles == maxParticles && autodestruct) { Destroy(gameObject); }
		if (!emit) { return; }
		
		
		if (oneShot) {
			if (liveParticles == 0) {
				Emit((int)Random.Range(minEmission, maxEmission));
			}
		} else {
			emitTime -= Time.deltaTime;
			while (emitTime <= 0) {
				Emit();
				emitTime += 1.0f / Random.Range(minEmission, maxEmission);
			}
		}
		
	}
	
	FakeParticle MakeParticle() {
		GameObject obj = new GameObject("Particle");
		FakeParticle particle = obj.AddComponent<FakeParticle>() as FakeParticle;
		particle.system = this;
		particle.live = false;
		particle.transform.parent = transform;
		particle.billboard = obj.AddComponent<Billboard>();
		
		//deadParticles.Enqueue(particle);
		
		obj.AddComponent<MeshFilter>().mesh = mesh;
		obj.AddComponent<MeshRenderer>().material = material;
		obj.renderer.enabled = false;
		
		return particle;
	}
	
	public void EmitAll() {
		for (int i = 0; i < maxParticles; i++) {
			Emit();
		}
	}
	
	public void Emit(int num) {
		for (int i = 0; i < num; i++) {
			Emit();
		}
	}
	
	public void Emit() {
		EmitParticle(NextParticle());
	}
	
	public FakeParticle NextParticle() {
		nextParticle += 1;
		if (nextParticle >= maxParticles) { nextParticle = 0; }
		return particles[nextParticle];
	}
	
	public void EmitParticle(FakeParticle target) {
		liveParticles = Mathf.Min(maxParticles, liveParticles + 1);
		
		target.live = true;
		target.renderer.enabled = true;
		
		Vector3 offset = Vector3.Scale(Random.insideUnitSphere, area);
		if (offset.magnitude < minEmitterRange) { offset = offset.normalized * minEmitterRange; }
		target.transform.position = transform.position + offset;
		
		target.transform.localScale = Vector3.one * minSize;
		target.energy = Random.Range(minEnergy, maxEnergy);
		
		target.velocity = worldVelocity + Vector3.Scale(RandomF.insideUnitCube, randomVelocity);
		target.localVelocity = localVelocity + Vector3.Scale(RandomF.insideUnitCube, randomLocalVelocity);
		target.force = force + Vector3.Scale(RandomF.insideUnitCube, randomForce);
		
		target.angularVelocity = angularVelocity + (-.5f + Random.value) * randomAngularVelocity;
		if (randomRotation) { target.rotation = Random.value * 360; }
		
		
		
		
	}
	
	
	
}














