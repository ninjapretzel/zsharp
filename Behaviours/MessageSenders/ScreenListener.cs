using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Watches for this object moving off of the screen.
//If target is not assigned, it uses Camera.main


public class ScreenListener : MonoBehaviour {
	
	public Camera target;
	Camera cam { get { return target != null ? target : Camera.main; } }
	Vector2 lastScreenPosition;
	Vector2 currentPosition;
	
	bool wasOnScreen {
		get { 
			return RectF.unit.Contains(lastScreenPosition);
		}
	}
	
	bool isOnScreen {
		get {
			return RectF.unit.Contains(currentPosition);
		}
	}
	
	
	
	void Start() {
		lastScreenPosition = transform.GetViewPosition(cam);
		
	}
	
	void Update() {
		currentPosition = transform.GetViewPosition(cam);
		
		string direction = "WHAT";
		if (wasOnScreen && !isOnScreen) {
			if (currentPosition.x < 0) { direction = "Left"; }
			if (currentPosition.x > 1) { direction = "Right"; }
			if (currentPosition.y < 0) { direction = "Top"; }
			if (currentPosition.y > 1) { direction = "Bottom"; }
			Exited(direction);
			
		} else if (!wasOnScreen && isOnScreen) {
			if (lastScreenPosition.x < 0) { direction = "Left"; }
			if (lastScreenPosition.x > 1) { direction = "Right"; }
			if (lastScreenPosition.y < 0) { direction = "Top"; }
			if (lastScreenPosition.y > 1) { direction = "Bottom"; }
			Entered(direction);
			
		}
		
		lastScreenPosition = currentPosition;
		
		
	}
	
	
	void Exited(string direction) {
		transform.Broadcast("OnScreenExit");
		transform.Broadcast("OnScreenExit"+direction);
	}
	
	void Entered(string direction) {
		transform.Broadcast("OnScreenEnter");
		transform.Broadcast("OnScreenEnter"+direction);
	}
	
	
}




















