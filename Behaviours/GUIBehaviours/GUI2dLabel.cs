
using UnityEngine;
using System.Collections; 

public class GUI2dLabel : MonoBehaviour {
	public string label = "GUILabel";
	public Color color = Color.white;
	public float outline = 2;
	public float fontSize = 26;
	public Rect area = new Rect(0, 0, .33f, .33f);
	
	void OnGUI() {
		Rect brush = transform.ToScreenArea(area.width, area.height);
		brush.x += area.x * Screen.width;
		brush.y += area.y * Screen.height;
		
		GUIStyle style = GUI.skin.label.Aligned(TextAnchor.MiddleCenter).FontSize(fontSize);
		GUI.color = color;
		
		GUIStyle prevStyle = GUI.skin.label;
		GUI.skin.label = style;
		GUIF.Label(brush, label, outline);
		
		GUI.skin.label = prevStyle;
	}
}