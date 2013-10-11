using UnityEngine;
using System.Collections;

public static class FloatF {
	public static float Floor(this float f) { return Mathf.Floor(f); }
	public static float Ceil(this float f) { return Mathf.Ceil(f); }
	public static float Round(this float f) { return Mathf.Round(f); }
	public static float RoundDown(this float f) { return f.Floor(); }
	public static float RoundUp(this float f) { return f.Ceil(); }
	public static float Fract(this float f) { return f - f.Floor(); }
	
}
	