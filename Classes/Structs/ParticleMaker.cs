using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleMaker {

	public Vector3 position = Vector3.zero;
	
	public float size = 1;
	public Color color = Color.white;
	public float angularVelocity = 0;
	public float rotation = 0;
	
	public virtual void Update() {}
	
	public virtual ParticleSystem.Particle particle {
		get {
			ParticleSystem.Particle p = new ParticleSystem.Particle();
			
			p.position = position;
			
			p.size = size;
			p.color = color;
			p.rotation = rotation + angularVelocity * Time.time;
			
			return p;
		}
	}
	
}

public class TimedParticleMaker : ParticleMaker { 
	
	public float lifeTime = 0;
	
	public float sizeChange = 0;
	public Vector3 velocity = Vector3.zero;
	public AnimationCurve alphaOverTime;
	
	public static AnimationCurve baseCurve = new AnimationCurve(
		new Keyframe(0, 1),
		new Keyframe(2, 0));
		
	
	public override void Update() { lifeTime += Time.deltaTime; }
	
	public override ParticleSystem.Particle particle {
		get {
			ParticleSystem.Particle p = base.particle;
			
			p.position += velocity * lifeTime;
			p.size += sizeChange * lifeTime;
			if (p.size < 0) { p.size = 0; }
			
			Color c = p.color;
			c.a = alphaOverTime.Evaluate(lifeTime);
			p.color = c;
			
			return p;
		}
	}
	
}

public class DummyParticleMaker : ParticleMaker {
	
	
	public DummyParticleMaker() {
		color = Color.clear;
	}
	
}