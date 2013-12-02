using UnityEngine;
using System.Collections;

public class SendMessageAfterDelay : DelayedAction {
	public string message;
	
	public override void Action() {
		SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
	
}
