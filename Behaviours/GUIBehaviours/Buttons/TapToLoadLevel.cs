using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TapToLoadLevel : GUIButtonAction {
	
	public string levelToLoad;
	
	public void Action() {
		
		Application.LoadLevel(levelToLoad);
		
	}	
	
}




















