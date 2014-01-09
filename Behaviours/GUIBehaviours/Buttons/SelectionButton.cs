using UnityEngine;
using System.Collections;

public class SelectionButton : GUIButtonAction {

	public GUISelection container {
		get { return this.GetComponentAbove<GUISelection>(); }
	}
	
	// Use this for initialization
	public void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	
	}
	
	public void Action() {
		GUISelection c = container;
		if(c != null) {
			c.Select(this);
		}
	}
	
}
