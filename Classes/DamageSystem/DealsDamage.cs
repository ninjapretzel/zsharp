using UnityEngine;
using System.Collections;

public class DealsDamage : MonoBehaviour {
	public Unit source;
	public bool sticksToSource = false;
	public Attack atk;
	public AudioClip playWhenTargetKilled;
	
	void OnTriggerEnter(Collider c) {
		Unit unit = c.GetComponent<Unit>();
		if (source != null && unit != null) {
			if (source.team != unit.team) {
				//Debug.Log("OH SHIT" + c.gameObject.name);
				//Debug.Log(atk);
				unit.mortality.Hit(atk, playWhenTargetKilled);
				HitATarget();
			}
			HitATeammate();
			
		}
		
	}
	
	public virtual void HitATarget() {}
	public virtual void HitATeammate() {}
	
	
}
