using UnityEngine;
using System.Collections;

public class GUI2dTextField : MonoBehaviour {
	public string value = "";
	public GUISkin skin;
	public Rect area = new Rect(0,0,.2f,.2f);
	public float fontSize = 32;
	
	
	void OnGUI() {
		Rect brush = transform.ToScreenArea(area.width, area.height);
		brush.x += area.x * Screen.width;
		brush.y += area.y * Screen.height;
		GUISkin sk = skin;
		if (sk == null) { sk = GUI.skin; }
		GUIStyle style = sk.textField.FontSize(fontSize);
		value = GUI.TextField(brush, value, style);
		
		
	}
	
}