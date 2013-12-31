using UnityEngine;
using System.Collections;

public class ScrollingButton : GUIButtonAction {
	public GUIScrollableArea target;
	public Vector2 scroll;
	
	void Action() {
		target.scrollPosition += scroll;
	}
	
}
