using UnityEngine;
using System.Collections;

public class DealsDamageOverTime : DealsDamage {
	
	public float damage;
	
	void Start() {
		source = GetComponent<Unit>();
		
	}
	
	
	void OnTriggerEnter(Collider c) { }
	
	void OnTriggerStay(Collider c) {
		Unit unit = c.GetComponent<Unit>();
		if (source != null && unit != null) {
			if (source.team != unit.team) {
				//Debug.Log("OH SHIT" + c.gameObject.name);
				//Debug.Log(atk);
				unit.mortality.Hit(damage * Time.deltaTime);
				HitATarget();
			}
			HitATeammate();
			
		}
	}
	
}
