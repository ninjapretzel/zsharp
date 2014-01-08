using UnityEngine;
using System.Collections;

public static class FloatF {

	public static float TLerp(this float f, float target) { return f.TLerp(target, 1); }
	public static float TLerp(this float f, float target, float v) { return f.Lerp(target, Time.deltaTime * v); }
	
	public static float Lerp(this float f, float target, float v) { return Mathf.Lerp(f, target, v); }
	public static float Clamp(this float f, float min, float max) { return Mathf.Clamp(f, min, max); }
	public static float Clamp01(this float f) { return Mathf.Clamp01(f); }
	public static float Floor(this float f) { return Mathf.Floor(f); }
	public static float Ceil(this float f) { return Mathf.Ceil(f); }
	public static float Round(this float f) { return Mathf.Round(f); }
	public static float RoundDown(this float f) { return f.Floor(); }
	public static float RoundUp(this float f) { return f.Ceil(); }
	public static float Fract(this float f) { return f - f.Floor(); }
	
	public static bool IsNAN(this float f) { return float.IsNaN(f); }
	public static bool IsNaN(this float f) { return float.IsNaN(f); }
	
	
	public static float Nearest(this float f, float v) { return f.Nearest(v, 0); }
	public static float Nearest(this float f, float v, float offset) {
		float d = (f + offset) / v;
		return Mathf.Round(d) * v;
	}
	
	public static float Sign(this float f) { return Mathf.Sign(f); }
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
	