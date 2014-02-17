using UnityEngine;
using System.Collections;

public class GUIButtonAction : MonoBehaviour {
	
	
	void OnMouseUpAsButton() {
		SendMessage("Action", SendMessageOptions.DontRequireReceiver);
		
	}
}
