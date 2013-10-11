using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Slider {
	public Rect baseArea;
	public Rect slideIn;
	public Rect slideOut;
	
	public float dampening = 5;
	
	public Vector2 slideChange = Vector2.zero;
	
	public Slider(Rect area) { baseArea = area; }
	
	public void Update() { Update(Time.deltaTime); }
	public void FixedUpdate() { Update(.02f); }
	public void Update(float time) {
		slideIn.x += slideChange.x * time * dampening;
		slideIn.y += slideChange.y * time * dampening;
		slideOut.x += slideChange.x * time * dampening;
		slideOut.y += slideChange.y * time * dampening;
		
		slideChange -= slideChange * time * dampening;
	}
	
	public void Slide(Vector2 v) { Slide(v.x, v.y); }
	public void Slide(float x, float y) {
		slideIn = baseArea.Move(-x, -y);
		slideOut = baseArea;
		slideChange = new Vector2(x * baseArea.width, y * baseArea.height);
	}
	
	public void SlideLeft() { Slide(-1, 0); }
	public void SlideRight() { Slide(1, 0); }
	public void SlideUp() { Slide(0, -1); }
	public void SlideDown() { Slide(0, 1); }
	
	public void Finish() {
		slideIn.x += slideChange.x;
		slideIn.y += slideChange.y;
		slideOut.x += slideChange.x;
		slideOut.y += slideChange.y;
		slideChange = Vector2.zero;
	}
	
}
