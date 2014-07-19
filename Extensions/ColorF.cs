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
	
	public static Color purple() { return new Color(1, 0, 1, 1); }
	public static Color cyan() { return new Color(0, 1, 1, 1);  }
	
	public static Color white(float f) { return white(f, 1); }
	public static Color gray(float f) { return gray(f, 1); }
	public static Color red(float f) { return red(f, 1); }
	public static Color green(float f) { return green(f, 1); }
	public static Color blue(float f) { return blue(f, 1); }
	public static Color yellow(float f) { return yellow(f, 1); }
	public static Color purple(float f) { return purple(f, 1); }
	public static Color cyan(float f) { return cyan(f, 1); }
	
	public static Color white(float f, float a) { return new Color(f, f, f, a); }
	public static Color gray(float f, float a) { return new Color(f, f, f, a); }
	public static Color red(float f, float a) { return new Color(f, 0, 0, a); }
	public static Color green(float f, float a) { return new Color(0, f, 0, a); }
	public static Color blue(float f, float a) { return new Color(0, 0, f, a); }
	public static Color yellow(float f, float a) { return new Color(f, f, 0, a); }
	public static Color purple(float f, float a) { return new Color(f, 0, f, a); }
	public static Color cyan(float f, float a) { return new Color(0, f, f, a); }
	
	
	public static Color HSV(float h, float s, float v, float a = 1) { return new Color(h, s, v, a).HSVtoRGB(); }
	public static Color RandomHue(float s, float v, float a = 1) { return new Color(RandomF.Range(0, 1), s, v, a).HSVtoRGB(); }
	
	public static Color HSVLerp(Color a, Color b, float val) {
		Color ahsv = a.RGBtoHSV();
		Color bhsv = b.RGBtoHSV();
		return Color.Lerp(ahsv, bhsv, val).HSVtoRGB();
	}
	
	public static Color ShiftHue(this Color c, float shift) {
		Color hsv = c.RGBtoHSV();
		hsv.r = (hsv.r + shift) % 1f;
		return hsv.HSVtoRGB();
	}
	
	public static Color Saturate(this Color c, float saturation) {
		Color hsv = c.RGBtoHSV();
		hsv.g = Mathf.Clamp01(hsv.g + saturation);
		return hsv.HSVtoRGB();
	}
	
	//HSV colors are represented as
	//R = H (0...1) for red corrosponds to (0...360) for hue
	//G = S (0...1) for green corrosponds to (0...1) for saturation
	//B = V (0...1) for blue corrosponds to (0...1) for value
	//alpha channel value is maintained.
	public static Color RGBtoHSV(this Color c) {
		Color hsv = new Color(0, 0, 0, c.a);
		
		float max = Mathf.Max(c.r, c.g, c.b);
		if (max <= 0) { return hsv; }
		//Value
		hsv.b = max;
		
		float r, g, b;
		r = c.r;
		g = c.g;
		b = c.b;
		float min = Mathf.Min(r, g, b);
		float delta = max - min;
		
		//Saturation
		hsv.g = delta/max;
		
		//Hue
		float h;
		if (r == max) {
			h = (g - b) / delta;
		} else if (g == max) {
			h = 2 + (b - r) / delta;
		} else {
			h = 4 + (r - g) / delta;
		}
		
		h /= 6f; // convert h (0...6) space to (0...1) space
		if (h < 0) { h += 1; }
		
		hsv.r = h;
		
		
		return hsv;
	}
	
	public static Color HSVtoRGB(this Color c) {
		int i;
		
		float a = c.a;
		float h, s, v;
		float f, p, q, t;
		h = c.r;
		s = c.g;
		v = c.b;
		
		if (s == 0) {
			return new Color(v, v, v, a);
		}
		
		//convert h from (0...1) space to (0...6) space
		h *= 6f;
		i = (int)Mathf.Floor(h);
		f = h - i;
		p = v * (1 - s);
		q = v * (1 - s * f);
		t = v * (1 - s * (1 - f) );
		
		if (i == 0) {
			return new Color(v, t, p, a);
		} else if (i == 1) {
			return new Color(q, v, p, a);
		} else if (i == 2) {
			return new Color(p, v, t, a);
		} else if (i == 3) {
			return new Color(p, q, v, a);
		} else if (i == 4) {
			return new Color(t, p, v, a);
		} 
		
		return new Color(v, p, q, a);
		
	}
	
	
	
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
