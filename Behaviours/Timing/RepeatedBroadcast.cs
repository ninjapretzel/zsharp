using UnityEngine;
using System.Collections;

public class RepeatedBroadcast : RepeatedAction {

	public string message;
	public string param;
	
	public override void Action() {
		transform.SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
}
