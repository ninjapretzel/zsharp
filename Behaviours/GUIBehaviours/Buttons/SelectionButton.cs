using UnityEngine;
using System.Collections;

public class SelectionButton : GUIButtonAction {

	public GUISelection container {
		get { return this.GetComponentAbove<GUISelection>(); }
	}
	
	
	public void Action() {
		GUISelection c = container;
		if(c != null) {
			c.Select(this);
			
		}
	}
	
}
