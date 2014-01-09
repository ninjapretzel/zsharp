using UnityEngine;
using System.Collections;

public class GUISelection : MonoBehaviour {

	public GameObject selection;
	public float selectionSize = 1.2f;
	
	public void Select(Component c) {
		
		Select(c.gameObject);
	}
	
	public void Select(GameObject o) {
		Deselect();
		
		selection = o;
		if (selection != null) {
			ScaleController sc = selection.Require<ScaleController>();
			sc.targetSize = selectionSize;
		}
		
	}
	
	void Deselect() {
		if (selection != null) {
			ScaleController sc = selection.Require<ScaleController>();
			sc.targetSize = 1;
			
			
		} else { return; }
		selection = null;
	}
}
