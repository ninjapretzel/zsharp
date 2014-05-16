using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class GUIF {
	public static GUISkin blankSkin { get { return Resources.Load("blank", typeof(GUISkin)) as GUISkin; } }
	public const float defaultPadding = 1.0f;
	
	public static List<SelectableControl> controls = new List<SelectableControl>();
	//public static SelectableControl[] selectableControls = new SelectableControl[0];
	
	public static int currentSelectedControl = -1;
	public static bool controlHit = false;
	
	public static Texture2D pixel { get { return Resources.Load("pixel", typeof(Texture2D)) as Texture2D; } }
	
	public static Rect screen { 
		get { return new Rect(0, 0, Screen.width, Screen.height); }
	}
	
	
	public static void Label(Rect area, string str, float padding) { Label(area, new GUIContent(str), padding); }
	public static void Label(Rect area, string str) { Label(area, str, defaultPadding); }
	public static void Label(Rect area, GUIContent content) { Label(area, content, defaultPadding); }
	public static void Label(Rect area, GUIContent content, float padding) {
		if (padding > 0) {
			float alpha = GUI.color.a;
			Color c = GUI.color;
			Color contentColor = GUI.contentColor;
			if (alpha < 1) {
				Color cc = GUI.color;
				cc.a *= .325f; 
				GUI.color = cc;
			}
			GUI.contentColor = Color.black;
			GUI.Label(area.Shift(-padding, -padding), content);
			GUI.Label(area.Shift(padding, -padding), content);
			GUI.Label(area.Shift(-padding, padding), content);
			GUI.Label(area.Shift(padding, padding), content);
			
			GUI.Label(area.Shift(0, padding), content);
			GUI.Label(area.Shift(0, -padding), content);
			GUI.Label(area.Shift(padding, 0), content);
			GUI.Label(area.Shift(-padding, 0), content);
			
			GUI.color = c;
			GUI.contentColor = contentColor;
		}
		GUI.Label(area, content);
	}
	
	public static void Label(Rect area, string str, GUIStyle callingFrom) { Label(area, new GUIContent(str), defaultPadding, callingFrom); }
	public static void Label(Rect area, Texture2D tex, GUIStyle callingFrom) { Label(area, new GUIContent(tex), defaultPadding, callingFrom); }
	public static void Label(Rect area, string str, Texture2D tex, GUIStyle callingFrom) { Label(area, new GUIContent(str, tex), defaultPadding, callingFrom); }
	public static void Label(Rect area, string str, float padding, GUIStyle callingFrom) { Label(area, new GUIContent(str), padding, callingFrom); }
	public static void Label(Rect area, string str, Texture2D tex, float padding, GUIStyle callingFrom) { Label(area, new GUIContent(str, tex), padding, callingFrom); }
	public static void Label(Rect area, GUIContent content, float padding, GUIStyle callingFrom) {
		//GUIStyle prevStyle = GUI.skin.label;
		//GUI.skin.label = callingFrom;
		TextAnchor prevAlign = GUI.skin.label.alignment;
		ImagePosition prevImagePos = GUI.skin.label.imagePosition;
		RectOffset prevBorder = GUI.skin.label.border;
		RectOffset prevMargin = GUI.skin.label.margin;
		RectOffset prevPadding = GUI.skin.label.padding;
		RectOffset prevOverflow = GUI.skin.label.overflow;
		
		GUI.skin.label.alignment = callingFrom.alignment;
		GUI.skin.label.imagePosition = callingFrom.imagePosition;
		GUI.skin.label.border = callingFrom.border;
		GUI.skin.label.margin = callingFrom.margin;
		GUI.skin.label.padding = callingFrom.padding;
		GUI.skin.label.overflow = callingFrom.overflow;
		
		Label(area, content, padding);

		GUI.skin.label.alignment = prevAlign;
		GUI.skin.label.imagePosition = prevImagePos;
		GUI.skin.label.border = prevBorder;
		GUI.skin.label.margin = prevMargin;
		GUI.skin.label.padding = prevPadding;
		GUI.skin.label.overflow = prevOverflow;
		//GUI.skin.label = prevStyle;
	}
	
	public static void Box(Rect area, Texture2D t) { Box(area, new GUIContent(t), defaultPadding); }
	public static void Box(Rect area, string str) { Box(area, new GUIContent(str), defaultPadding); }
	public static void Box(Rect area, string str, float padding) { Box(area, new GUIContent(str), padding); }
	public static void Box(Rect area, string str, Texture2D tex) { Box(area, new GUIContent(str, tex), defaultPadding); }
	public static void Box(Rect area, string str, float padding, Texture2D tex) { Box(area, new GUIContent(str, tex), padding); }
	public static void Box(Rect area, GUIContent c) { Box(area, c, defaultPadding); }
	public static void Box(Rect area, GUIContent c, float padding) { 
		GUI.Box(area, "");
		Label(area, c, padding, GUI.skin.box);
	}
	
	
	public static void Box(Rect area, Texture2D t, GUIStyle style) { Box(area, new GUIContent(t), defaultPadding, style); }
	public static void Box(Rect area, Texture2D t, float padding, GUIStyle style) { Box(area, new GUIContent(t), padding, style); }
	public static void Box(Rect area, string s, GUIStyle style) { Box(area, new GUIContent(s), defaultPadding, style); }
	public static void Box(Rect area, string s, Texture2D t, GUIStyle style) { Box(area, new GUIContent(s,t), defaultPadding, style); }
	public static void Box(Rect area, string s, float padding, GUIStyle style) { Box(area, new GUIContent(s), padding, style); }
	public static void Box(Rect area, string s, Texture2D t, float padding, GUIStyle style) { Box(area, new GUIContent(s,t), padding, style); }
	public static void Box(Rect area, GUIContent c, GUIStyle style) { Box(area, c, defaultPadding, style); }
	public static void Box(Rect area, GUIContent c, float padding, GUIStyle style) {
		GUIStyle oldBox = GUI.skin.box;
		GUI.skin.box = style;
		Box(area, c, padding);
		GUI.skin.box = oldBox;
	}
	
	public static void Background(Rect area, string str, Color background, GUIStyle style) { Background(area, str, background, style, Vector2.zero); }
	public static void Background(Rect area, string str, Color background, GUIStyle style, Vector2 trim) {
		Rect brush = GetArea(area, str, style);
		brush = brush.Trim(trim);
		
		Color prev = GUI.color;
		GUI.color = background;
		GUI.DrawTexture(brush, Resources.Load("pixel", typeof(Texture2D)) as Texture2D);
		GUI.color = prev;
	}
	
	public static bool InvisibleButton(Rect area) {
		GUISkin oldSkin = GUI.skin;
		GUI.skin = blankSkin;
		bool ret = GUI.Button(area, "");
		GUI.skin = oldSkin;
		return ret;
	}
	
	public static bool TouchButtonDown(Rect area) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Began) {
				Vector2 pos = t.position;
				pos.y = Screen.height - pos.y;
				if (area.Contains(pos)) {
					return true;
				}
			}
		}
		return false;
	}
	
	public static bool TouchButton(Rect area) {
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved) { 
				Vector2 pos = t.position;
				pos.y = Screen.height - pos.y;
				if (area.Contains(pos)) {
					return true;
				}
			}
		}
		return false;
	}
	
	public static bool Button(Rect area, string str) { return Button(area, str, defaultPadding, "MenuSelect"); }
	public static bool Button(Rect area, string str, string sound) { return Button(area, str, defaultPadding, sound); }
	public static bool Button(Rect area, string str, float padding) { return Button(area, new GUIContent(str), defaultPadding, "MenuSelect"); }
	public static bool Button(Rect area, string str, float padding, string sound) { return Button(area, new GUIContent(str), defaultPadding, sound); }
	public static bool Button(Rect area, GUIContent c) { return Button(area, c, defaultPadding, "MenuSelect"); }
	public static bool Button(Rect area, GUIContent c, float padding) { return Button(area, c, defaultPadding, "MenuSelect"); }
	public static bool Button(Rect area, GUIContent c, float padding, string sound) {
		bool ret = GUI.Button(area, "");
		Label(area, c, padding, GUI.skin.button);
		if (ret) { SoundMaster.Play(sound); }
		return ret;
	}
	
	public static bool ToggleButton(Rect area, bool toggle, string str) {
		string s = str + " " + (toggle ? "[x]" : "[ ]");
		if (GUI.Button(area, s)) {
			return !toggle;
		}
		return toggle;
	}
	
	public static Rect GetArea(Rect area, GUIContent c, GUIStyle style) { return GetArea(area, style.CalcSize(c), style); }
	public static Rect GetArea(Rect area, string str, GUIStyle style) { return GetArea(area, style.CalcSize(new GUIContent(str)), style); }
	public static Rect GetArea(Rect area, Vector2 size, GUIStyle style) {
		Vector2 s = new Vector2(size.x/area.width, size.y/area.height);
		
		switch (style.alignment) {
			case TextAnchor.UpperLeft: 		return area.UpperLeft(s); 		
			case TextAnchor.UpperCenter:	return area.UpperCenter(s);		
			case TextAnchor.UpperRight: 	return area.UpperRight(s);		
			case TextAnchor.MiddleLeft:		return area.MiddleLeft(s);		
			case TextAnchor.MiddleCenter: 	return area.MiddleCenter(s);	
			case TextAnchor.MiddleRight:	return area.MiddleRight(s);		
			case TextAnchor.LowerLeft: 		return area.BottomLeft(s);		
			case TextAnchor.LowerCenter:	return area.BottomCenter(s);	
			case TextAnchor.LowerRight: 	return area.BottomRight(s);		
		}
		return area;
	}
	
	public static Vector4 EditorSelectionArea(Rect area, Vector4 scrollPosition, Rect entrySize, string[] names) {
		return EditorSelectionArea(area, scrollPosition, entrySize, names, new Color(.7f, .7f, 1), false); }
	public static Vector4 EditorSelectionArea(Rect area, Vector4 scrollPosition, Rect entrySize, string[] names, bool showNumbers) {
		return EditorSelectionArea(area, scrollPosition, entrySize, names, new Color(.7f, .7f, 1), showNumbers); }
	public static Vector4 EditorSelectionArea(Rect area, Vector4 scrollPosition, Rect entrySize, string[] names, Color selectionColor, bool showNumbers) {
		Rect scrollableArea = entrySize;
		float rem = 0;
		scrollableArea.height *= names.Length;
		int selected = (int)scrollPosition.z;
		int doSelection = -1;
		Vector2 pos = new Vector2(scrollPosition.x, scrollPosition.y);
		GUISkin lastSkin = GUI.skin;
		Color lastColor = GUI.color;
		
		Rect brush = entrySize;
		pos = GUI.BeginScrollView(area, pos, scrollableArea);
			for (int i = 0; i < names.Length; i++) {
				GUI.skin = lastSkin;
				GUI.color = lastColor;
				if (i == selected) { GUI.color = selectionColor; }
				if (showNumbers) {
					Rect b = brush.Left(.5f);
					GUI.Box(b.Left(.4f), ""+i);
					b = b.Right(.3f);
					if (GUI.Button(b.MoveLeft(), "-")) { rem = -1; doSelection = i; }
					if (GUI.Button(b, "D")) { rem = 1; doSelection = i; }
					
					GUI.Box(brush.Right(.5f), names[i]);
				} else {
					GUI.Box(brush, names[i]);
				}
				
				GUI.skin = blankSkin;
				if (GUI.Button(brush, "")) { doSelection = i; }
				
				
				brush.y += brush.height;
			}
		GUI.EndScrollView();
		
		GUI.skin = lastSkin;
		GUI.color = lastColor;
		
		if (doSelection >= 0) { selected = doSelection; }
		return new Vector4(pos.x, pos.y, selected, rem);
	}
	
	
	public static SelectableControl GetSelection() { 
		if (controls.Count == 0) { return null; }
		if (currentSelectedControl <= -1 || currentSelectedControl >= controls.Count) { return null; }
		return controls[currentSelectedControl];
	}
	
	public static string GetSelectedName() { 
		SelectableControl sel = GetSelection();
		if (sel == null) { return ""; }
		return GetSelection().name;
	}
	
	public static Rect GetSelectedArea() { 
		SelectableControl sel = GetSelection();
		if (sel == null) { return new Rect(0, 0, 0, 0); }
		return GetSelection().area; 
	}
	
	public static string LastControlName() { return "Control" + (controls.Count); }
	public static string NextControlName() { return "Control" + (controls.Count+1); }
	public static string CreateNextControl(Rect area) { return CreateNextControl(area, NextControlName()); }
	public static string CreateNextControl(Rect area, string name) {
		AddControl(new SelectableControl(area, name));
		GUI.SetNextControlName(name);
		return name;
	}
	
	public static bool SButton(Rect area, string str, float padding = defaultPadding, string selectControl = "Jump") { return SButton(area, new GUIContent(str), padding, selectControl); }
	public static bool SButton(Rect area, GUIContent c, float padding = defaultPadding, string selectControl = "Jump") {
		string name = CreateNextControl(area);
		// NOTE: If Unity ever fixes the issue with Space bar pressing currently selected button, REMOVE THE INVERTED INPUT.GETKEY HERE! This is a crappy hacky workaround that only works 95% of the time, and disables clicking while space is held!
		bool ret = (Button(area, c, padding) && !Input.GetKey(KeyCode.Space)) || (InputWrapper.GUIGetButtonDown(selectControl) && GUI.GetNameOfFocusedControl() == name);
		//if (ret) { FocusLastControl(); }
		return ret;
	}
	
	[System.Obsolete("Use SButton instead.")]
	public static bool SelectableButton(Rect area, string str, string name) {
		return SelectableButton(area, str, name, defaultPadding);
	}
	
	[System.Obsolete("Use SButton instead.")]
	public static bool SelectableButton(Rect area, string str, string name, float padding) {
		//string nn = NextControlName();
		AddControl(new SelectableControl(area, name));
		GUI.SetNextControlName(name);
		return Button(area, str, padding);
	}
	
	public static float SelectableHorizontalSlider(Rect area, float value, float leftValue, float rightValue) {
		return SelectableHorizontalSlider(area, value, leftValue, rightValue, NextControlName());
	}
	public static float SelectableHorizontalSlider(Rect area, float value, float leftValue, float rightValue, string name) {
		AddControl(new SelectableControl(area, name));
		GUI.SetNextControlName(name);
		return GUI.HorizontalSlider(area, value, leftValue, rightValue);
	}
	
	public static float SelectableVerticalSlider(Rect area, float value, float leftValue, float rightValue) {
		return SelectableVerticalSlider(area, value, leftValue, rightValue, NextControlName());
	}
	public static float SelectableVerticalSlider(Rect area, float value, float topValue, float bottomValue, string name) {
		AddControl(new SelectableControl(area, name));
		GUI.SetNextControlName(name);
		return GUI.VerticalSlider(area, value, topValue, bottomValue);
	}
	
	public static bool SelectableToggle(Rect area, bool value, string text) {
		return SelectableToggle(area, value, text, NextControlName());
	}
	
	public static bool SelectableToggle(Rect area, bool value, string text, string name) {
		AddControl(new SelectableControl(area, name));
		GUI.SetNextControlName(name);
		return GUI.Toggle(area, value, text);
	}
	
	
	public static void AddControl(SelectableControl c) { controls.Add(c); }
	
	public static void ResetControls() {
		controls = new List<SelectableControl>();
	}
	
	public static string Raycast(Vector2 start, Vector2 direction) {
		if (controls.Count == 0) { return ""; }
		Vector2 currentPoint = start;
		Rect screen = new Rect(0, 0, Screen.width, Screen.height);
		
		while (screen.Contains(currentPoint)) {
			for (int i = 0; i < controls.Count; i++) {
				if (i != currentSelectedControl && controls[i].area.Contains(currentPoint)) {
					FocusControl(i);
					return controls[i].name;
				}
			}
			currentPoint += direction.normalized;
		}
		return "";
	}
	
	public static Vector2 ScrollableVerticalSelection(Rect area, Rect buttonSize, Rect repeat, Vector2 scroll, out int selection, string[] names) {
		selection = -1;
		Rect viewArea = buttonSize ;
		viewArea.height *= names.Length;
		Rect brush = buttonSize;
		brush.x = area.x;
		brush.y = area.y;
		
		scroll = GUI.BeginScrollView(area, scroll, viewArea);
			for (int i = 0; i < names.Length; i++) {
				if (GUI.Button(brush, names[i])) {
					selection = i;
				}
				brush.y += brush.height;
			}
			
		GUI.EndScrollView();
		
		return scroll;
	}
	
	
	public static string Up() {
		if(currentSelectedControl<0) {
			FocusControl(0);
			return controls[0].name;
		}
		return Raycast(GetSelectedArea().UpperCenter(), new Vector2(0, -1) );
	}
	public static string Down() {
		if(currentSelectedControl<0) {
			FocusControl(0);
			return controls[0].name;
		}
		return Raycast(GetSelectedArea().BottomCenter(), new Vector2(0, 1) );
	}
	
	public static string Left() {
		if(currentSelectedControl<0) {
			FocusControl(0);
			return controls[0].name;
		}
		return Raycast(GetSelectedArea().MiddleLeft(), new Vector2(-1, 0) );
	}
	public static string Right() {
		if(currentSelectedControl<0) {
			FocusControl(0);
			return controls[0].name;
		}
		return Raycast(GetSelectedArea().MiddleRight(), new Vector2(1, 0) );
	}
	
	public static void FocusControl(int index) {
		currentSelectedControl = index;
		if (GetSelection() == null) { GUI.FocusControl(""); return; }
		GUI.FocusControl(GetSelection().name);
	}
	
	public static void DefaultFocus(bool focus) { DefaultFocus(focus, 0); }
	public static void DefaultFocus(bool focus, int index) {
		if (focus) { FocusControl(index); }
		else { FocusControl(-1); }
	}
	
	public static void FocusLastControl() {
		FocusControl(-1 + int.Parse(LastControlName().Replace("Control", "")));
	}
	
	
	public static bool LastControlIsFocused() {
		return (GUI.GetNameOfFocusedControl() == LastControlName());
	}
	
	public static void UnfocusIfEmpty() {
		if (controls.Count > 0 && currentSelectedControl >= 0 && currentSelectedControl < controls.Count) {
			FocusControl(currentSelectedControl);
		} else {
			GUI.FocusControl("");
		}
		
		
	}
	
	public class SelectableControl {
		public string name;
		public Rect area;
		
		public SelectableControl(Rect area, string name) {
			this.name = name;
			this.area = area;
		}
	}
	
	///////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////
	//Matrix shit
	
	public static void ResetProjection() {
		GUI.matrix = Matrix4x4.identity;
		
	}
	
	//sets the GUI matrix to map normal screen coords to the normalized target area, at the target rotation
	public static void Rotate90Left() {
		GUI.matrix = Matrix4x4.identity;
		
		Vector2 center = new Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(-90, center);
		Vector3 offset = Vector3.zero;
		offset.x = (Screen.width - Screen.height) / 2;
		offset.y = -(Screen.width - Screen.height) / 2;
		GUI.matrix *= Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
	}
	
	public static void Rotate90Left(Vector3 offset) {
		GUI.matrix = Matrix4x4.identity;
		
		Vector2 center = new Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(-90, center);
		GUI.matrix *= Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
		//GUIUtility.ScaleAroundPivot(new Vector2(0, 0), center);
	}
	
	public static void Rotate90Right() {
		GUI.matrix = Matrix4x4.identity;
		
		Vector2 center = new Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(90, center);
		Vector3 offset = Vector3.zero;
		offset.x = (Screen.width - Screen.height) / 2;
		offset.y = -(Screen.width - Screen.height) / 2;
		GUI.matrix *= Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
		
	}
	
	public static void Rotate90Right(Vector3 offset) {
		GUI.matrix = Matrix4x4.identity;
		
		Vector2 center = new Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(90, center);
		GUI.matrix *= Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
		//GUIUtility.ScaleAroundPivot(new Vector2(0, 0), center);
	}
	
	
	///////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////
	//WORK IN PROGRESS
	public class Layout {
		public List<GUIElement> elements;
		
		public void Draw(Component caller) {
			foreach (GUIElement element in elements) { element.Draw(caller); }
		}
		
		
	}
	
	public abstract class GUIElement {
		public string name;
		public GUIContent content;
		public string function;
		public Rect area;
		
		public virtual void Draw(Component caller) {}
		
		public void Call(Component caller) { caller.SendMessage(function, SendMessageOptions.DontRequireReceiver); }
		public void Call(Transform target) { target.SendMessage(function, SendMessageOptions.DontRequireReceiver); }
		public void Call(GameObject target) { target.SendMessage(function, SendMessageOptions.DontRequireReceiver); }
		
	}
	
	public class GUILabel : GUIElement {
		public new void Draw(Component caller) { GUIF.Label(area, content); }
	}
	
	public class GUIBox : GUIElement {
		public new void Draw(Component caller) { GUIF.Box(area, content); }
	}
	
	public class GUIButton : GUIElement {
		private bool clicked = false;
		public bool wasClicked { get { return clicked; } }
		
		public new void Draw(Component caller) {
			clicked = false;
			if (GUIF.Button(area, content)) { clicked = true; Call(caller); }
		}
		
	}
	
	public class GUISelectionButton : GUIElement {
		private bool clicked = false;
		public bool wasClicked { get { return clicked; } }
		
		public new void Draw(Component caller) {
			clicked = false;
			if (GUIF.SButton(area, content)) { clicked = true; Call(caller); }
		}
	}
	
	
	public class GUISlider : GUIElement {
		public float value = 0;
		public float minValue = 0;
		public float maxValue = 10;
		private bool changed = false;
		public bool horizontal = true;
		public bool wasChanged { get { return changed; } }
		
		public new void Draw(Component caller) {
			changed = false;
			float prevValue = value;
			if (horizontal) { value = GUI.HorizontalSlider(area, value, minValue, maxValue); }
			else { value = GUI.VerticalSlider(area, value, minValue, maxValue); }
			
			if (value != prevValue) { changed = true; ZScript.SetValue(name, value); }
		}
		
	}

		
	
	
}




