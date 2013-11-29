using UnityEngine;
using System.Collections;

public class DealsDamage : MonoBehaviour {
	public Unit source;
	public Attack atk;
	
	
	void OnTriggerEnter(Collider c) {
		Unit unit = c.GetComponent<Unit>();
		
		if (source != null && unit != null) {
			if (source.team != unit.team) {
				unit.mortality.Hit(atk);
				HitATarget();
			}
			HitATeammate();
			
		}
		
	}
	
	public virtual void HitATarget() {}
	public virtual void HitATeammate() {}
	
	
}
