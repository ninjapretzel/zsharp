#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ZEditorWindow : EditorWindow {
	
	public virtual float fieldWidth { get { return .6f * position.width; } }
	public bool changed = false;
	public bool checkChanges = false;
	
	public string TextField(string label, string text) { return TextField(label, text, 1); }
	public string TextField(string label, string text, float scale) {
		string txt;
		GUILayout.BeginHorizontal("box");
			GUILayout.Label(label);
			txt = EditorGUILayout.TextField(text, GUILayout.Width(fieldWidth * scale));
			changed = changed || (checkChanges && (txt != text));
		GUILayout.EndHorizontal();
		return txt;
	}
	
	public string TextArea(string label, string text) {
		string txt;
		GUILayout.BeginHorizontal("box");
			GUILayout.Label(label);
			txt = EditorGUILayout.TextArea(text, GUILayout.Width(fieldWidth));
			changed = changed || (checkChanges && (txt != text));
		GUILayout.EndHorizontal();
		return txt;
	}
	
	public float FloatField(string label, float val) { return FloatField(label, val, 1); }
	public float FloatField(string label, float val, float scale) {
		float v;
		GUILayout.BeginHorizontal("box");
			GUILayout.Label(label);
			v = EditorGUILayout.FloatField(val, GUILayout.Width(fieldWidth * scale));
			changed = changed || (checkChanges && (v != val));
		GUILayout.EndHorizontal();
		
		return v;
	}
	
	public int IntField(string label, int val) { return IntField(label, val, 1); }
	public int IntField(string label, int val, float scale) {
		int v;
		GUILayout.BeginHorizontal("box");
			GUILayout.Label(label);
			v = EditorGUILayout.IntField(val, GUILayout.Width(fieldWidth));
			changed = changed || (checkChanges && (v != val));
		GUILayout.EndHorizontal();
		
		return v;
	}
	
	public Color ColorField(string label, Color color) { return ColorField(label, color, .2f); }
	public Color ColorField(string label, Color color, float scale) {
		Color c;
		GUILayout.BeginHorizontal("box");
			GUILayout.Label(label);
			c = EditorGUILayout.ColorField(color, GUILayout.Width(fieldWidth * scale));
			changed = changed || (checkChanges && (c != color));
		GUILayout.EndHorizontal();
		
		return c;
	}
	
	
}




















#endif