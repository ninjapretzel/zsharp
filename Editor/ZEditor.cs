#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ZEditorWindow : EditorWindow {
	
	public float fieldWidth { get { return .6f * position.width; } }
	public bool changed = false;
	public bool checkChanges = false;
	
	
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
	
	public static bool Toggle(bool v, string label, params GUILayoutOption[] options) { return Toggle(v, label, options); } 
	public static bool Toggle(bool v, Texture label, params GUILayoutOption[] options) { return Toggle(v, label, options); } 
	public static bool Toggle(bool v, GUIContent label, params GUILayoutOption[] options) { return Toggle(v, label, options); } 
	public static bool Toggle(bool v, string label, string style, params GUILayoutOption[] options) { return Toggle(v, label, style, options); } 
	public static bool Toggle(bool v, Texture label, string style, params GUILayoutOption[] options) { return Toggle(v, label, style, options); } 
	public static bool Toggle(bool v, GUIContent label, string style, params GUILayoutOption[] options) { return Toggle(v, label, style, options); } 
	
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
	
	public static GUILayoutOption Height(float size) { return GUILayout.Height(size); }
	public static GUILayoutOption MinHeight(float val) { return GUILayout.MinHeight(val); }
	public static GUILayoutOption MaxHeight(float val) { return GUILayout.MaxHeight(val); }
	public static GUILayoutOption ExpandHeight(bool expand) { return GUILayout.ExpandHeight(expand); }
	
	public static GUILayoutOption Width(float size) { return GUILayout.Width(size); }
	public static GUILayoutOption MinWidth(float val) { return GUILayout.MinWidth(val); }
	public static GUILayoutOption MaxWidth(float val) { return GUILayout.MaxWidth(val); }
	public static GUILayoutOption ExpandWidth(bool expand) { return GUILayout.ExpandWidth(expand); }
	
	public string TextField(string label, string text) { return TextField(label, text, 1); }
	public string TextField(string label, string text, float scale) {
		string txt;
		BeginHorizontal("box");
			Label(label);
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