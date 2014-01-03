using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplaysItem : MonoBehaviour {
	Item item;
	public string itemName;
	public bool canDisplayTooltip = true;
	bool displayTooltip = false;
	
	Rect tooltipArea { get { return new Rect(0, 0, 240, 240); } }
	
	
	void Start() {
		item = Inventory.database.GetNamed(itemName);
	}
	
	void Update() {
		if (item != null) {
			renderer.material.mainTexture = item.icon;
		}
	}
	
	void OnGUI() {
		if (canDisplayTooltip && displayTooltip) {
			Vector3 pos = Input.mousePosition;
			pos.y = Screen.height - pos.y;
			
			Rect area = tooltipArea;
			area.x = pos.x;
			area.y = pos.y;
			
			
		}
		
	}
	
	void OnMouseEnter() {
		displayTooltip = true;
	}
	
	void OnMouseExit() {
		displayTooltip = false;
	}
	
	
}




















