using UnityEngine;
using System.Collections;

public static class Bars {
	public static Texture2D graphic;
	public static Texture2D vertical;
	
	public static void Draw(Rect area, float pp, Color tint, Color back, int padding) {
		Rect brush = area.Trim(-padding);
		float p = Mathf.Clamp01(pp);
		Texture2D g = GetGraphic(area);
		
		GUI.color = back;
		GUI.DrawTexture(brush, g);
		
		brush = brush.Trim(padding);
		if (area.width > area.height) { brush.width *= p; }
		else { 
			brush.y += area.height * (1.0f - p);
			brush.height *= p;
		}
		GUI.color = tint;
		GUI.DrawTexture(brush, g);
	}
	
	public static void DrawFixed(Rect area, float pp, Color tint, Color back, int padding) {
		Rect brush = area.Trim(-padding);
		float p = Mathf.Clamp01(pp);
		Texture2D g = GetGraphic(area);
		
		GUI.color = back;
		GUI.DrawTexture(brush, g);
		
		
		brush = brush.Trim(padding);
		GUI.color = tint;
		GUI.DrawTexture(brush, g);
		
		if (area.width > area.height) {
			brush.x += area.width * p;
			brush.width *= p;
		} else { brush.height *= (1.0f-p); }
		GUI.color = back;
		GUI.DrawTexture(brush, g);
	}
	
	public static void Draw(Rect area, Rect repeat, float pp, Color tint, Color back, int padding) {
		Rect brush = area.Pad(padding);
		float p = Mathf.Clamp01(pp);
		Texture2D g = GetGraphic(area);
		
		GUI.color = back;
		GUI.DrawTextureWithTexCoords(brush, g, repeat);
		brush = brush.Trim(padding);
		
		Rect filled = brush;
		Rect filledReps = repeat;
		Rect empty = brush;
		Rect emptyReps = repeat;
		
		if (area.width > area.height) {
			filled = filled.UpperLeft(p, 1);
			filledReps = filledReps.UpperLeft(p, 1);
			empty = empty.UpperRight(1.0f-p, 1);
			emptyReps = emptyReps.UpperRight(1.0f-p, 1);
		} else {
			filled = filled.BottomLeft(1, p);
			filledReps = filledReps.UpperLeft(1, p);
			empty = empty.UpperLeft(1, 1.0f-p);
			emptyReps = emptyReps.BottomLeft(1, 1.0f-p);
		}
		
		GUI.color = tint;
		GUI.DrawTextureWithTexCoords(filled, g, filledReps);
		GUI.color = back;
		GUI.DrawTextureWithTexCoords(empty, g, emptyReps);
		
		
		
	}
	
	public static Texture2D GetGraphic(Rect area) {
		if (area.width > area.height || vertical == null) { return graphic; }
		return vertical;
	}
	
	
}
