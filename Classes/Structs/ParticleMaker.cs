using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleMaker {

	public Vector3 position = Vector3.zero;
	public float size = 100;
	public Color color = Color.white;
	public float angularVelocity = 0;
	public float rotation = 0;
	
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