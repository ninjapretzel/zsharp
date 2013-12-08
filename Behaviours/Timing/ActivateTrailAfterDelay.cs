using UnityEngine;
using System.Collections;

public class ActivateTrailAfterDelay : DelayedAction {
	
	public override void Action() {
		TrailRenderer tr = GetComponent<TrailRenderer>() as TrailRenderer;
		if (tr != null) {
			tr.enabled = true;
		}
		
	}
}
