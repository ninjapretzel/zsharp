using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to represent a bar on the screen at some absolute position.
//Mostly obsolete, but useful for testing.
[System.Serializable]
public class Bar {
	
	public enum Mode { Normal, Fixed, Repeat, Icons }
	
	public Mode mode = Mode.Normal;
	public Texture2D pixel { get { return Resources.Load<Texture2D>("pixel"); } }
	
	public Texture2D frontGraphic;
	public Texture2D backGraphic;
	
	public Color frontColor = Color.green;
	public Color backColor = Color.black;
	
	public Rect area;
	public Rect repeat;
	
	//Padding for 'back'
	public float padding;
	
	#region CONSTRUCTORS
	public Bar() {
		Init();
	}
	
	
	public Bar(Rect a, float pad = 2f) {
		Init();
		area = a;
		padding = pad;
	}
	
	public Bar(Texture2D front, Texture2D back, float pad = 2f) {
		frontGraphic = front;
		backGraphic = back;
		padding = pad;
	}
	
	public Bar(Texture2D front, Texture2D back, Color frontC, float pad = 2f) {
		frontGraphic = front;
		backGraphic = back;
		frontColor = frontC;
		padding = pad;
	}
	
	public Bar(Texture2D front, Texture2D back, Color frontC, Color backC, float pad = 2f) {
		frontGraphic = front;
		backGraphic = back;
		frontColor = frontC;
		backColor = backC;
		padding = pad;
	}
	
	public Bar(Rect a, Texture2D front, Texture2D back, float pad = 2f) {
		area = a;
		frontGraphic = front;
		backGraphic = back;
		padding = pad;
	}
	
	public Bar(Rect a, Texture2D front, Texture2D back, Color frontC, float pad = 2f) {
		area = a;
		frontGraphic = front;
		backGraphic = back;
		frontColor = frontC;
		padding = pad;
	}
	
	
	public Bar(Rect a, Texture2D front, Texture2D back, Color frontC, Color backC, float pad = 2f) {
		area = a;
		frontGraphic = front;
		backGraphic = back;
		frontColor = frontC;
		backColor = backC;
		padding = pad;
	}
	
	void Init() {
		area = ScreenF.all.Bottom(.1f);
		repeat = new Rect(0, 0, 1, 1);
		frontGraphic = pixel;
		backGraphic = pixel;
	}
	
	#endregion
	
	
	public void Draw(Rect area, float fill) {
		if (mode == Mode.Normal) { DrawNormal(area, fill); }
		else if (mode == Mode.Fixed) { DrawFixed(area, fill); }
		else if (mode == Mode.Repeat) { DrawRepeat(area, fill); }
	}
	
	public void Draw(float fill) {
		if (mode == Mode.Normal) { DrawNormal(fill); }
		else if (mode == Mode.Fixed) { DrawFixed(fill); }
		else if (mode == Mode.Repeat) { DrawRepeat(fill); }
	}
	
	//Normal draw method.
	//Paints the background, then the foreground over it.
	//Textures are stretched to fit.
	public void DrawNormal(float fill) { DrawNormal(area, fill); }
	public void DrawNormal(Rect area, float fill) {
		Rect brush = area.Pad(padding);
		float p = Mathf.Clamp01(fill);
		
		GUI.color = backColor;
		GUI.DrawTexture(brush, backGraphic);
		
		brush = brush.Trim(padding);
		if (area.width > area.height) { brush.width *= p; }
		else { 
			brush.y += area.height * (1.0f - p);
			brush.height *= p;
		}
		GUI.color = frontColor;
		GUI.DrawTexture(brush, frontGraphic);
	}
	
	//Draws the stuff with 'fixed' texture positions
	//The textures won't move relative to the left edges of the rectangles.
	//Draws the foreground first, then paints the background over it.
	public void DrawFixed(float fill) { DrawFixed(area, fill); }
	public void DrawFixed(Rect area, float fill) {
		Rect brush = area;//.Pad(padding);
		float p = Mathf.Clamp01(fill);
		
		
		GUI.color = frontColor;
		GUI.DrawTexture(brush, frontGraphic);
		
		if (area.width > area.height) {
			brush.x += area.width * p;
			brush.width *= p;
		} else { brush.height *= (1.0f-p); }
		GUI.color = backColor;
		GUI.DrawTexture(brush, backGraphic);
	}
	
	public void DrawRepeat(float fill) { DrawRepeat(area, fill); }
	public void DrawRepeat(Rect area, float fill) {
		Rect brush = area.Pad(padding);
		float p = Mathf.Clamp01(fill);
		
		
		GUI.color = backColor;
		GUI.DrawTextureWithTexCoords(brush, backGraphic, repeat);
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
		
		GUI.color = backColor;
		GUI.DrawTextureWithTexCoords(empty, backGraphic, emptyReps);
		GUI.color = frontColor;
		GUI.DrawTextureWithTexCoords(filled, frontGraphic, filledReps);
	}
	
	public void DrawIcons(float fill) { DrawIcons(area, fill); }
	public void DrawIcons(Rect area, float fill) {
		Bars.graphic = frontGraphic;
		Bars.vertical = frontGraphic;
		Bars.Draw(area, repeat, fill, frontColor, backColor);
	}
	
	
	
}

public static class Bars {
	public static int defaultPadding = 2;
	
	public static Texture2D graphic;
	public static Texture2D vertical;
	
	static Bars() {
		graphic = Resources.Load<Texture2D>("pixel");
		vertical = Resources.Load<Texture2D>("pixel");
		
	}
	
	//General drawing functions
	public static void Draw(Rect area, float pp) { Draw(area, pp, Color.white, Color.black, defaultPadding); }
	public static void Draw(Rect area, float pp, Color tint) { Draw(area, pp, tint, Color.black, defaultPadding); }
	public static void Draw(Rect area, float pp, Color tint, Color back) { Draw(area, pp, tint, back, defaultPadding); }
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
	
	
	//Draws a repeated texture
	public static void Draw(Rect area, Rect repeat, float pp) { Draw(area, repeat, pp, Color.white, Color.black, defaultPadding); }
	public static void Draw(Rect area, Rect repeat, float pp, Color tint) { Draw(area, repeat, pp, tint, Color.black, defaultPadding); }
	public static void Draw(Rect area, Rect repeat, float pp, Color tint, Color back) { Draw(area, repeat, pp, tint, back, defaultPadding); }
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
	
	
	//Draws bars as icons
	public static void Draw(Rect area, Vector2 iconRepeat, float pp) { Draw(area, iconRepeat, pp, Color.white, Color.black); }
	public static void Draw(Rect area, Vector2 iconRepeat, float pp, Color tint) { Draw(area, iconRepeat, pp, tint, Color.black); }
	public static void Draw(Rect area, Vector2 iconRepeat, float pp, Color tint, Color back) {
		float numRows = Mathf.Floor(iconRepeat.y);
		Rect row = new Rect(area.x, area.y, area.width, area.height / numRows);
		Rect repeat = new Rect(0, 0, iconRepeat.x, 1);
		
		
		for (int i = (int)numRows-1; i >= 0; i--) {
			float p = (pp*numRows) - i;
			Draw(row, repeat, p, tint, back, 0);
			
			row = row.MoveDown();
			
			
		}
		
	}
	
	
	
	public static Texture2D GetGraphic(Rect area) {
		if (area.width > area.height || vertical == null) { return graphic; }
		return vertical;
	}
	
	
}
