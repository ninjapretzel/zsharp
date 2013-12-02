using UnityEngine;
using System.Collections;

public class ActivateBehaviourAfterDelay : DelayedAction {
	public string behaviour;
	
	public override void Action() {
		MonoBehaviour b = GetComponent(behaviour) as MonoBehaviour;
		b.enabled = true;
		Destroy(this);
	}
	
}
