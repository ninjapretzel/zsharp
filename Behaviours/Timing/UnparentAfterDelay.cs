using UnityEngine;
using System.Collections;

public class UnparentAfterDelay : DelayedAction {

	public override void Action() {
		transform.parent = null;
	}
	
}
