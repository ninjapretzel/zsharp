using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetsSkins : MonoBehaviour {
	public GUISkinInfo[] skins;
	
	int lastWidth = 0;
	
	void Awake() { 
		GUISkins.Init();
		foreach (GUISkinInfo skin in skins) { skin.Add(); } 
	}
	
	void Update() {
		if (Screen.width != lastWidth) {
			foreach (GUISkinInfo skin in skins) { skin.Update(); }
			lastWidth = Screen.width;
		}
	}
	
}


public static class GUISkins {
	public static Dictionary<string, GUISkin> skins;
	public static Dictionary<string, FontSet> fonts;
	public static GUISkin blank;
	public static GUISkin skin;
	public static Stack<string> skinStack;
	public static string currentSkin;
	
	public static void Init() {
		skinStack = new Stack<string>();
		
	}

	
	public static void AddAll(FontSet[] s) { foreach (FontSet fs in s) { Add(fs); } }
	public static void Add(FontSet s) { Add(s.name, s); }
	public static void Add(string name, FontSet s) {
		if (fonts == null) { fonts = new Dictionary<string, FontSet>(); }
		if (fonts.ContainsKey(name)) { fonts[name] = s; }
		else { fonts.Add(name, s); }
	}
	
	public static void AddAll(GUISkinInfo[] s) { foreach (GUISkinInfo gs in s) { Add(gs); } }
	public static void Add(GUISkinInfo s) { Add(s.name, s.skin); }
	public static void Add(string name, GUISkin s) {
		if (skins == null) { skins = new Dictionary<string, GUISkin>(); }
		if (skins.ContainsKey(name)) { skins[name] = s; }
		else { skins.Add(name, s); }
		if (name == "blank") { blank = s; }
		if (name == "default") { skin = s; }
	}
	
	public static void Switch(string name) { 
		currentSkin = name;
		GUI.skin = Get(name);
	}
	
	public static void Push() { Push(currentSkin); }	
	public static void Push(string newSkin) {
		skinStack.Push(currentSkin);
		Switch(newSkin);
	}
	
	public static bool CanPop() {
		return skinStack.Count > 0;
	}
	
	public static void Pop() {
		Switch(skinStack.Pop());
	}
	
	public static GUISkin Get(string name) { 
		if (skins.ContainsKey(name)) { return skins[name]; }
		return null;
	}
	
}

[System.Serializable]
public class GUISkinInfo {
	public string name;
	public GUISkin skin;
	
	
	public void Add() { GUISkins.Add(name, skin); }
	
	public void Update() {
		
	}
	
}

public class FontSet {
	public string name;
	public GUISkin skin;
}
