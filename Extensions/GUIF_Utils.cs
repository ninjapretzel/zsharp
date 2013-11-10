using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class GUIStyleF {
	public static GUIStyle Clone(this GUIStyle c) { return new GUIStyle(c); }

	public static GUIStyle ScaledFontTo(this GUIStyle style, float p) {
		GUIStyle copy = new GUIStyle(style);
		copy.fontSize = (int)(p * (float)Screen.height);
		return copy;
	}
	
	public static GUIStyle Aligned(this GUIStyle style, TextAnchor a) {
		GUIStyle copy = new GUIStyle(style);
		copy.alignment = a;
		return copy;
	}
	
	public static GUIStyle LeftAligned(this GUIStyle style) {
		GUIStyle copy = new GUIStyle(style);
		TextAnchor a = style.alignment;
		int b = (int)a / 3;
		copy.alignment = (TextAnchor)(b * 3);
		return copy;
	}
	
	public static GUIStyle CenterAligned(this GUIStyle style) {
		GUIStyle copy = new GUIStyle(style);
		TextAnchor a = style.alignment;
		int b = (int)a / 3;
		copy.alignment = (TextAnchor)(b * 3 + 1);
		return copy;
	}	
	
	public static GUIStyle RightAligned(this GUIStyle style) {
		GUIStyle copy = new GUIStyle(style);
		TextAnchor a = style.alignment;
		int b = (int)a / 3;
		copy.alignment = (TextAnchor)(b * 3 + 2);
		return copy;
	}
	
	public static GUIStyle UpperAligned(this GUIStyle style) {
		GUIStyle copy = new GUIStyle(style);
		TextAnchor a = style.alignment;
		int b = (int)a % 3;
		copy.alignment = (TextAnchor)b;
		return copy;
	}
	
	public static GUIStyle MiddleAligned(this GUIStyle style) {
		GUIStyle copy = new GUIStyle(style);
		TextAnchor a = style.alignment;
		int b = (int)a % 3;
		copy.alignment = (TextAnchor)3 + b;
		return copy;
	}
	
	public static GUIStyle LowerAligned(this GUIStyle style) {
		GUIStyle copy = new GUIStyle(style);
		TextAnchor a = style.alignment;
		int b = (int)a % 3;
		copy.alignment = (TextAnchor)6 + b;
		return copy;
	}
	
	
}

public static class GUISkinF {
	
	public static void FontSize(this GUISkin skin, float size) { skin.ScaleFontTo(size/720.0f); }
	public static void ScaleFontTo(this GUISkin skin, float size) {
		skin.label 							= skin.label.ScaledFontTo(size);
		skin.button 						= skin.button.ScaledFontTo(size);
		skin.box 							= skin.box.ScaledFontTo(size);
		skin.textField 						= skin.textField.ScaledFontTo(size);
		skin.textArea						= skin.textArea.ScaledFontTo(size);
		skin.toggle 						= skin.toggle.ScaledFontTo(size);
		skin.window 						= skin.window.ScaledFontTo(size);
		skin.horizontalSlider 				= skin.horizontalSlider.ScaledFontTo(size);
		skin.horizontalSliderThumb 			= skin.horizontalSliderThumb.ScaledFontTo(size);
		skin.verticalSlider 				= skin.verticalSlider.ScaledFontTo(size);
		skin.verticalSliderThumb 			= skin.verticalSliderThumb.ScaledFontTo(size);
		skin.horizontalScrollbar 			= skin.horizontalScrollbar.ScaledFontTo(size);
		skin.horizontalScrollbarThumb 		= skin.horizontalScrollbarThumb.ScaledFontTo(size);
		skin.horizontalScrollbarLeftButton 	= skin.horizontalScrollbarLeftButton.ScaledFontTo(size);
		skin.horizontalScrollbarRightButton = skin.horizontalScrollbarRightButton.ScaledFontTo(size);
		skin.verticalScrollbar 				= skin.verticalScrollbar.ScaledFontTo(size);
		skin.verticalScrollbarThumb 		= skin.verticalScrollbarThumb.ScaledFontTo(size);
		skin.verticalScrollbarUpButton 		= skin.verticalScrollbarUpButton.ScaledFontTo(size);
		skin.verticalScrollbarDownButton 	= skin.verticalScrollbarDownButton.ScaledFontTo(size);
		skin.scrollView 					= skin.scrollView.ScaledFontTo(size);
		
		if (skin.customStyles != null) {
			for (int i = 0; i < skin.customStyles.Length; i++) {
				skin.customStyles[i] = skin.customStyles[i].ScaledFontTo(size);
			}
		}
		
	}
	
	public static GUISettings Clone(this GUISettings sets) {
		GUISettings clone = new GUISettings();
		clone.doubleClickSelectsWord = sets.doubleClickSelectsWord;
		clone.tripleClickSelectsLine = sets.tripleClickSelectsLine;
		clone.cursorColor = sets.cursorColor;
		clone.cursorFlashSpeed = sets.cursorFlashSpeed;
		clone.selectionColor = sets.selectionColor;
		return clone;
	}
	
	public static void CloneValues(this GUISettings sets, GUISettings other) {
		sets.doubleClickSelectsWord = other.doubleClickSelectsWord;
		sets.tripleClickSelectsLine = other.tripleClickSelectsLine;
		sets.cursorColor = other.cursorColor;
		sets.cursorFlashSpeed = other.cursorFlashSpeed;
		sets.selectionColor = other.selectionColor;
	}
	
}







