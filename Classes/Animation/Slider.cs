using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Slider {
	public Rect baseArea;
	
	public Rect IN { get { return slideIn; } }
	public Rect In { get { return slideIn; } }
	public Rect ON { get { return slideIn; } }
	public Rect On { get { return slideIn; } }
	
	public Rect OUT { get { return slideOut; } }
	public Rect Out { get { return slideOut; } }
	public Rect OFF { get { return slideOut; } }
	public Rect Off { get { return slideOut; } }
	
	public Rect slideIn;
	public Rect slideOn { get { return slideIn; } }
	
	public Rect slideOut;
	public Rect slideOff { get { return slideOut; } }
	
	public float dampening = 5;
	
	public bool done { get { return slideChange.magnitude < .04f; } }
	public Vector2 slideChange = Vector2.zero;
	
	public Slider(Rect area) { baseArea = area; }
	public static Slider Normalized(Rect area) {
		Rect a = new Rect(area.x * Screen.width, area.y * Screen.height, area.width * Screen.width, area.height * Screen.height);
		
		return new Slider(a);
	}
	
	public static Slider Up(Rect area) { Slider s = Normalized(area); s.SlideUp(); return s; }
	public static Slider Down(Rect area) { Slider s = Normalized(area); s.SlideDown(); return s; }
	public static Slider Left(Rect area) { Slider s = Normalized(area); s.SlideLeft(); return s; }
	public static Slider Right(Rect area) { Slider s = Normalized(area); s.SlideRight(); return s; }
	
	
	public void Update() { Update(Time.deltaTime); }
	public void FixedUpdate() { Update(.02f); }
	public void Update(float t) {
		float time = t;
		if (time > .1) { time = .1f; }
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
