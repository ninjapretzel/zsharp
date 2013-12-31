using UnityEngine;
using System.Collections;

public class GUIButtonAction : MonoBehaviour {
	
	
	void OnMouseDown() {
		SendMessage("Action", SendMessageOptions.DontRequireReceiver);
		
	}
}
