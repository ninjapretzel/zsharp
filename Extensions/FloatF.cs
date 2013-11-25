using UnityEngine;
using System.Collections;

public static class FloatF {
	public static float Floor(this float f) { return Mathf.Floor(f); }
	public static float Ceil(this float f) { return Mathf.Ceil(f); }
	public static float Round(this float f) { return Mathf.Round(f); }
	public static float RoundDown(this float f) { return f.Floor(); }
	public static float RoundUp(this float f) { return f.Ceil(); }
	public static float Fract(this float f) { return f - f.Floor(); }
	
	public static float Abs(this float f) { return Mathf.Abs(f); }
	
	public static float Outside(this float val, float a, float b) {
		float min = Mathf.Min(a, b);
		float max = Mathf.Max(a, b);
		if (val > min && val < max) { return 0; }
		if (val > max) { return val - max; }
		return val - min;
	}
	
	public static float CosInterp(float a, float b, float x) {
		float ft = x * 3.1415927f;
		float f = (1 - Mathf.Cos(ft)) * .5f;
		return  a * (1-f) + b * f;
	}
	
}
	