using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class ScreenF {
	public static Vector2 size { get { return new Vector2(width, height); } }
	public static Vector2 rot90size { get { return new Vector2(height, width); } }
	
	
	public static float width { get { return Screen.width; } }
	public static float height { get { return Screen.height; } }
	public static float w { get { return width; } }
	public static float h { get { return height; } } 
	
	public static float aspect { get { return width/height; } }
	public static Rect all { get { return new Rect(0, 0, w, h); } }
	
	public static Vector2 TopLeft() { return all.TopLeft(); }
	public static Vector2 TopCenter() { return all.TopCenter(); }
	public static Vector2 TopRight() { return all.TopRight(); }
	public static Vector2 UpperLeft() { return all.UpperLeft(); }
	public static Vector2 UpperCenter() { return all.UpperCenter(); }
	public static Vector2 UpperRight() { return all.UpperRight(); }
	public static Vector2 MiddleLeft() { return all.MiddleLeft(); }
	public static Vector2 MiddleCenter() { return all.MiddleCenter(); }
	public static Vector2 MiddleRight() { return all.MiddleRight(); }
	public static Vector2 BottomLeft() { return all.BottomLeft(); }
	public static Vector2 BottomCenter() { return all.BottomCenter(); }
	public static Vector2 BottomRight() { return all.BottomRight(); }
	public static Vector2 LowerLeft() { return all.LowerLeft(); }
	public static Vector2 LowerCenter() { return all.LowerCenter(); }
	public static Vector2 LowerRight() { return all.LowerRight(); }
	
	public static Rect TopLeft(float x, float y) { return all.TopLeft(x, y); }
	public static Rect TopCenter(float x, float y) { return all.TopCenter(x, y); }
	public static Rect TopRight(float x, float y) { return all.TopRight(x, y); }
	public static Rect UpperLeft(float x, float y) { return all.UpperLeft(x, y); }
	public static Rect UpperCenter(float x, float y) { return all.UpperCenter(x, y); }
	public static Rect UpperRight(float x, float y) { return all.UpperRight(x, y); }
	public static Rect MiddleLeft(float x, float y) { return all.MiddleLeft(x, y); }
	public static Rect MiddleCenter(float x, float y) { return all.MiddleCenter(x, y); }
	public static Rect MiddleRight(float x, float y) { return all.MiddleRight(x, y); }
	public static Rect BottomLeft(float x, float y) { return all.BottomLeft(x, y); }
	public static Rect BottomCenter(float x, float y) { return all.BottomCenter(x, y); }
	public static Rect BottomRight(float x, float y) { return all.BottomRight(x, y); }
	public static Rect LowerLeft(float x, float y) { return all.LowerLeft(x, y); }
	public static Rect LowerCenter(float x, float y) { return all.LowerCenter(x, y); }
	public static Rect LowerRight(float x, float y) { return all.LowerRight(x, y); }
	
	public static Rect TopLeft(Vector2 v) { return all.TopLeft(v); }
	public static Rect TopCenter(Vector2 v) { return all.TopCenter(v); }
	public static Rect TopRight(Vector2 v) { return all.TopRight(v); }
	public static Rect UpperLeft(Vector2 v) { return all.UpperLeft(v); }
	public static Rect UpperCenter(Vector2 v) { return all.UpperCenter(v); }
	public static Rect UpperRight(Vector2 v) { return all.UpperRight(v); }
	public static Rect MiddleLeft(Vector2 v) { return all.MiddleLeft(v); }
	public static Rect MiddleCenter(Vector2 v) { return all.MiddleCenter(v); }
	public static Rect MiddleRight(Vector2 v) { return all.MiddleRight(v); }
	public static Rect BottomLeft(Vector2 v) { return all.BottomLeft(v); }
	public static Rect BottomCenter(Vector2 v) { return all.BottomCenter(v); }
	public static Rect BottomRight(Vector2 v) { return all.BottomRight(v); }
	public static Rect LowerLeft(Vector2 v) { return all.LowerLeft(v); }
	public static Rect LowerCenter(Vector2 v) { return all.LowerCenter(v); }
	public static Rect LowerRight(Vector2 v) { return all.LowerRight(v); }
	
	public static Rect Top(float f) { return all.Top(f); }
	public static Rect Upper(float f) { return all.Upper(f); }
	public static Rect Middle(float f) { return all.Middle(f); }
	public static Rect Lower(float f) { return all.Lower(f); }
	public static Rect Bottom(float f) { return all.Bottom(f); }
	public static Rect Left(float f) { return all.Left(f); }
	public static Rect Center(float f) { return all.Center(f); }
	public static Rect Right(float f) { return all.Right(f); }
	
}