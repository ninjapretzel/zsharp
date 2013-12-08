using UnityEngine;
using System.Collections;

public class RemoveBehaviourAfterDelay : DelayedAction {
	public string behaviour;
	
	public override void Action() {
		MonoBehaviour b = GetComponent(behaviour) as MonoBehaviour;
		Destroy(b);
		Destroy(this);
	}
	
}
