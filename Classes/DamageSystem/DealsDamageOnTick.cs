using UnityEngine;
using System.Collections;

public class DealsDamageOnTick : DealsDamage {
	
	public float damage;
	public bool baseDamageOnDistance = false;
	public float maxDistance = 15;
	
	public float tickTime = 1;
	float timeout = 0;
	
	Set<Unit> contained;
	
	void Awake() {
		contained = new Set<Unit>();
	}
	
	void Start() {
		source = GetComponent<Unit>();
	}
	
	void Update() {
		timeout += Time.deltaTime;
		if (timeout >= tickTime) {
			timeout -= tickTime;
			Tick();
		}
	}
	
	void Tick() {
		foreach (Unit u in contained) { Tick(u); }
	}
	
	
	void Tick(Unit u) {
		float d = damage;
		if (baseDamageOnDistance) {
			d *= Mathf.Clamp01(1 - (u.transform.position - transform.position).magnitude / maxDistance);
		}
		u.mortality.Hit(d * Time.deltaTime);
	}
	
	void OnTriggerEnter(Collider c) { 
		Unit test = c.GetComponent<Unit>();
		if (test != null && test.team != source.team) {
			contained.Add(test);
		}
		
	}
	
	void OnTriggerExit(Collider c) {
		Unit test = c.GetComponent<Unit>();
		if (test != null) {
			contained.Remove(test);
		}
	}
	
	
}
