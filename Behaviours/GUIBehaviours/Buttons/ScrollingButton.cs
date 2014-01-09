using UnityEngine;
using System.Collections;

public class ScrollingButton : GUIButtonAction {
	public GUIScrollableArea target;
	public Vector2 scroll;
	
	void Start() {
		if (target == null) { 
			target = transform.parent.GetComponent<GUIScrollableArea>();
		}
	}
	
	void Action() {
		if (target != null) {
			target.scrollPosition += scroll;
		}
	}
	
}
