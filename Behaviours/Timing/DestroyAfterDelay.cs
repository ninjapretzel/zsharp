using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : DelayedAction {
	
	public override void Action() {
		Destroy(gameObject);
	}
	
	
}
