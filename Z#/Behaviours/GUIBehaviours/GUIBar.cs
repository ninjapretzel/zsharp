using UnityEngine;
using System.Collections;

public class GUIBar : MonoBehaviour {
	public float val = .5f;
	public int depth = 0;
	public string skin = "";
	public Rect area = new Rect(.3f, .9f, .4f, .1f);
	public Rect repeat = new Rect(0, 0, 1, 1);
	public Texture2D tex;
	
	public Color tint = Color.white;
	public Color back = Color.black;
	
	public string message = "";
	public Color messageTint = new Color(0, .6f, 1f);
	
	public int padding = 2;
	
	void OnGUI() {
		if (skin != "") { GUI.skin = GUISkins.Get(skin); }
		GUI.depth = depth;
		Rect brush = area;
		//Debug.Log("dicks");
		Bars.graphic = tex;
		Bars.Draw(brush, repeat, val, tint, back, padding);
		
		if (message != "") {
			GUI.color = messageTint;
			GUIF.Label(brush, message);
		}
	}
}
