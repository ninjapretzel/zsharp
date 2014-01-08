using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplaysItem : MonoBehaviour {
	Item item;
	public string itemName;
	public bool canDisplayTooltip = true;
	public Renderer icon;
	public TextMesh textMesh;
	bool displayTooltip = false;
	
	Rect tooltipArea { get { return new Rect(0, 0, 240, 240); } }
	
	
	void Start() {
		item = Item.database.GetNamed(itemName);
		
		if (!icon) { icon = transform.GrabFromChild<Renderer>("Icon"); }
		if (!textMesh) { textMesh = transform.GrabFromChild<TextMesh>("Name"); }
		
		
	}
	
	void Update() {
		if (item != null) {
			icon.material.mainTexture = item.icon;
			textMesh.text = item.name;
			
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




















