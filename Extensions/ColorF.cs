using UnityEngine;
using System.Collections;

public static class ColorF {
	
	public static Color CosAlpha(this Color c, float change, float timescale) {
		Color col = c;
		float pos = Mathf.Cos(Time.time * timescale);
		col.a += pos * change;
		return col;
	}
	
}
