using UnityEngine;
using System.Collections;

public static class ColorF {
	public static Color Lerp(this Color[] colors, float position) {
		if (colors.Length == 0) { return Color.white; }
		else if (colors.Length == 1) { return colors[0]; }
		
		
		int segments = colors.Length-1;
		int segment = (int)(0f + RandomF.value * ((float)segments));
		float f = (position * segments) % 1f;
		
		return Color.Lerp(colors[segment], colors[segment+1], f);
	}
	
	public static Color white(float f, float a = 1) { return new Color(f, f, f, a); }
	public static Color gray(float f, float a = 1) { return new Color(f, f, f, a); }
	public static Color red(float f, float a = 1) { return new Color(f, 0, 0, a); }
	public static Color green(float f, float a = 1) { return new Color(0, f, 0, a); }
	public static Color blue(float f, float a = 1) { return new Color(0, 0, f, a); }
	public static Color yellow(float f, float a = 1) { return new Color(f, f, 0, a); }
	public static Color purple(float f, float a = 1) { return new Color(f, 0, f, a); }
	public static Color cyan(float f, float a = 1) { return new Color(0, f, f, a); }

	
	public static Color Blend(this Color a, Color b) { return Color.Lerp(a, b, .5f); }
	public static Color Blend(this Color a, Color b, float f) { return Color.Lerp(a, b, f); }
	
	
	public static Color MultRGB(this Color c, float f) { return new Color(c.r * f, c.g * f, c.b * f, c.a); }
	public static Color Half(this Color c) { return c.MultRGB(.5f); }
	
	public static Color CosAlpha(this Color c, float change) { return c.CosAlpha(change, 1); }
	public static Color CosAlpha(this Color c, float change, float timescale) {
		Color col = c;
		float pos = Mathf.Cos(Time.time * timescale);
		col.a += pos * change;
		return col;
	}
	
	public static string ToString(this Color c, char delim) { 
		return "" + c.r + delim + c.g + delim + c.b + delim + c.a;
	}
	
	public static Color FromString(string s) { return FromString(s, ','); }
	public static Color FromString(string s, char delim) {
		string[] strs = s.Split(delim);
		Color c = Color.white;
		if (strs.Length < 3) {
			Debug.LogWarning("Tried to load color from malformed string.\nDelim:" + delim + "\n" + s); 
			return c; 
		}
		c.r = strs[0].ParseFloat();
		c.g = strs[1].ParseFloat();
		c.b = strs[2].ParseFloat();
		if (strs.Length >= 4) { c.a = strs[3].ParseFloat(); }
		return c;
	}
	
	
	
	
}
