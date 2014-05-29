#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ZEditorWindow : EditorWindow {
	
	public float fieldWidth { get { return .6f * position.width; } }
	public bool changed = false;
	public bool checkChanges = false;
	
	
	public virtual Color changedColor { get { return new Color(1, .5f, .5f, 1); } }
	public void SetChangedColor() { SetChangedColor(changed); }
	public void SetChangedColor(bool b) {
		GUI.color = Color.white;
		if (b) { GUI.color = changedColor; }
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////
	//Generic wrapper functions
	
	public static void Label(string content, params GUILayoutOption[] options) { GUILayout.Label(content, options); }
	public static void Label(Texture content, params GUILayoutOption[] options) { GUILayout.Label(content, options); }
	public static void Label(GUIContent content, params GUILayoutOption[] options) { GUILayout.Label(content, options); }
	public static void Label(string content, string style, params GUILayoutOption[] options) { GUILayout.Label(content, style, options); }
	public static void Label(Texture content, string style, params GUILayoutOption[] options) { GUILayout.Label(content, style, options); }
	public static void Label(GUIContent content, string style, params GUILayoutOption[] options) { GUILayout.Label(content, style, options); }
	
	public static void Box(string content, params GUILayoutOption[] options) { GUILayout.Box(content, options); }
	public static void Box(Texture content, params GUILayoutOption[] options) { GUILayout.Box(content, options); }
	public static void Box(GUIContent content, params GUILayoutOption[] options) { GUILayout.Box(content, options); }
	public static void Box(string content, string style, params GUILayoutOption[] options) { GUILayout.Box(content, style, options); }
	public static void Box(Texture content, string style, params GUILayoutOption[] options) { GUILayout.Box(content, style, options); }
	public static void Box(GUIContent content, string style, params GUILayoutOption[] options) { GUILayout.Box(content, style, options); }
	
	public static bool Button(string content, params GUILayoutOption[] options) { return GUILayout.Button(content, options); }
	public static bool Button(Texture content, params GUILayoutOption[] options) { return GUILayout.Button(content, options); }
	public static bool Button(GUIContent content, params GUILayoutOption[] options) { return GUILayout.Button(content, options); }
	public static bool Button(string content, string style, params GUILayoutOption[] options) { return GUILayout.Button(content, style, options); }
	public static bool Button(Texture content, string style, params GUILayoutOption[] options) { return GUILayout.Button(content, style, options); }
	public static bool Button(GUIContent content, string style, params GUILayoutOption[] options) { return GUILayout.Button(content, style, options); }
	
	public static bool RepeatButton(string content, params GUILayoutOption[] options) { return GUILayout.RepeatButton(content, options); }
	public static bool RepeatButton(Texture content, params GUILayoutOption[] options) { return GUILayout.RepeatButton(content, options); }
	public static bool RepeatButton(GUIContent content, params GUILayoutOption[] options) { return GUILayout.RepeatButton(content, options); }
	public static bool RepeatButton(string content, string style, params GUILayoutOption[] options) { return GUILayout.RepeatButton(content, style, options); }
	public static bool RepeatButton(Texture content, string style, params GUILayoutOption[] options) { return GUILayout.RepeatButton(content, style, options); }
	public static bool RepeatButton(GUIContent content, string style, params GUILayoutOption[] options) { return GUILayout.RepeatButton(content, style, options); }
	
	public static bool Toggle(bool v, string label, params GUILayoutOption[] options) { return GUILayout.Toggle(v, label, options); } 
	public static bool Toggle(bool v, Texture label, params GUILayoutOption[] options) { return GUILayout.Toggle(v, label, options); } 
	public static bool Toggle(bool v, GUIContent label, params GUILayoutOption[] options) { return GUILayout.Toggle(v, label, options); } 
	public static bool Toggle(bool v, string label, string style, params GUILayoutOption[] options) { return GUILayout.Toggle(v, label, style, options); } 
	public static bool Toggle(bool v, Texture label, string style, params GUILayoutOption[] options) { return GUILayout.Toggle(v, label, style, options); } 
	public static bool Toggle(bool v, GUIContent label, string style, params GUILayoutOption[] options) { return GUILayout.Toggle(v, label, style, options); } 
	
	public static float HorizontalSlider(float value, float left, float right, params GUILayoutOption[] options)
	{ return GUILayout.HorizontalSlider(value, left, right, options); }
	public static float HorizontalScrollbar(float value, float size, float left, float right, params GUILayoutOption[] options)
	{ return GUILayout.HorizontalScrollbar(value, size, left, right, options); }
	public static float HorizontalScrollbar(float value, float size, float left, float right, string style, params GUILayoutOption[] options)
	{ return GUILayout.HorizontalScrollbar(value, size, left, right, style, options); }
	
	public static float VerticalSlider(float value, float left, float right, params GUILayoutOption[] options)
	{ return GUILayout.VerticalSlider(value, left, right, options); }
	public static float VerticalScrollbar(float value, float size, float left, float right, params GUILayoutOption[] options)
	{ return GUILayout.VerticalScrollbar(value, size, left, right, options); }
	public static float VerticalScrollbar(float value, float size, float left, float right, string style, params GUILayoutOption[] options)
	{ return GUILayout.VerticalScrollbar(value, size, left, right, style, options); }
	
	public static void FlexibleSpace() { GUILayout.FlexibleSpace(); }
	public static void Space(float size) { GUILayout.Space(size); }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////
	//Layout wrappers
	
	public static Vector2 BeginScrollView(Vector2 pos, params GUILayoutOption[] options) { return GUILayout.BeginScrollView(pos, options); }
	public static Vector2 BeginScrollView(Vector2 pos, bool h, bool v, params GUILayoutOption[] options) { return GUILayout.BeginScrollView(pos, h, v, options); }	
	public static void EndScrollView() { GUILayout.EndScrollView(); }
	
	public static void BeginArea(Rect area) { GUILayout.BeginArea(area); }
	public static void BeginArea(Rect area, string style) { GUILayout.BeginArea(area, style); }
	public static void EndArea() { GUILayout.EndArea(); }
	
	public static void BeginVertical(params GUILayoutOption[] options) { EditorGUILayout.BeginVertical(options); }
	public static void BeginVertical(string style, params GUILayoutOption[] options) { EditorGUILayout.BeginVertical(style, options); }
	public static void EndVertical() { EditorGUILayout.EndVertical(); }
	
	public static void BeginHorizontal(params GUILayoutOption[] options) { EditorGUILayout.BeginHorizontal(options); }
	public static void BeginHorizontal(string style, params GUILayoutOption[] options) { EditorGUILayout.BeginHorizontal(style, options); }
	public static void EndHorizontal() { EditorGUILayout.EndHorizontal(); }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////
	//Option Wrappers
	
	public static GUILayoutOption Height(float size) { return GUILayout.Height(size); }
	public static GUILayoutOption MinHeight(float val) { return GUILayout.MinHeight(val); }
	public static GUILayoutOption MaxHeight(float val) { return GUILayout.MaxHeight(val); }
	public static GUILayoutOption ExpandHeight(bool expand) { return GUILayout.ExpandHeight(expand); }
	
	public static GUILayoutOption Width(float size) { return GUILayout.Width(size); }
	public static GUILayoutOption MinWidth(float val) { return GUILayout.MinWidth(val); }
	public static GUILayoutOption MaxWidth(float val) { return GUILayout.MaxWidth(val); }
	public static GUILayoutOption ExpandWidth(bool expand) { return GUILayout.ExpandWidth(expand); }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////
	//Field wrappers
	
	public static string TextField(string text) { return EditorGUILayout.TextField(text); }
	public static string TextField(string text, params GUILayoutOption[] options) { return EditorGUILayout.TextField(text, options); }
	
	public static float FloatField(float val) { return EditorGUILayout.FloatField(val); }
	public static float FloatField(float val, params GUILayoutOption[] options) { return EditorGUILayout.FloatField(val, options); }
	
	public static int IntField(int val) { return EditorGUILayout.IntField(val); }
	public static int IntField(int val, params GUILayoutOption[] options) { return EditorGUILayout.IntField(val, options); }
	
	public static Color ColorField(Color color) { return EditorGUILayout.ColorField(color); }
	public static Color ColorField(Color color, params GUILayoutOption[] options) { return EditorGUILayout.ColorField(color, options); }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////
	//Placement functions
	
	public static void FixedLabel(string content) { GUILayout.Label(content, ExpandWidth(false), ExpandHeight(false)); }
	public static void FixedBox(string content) { GUILayout.Box(content, ExpandWidth(false), ExpandHeight(false)); }
	public static bool FixedButton(string content) { return GUILayout.Button(content, ExpandWidth(false), ExpandHeight(false)); }
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////
	//Custom fields.
	//These will automatically adjust the changed flag.
	//These are not static because they rely on the current window's flag.
	public string TextFieldC(string text) { return TextField("", text, 1); }
	public string TextField(string text, float scale) { return TextField("", text, scale); }
	public string TextField(string label, string text) { return TextField(label, text, 1); }
	public string TextField(string label, string text, float scale) {
		string txt;
		BeginHorizontal("box");
			if (label != "") { Label(label); }
			txt = EditorGUILayout.TextField(text, Width(fieldWidth * scale));
			changed = changed || (checkChanges && (txt != text));
		EndHorizontal();
		return txt;
	}
	
	public string TextArea(string label, string text) {
		string txt;
		BeginHorizontal("box");
			Label(label);
			txt = EditorGUILayout.TextArea(text, Width(fieldWidth));
			changed = changed || (checkChanges && (txt != text));
		EndHorizontal();
		return txt;
	}
	
	public List<string> StringListField(string label, List<string> list) { return StringListField(label, list, 1); }
	public List<string> StringListField(string label, List<string> list, float scale) {
		string str = list.ListString();
		string rec = TextField(label, str);
		
		if (str == rec) { return list; }
		return rec.ParseStringList();
	}
	
	public List<Color> ColorListField(string label, List<Color> list, int perLine) {
		List<Color> l = new List<Color>();
		BeginVertical("box"); {
			Label(label);
			
			BeginHorizontal(); {
				for (int i = 0; i < list.Count; i++) {
					BeginVertical("box", Width(50)); {
						
						Color c = EditorGUILayout.ColorField(list[i], Width(40), ExpandWidth(false));
						
						BeginHorizontal(); {
							if (Button("D", ExpandWidth(false))) { 
								changed = true; 
								l.Add(list[i]); 
							}
							
							if (!Button("-", ExpandWidth(false))) { 
								l.Add(c); 
							} else {
								changed = true;
							}
						} EndHorizontal();
						
					} EndVertical();
					
					
					if ((i+1)%perLine == 0) {
						EndHorizontal();
						BeginHorizontal();
					}
					
				}
				if (Button("+", ExpandWidth(false))) { l.Add(Color.white); }
				
			} EndHorizontal();
			
		} EndVertical();
		
		return l;
	}
	
	public bool ToggleField(string label, bool val) { return ToggleField(val, label); }
	public bool ToggleField(bool val, string label) {
		bool b = Toggle(val, label, ExpandWidth(false));
		changed = changed || (checkChanges && (b != val));
		return b;
	}
	
	public MM RangeField(string label, MM range) { return RangeField(label, range, 1); }
	public MM RangeField(string label, MM range, float scale) {
		MM v = new MM(range.ToString());
		
		BeginHorizontal("box");
			FixedLabel(label);
			v.min = EditorGUILayout.FloatField(range.min, Width(fieldWidth * scale/2f));
			FixedLabel("-");
			v.max = EditorGUILayout.FloatField(range.max, Width(fieldWidth * scale/2f));
		EndHorizontal();
		
		changed = changed || (checkChanges && (!v.Equals(range)));
		
		return v;
	}
	
	public float FloatField(string label, float val) { return FloatField(label, val, 1); }
	public float FloatField(string label, float val, float scale) {
		float v;
		BeginHorizontal("box");
			FixedLabel(label);
			v = EditorGUILayout.FloatField(val, Width(fieldWidth * scale));
			changed = changed || (checkChanges && (v != val));
		EndHorizontal();
		
		return v;
	}
	
	
	
	public int IntField(string label, int val) { return IntField(label, val, 1); }
	public int IntField(string label, int val, float scale) {
		int v;
		BeginHorizontal("box");
			FixedLabel(label);
			v = EditorGUILayout.IntField(val, Width(fieldWidth * scale));
			changed = changed || (checkChanges && (v != val));
		EndHorizontal();
		
		return v;
	}
	
	public Color ColorField(string label, Color color) { return ColorField(label, color, .2f); }
	public Color ColorField(string label, Color color, float scale) {
		Color c;
		BeginHorizontal("box");
			FixedLabel(label);
			c = EditorGUILayout.ColorField(color, Width(fieldWidth * scale));
			changed = changed || (checkChanges && (c != color));
		EndHorizontal();
		
		return c;
	}
	
	
}




















#endif