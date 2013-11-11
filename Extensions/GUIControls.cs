using UnityEngine;
using System.Collections;

public static class GUIControls {
	public static float defaultPadding { get { return GUIF.defaultPadding; } }
	
	public static bool ToggleButton(Rect area, string str, bool val) { return ToggleButton(area, str, val, defaultPadding); }
	public static bool ToggleButton(Rect area, string str, bool val, float padding) { return ToggleButton(area, str, val, padding, "MenuClick"); }
	public static bool ToggleButton(Rect area, string str, bool val, float padding, string sound) {
		bool ret = val;
		Color color = GUI.color;
		Color c = color;
		if (!val) { c = c.Half(); }
		GUI.color = c;
		if (GUIF.Button(area, str, padding, sound)) { ret = ! ret; }
		GUI.color = color;
		return ret;
	}
	
}
