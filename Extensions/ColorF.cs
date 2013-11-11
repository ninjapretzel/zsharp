using UnityEngine;
using System.Collections;

public static class ColorF {

	public static Color MultRGB(this Color c, float f) { return new Color(c.r * f, c.g * f, c.b * f, c.a); }
	public static Color Half(this Color c) { return c.MultRGB(.5f); }
	
	public static Color CosAlpha(this Color c, float change) { return c.CosAlpha(change, 1); }
	public static Color CosAlpha(this Color c, float change, float timescale) {
		Color col = c;
		float pos = Mathf.Cos(Time.time * timescale);
		col.a += pos * change;
		return col;
	}
	
}
