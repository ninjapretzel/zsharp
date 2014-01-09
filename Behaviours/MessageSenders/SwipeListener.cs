using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
	Listens for swipe motion in the specified normalized rectangle (To the screen)
	Sends messages in the form "OnDownSwipe" - "OnDownSwipeRelease" - "OnDownSwipePress"
	To use this class, attach it to a game object, and then write a new script with methods that fit the above form.
	
	All messages:
	
	OnDownSwipe
	OnDownSwipePress
	OnDownSwipeRelease
	OnUpSwipe
	OnUpSwipePress
	OnUpSwipeRelease
	OnRightSwipe
	OnRightSwipePress
	OnRightSwipeRelease
	OnLeftSwipe
	OnLeftSwipePress
	OnLeftSwipeRelease

*/

public class SwipeListener : MonoBehaviour {
	
	public Rect area = new Rect(.5f, 0, .5f, 1);
	public float deadZone = .2f;
	public bool testMode = true;
	
	bool sentPress = false;
	bool touched = false;
	Vector2 testSize = new Vector2(50, 50);
	Vector2 startTouchPosition;
	Vector2 currentTouchPosition;
	
	string rightMessage = "OnRightSwipe";
	string leftMessage = "OnLeftSwipe";
	string upMessage = "OnUpSwipe";
	string downMessage = "OnDownSwipe";
	
	float neededDistance { get { return deadZone * Screen.height; } }
	
	void OnGUI() {
		foreach (Touch t in Input.touches) {
			if (area.Denormalized().Contains(t.position)) {
				currentTouchPosition = t.position;
				currentTouchPosition.y = Screen.height - currentTouchPosition.y;
				
				if (touched) { 
					
					if (t.phase.IsRelease()) { 
						sentPress = false;
						touched = false; 
						
					}
					
				} else {
					
					if (t.phase.IsPress()) {
						touched = true;
						sentPress = false;
						startTouchPosition = currentTouchPosition;
						
					}
					
				}
				
				HandleSwipe(t, startTouchPosition, currentTouchPosition);
				if (testMode) {
					GUI.DrawTexture(RectF.Make(currentTouchPosition, testSize), GUIF.pixel);
					GUI.DrawTexture(RectF.Make(startTouchPosition, testSize), GUIF.pixel);
				}
				break;
			}
		}
	}
	
	void HandleSwipe(Touch t, Vector2 start, Vector2 end) {
		Vector2 diff = start.BiggestDifferenceTo(end);
		GUI.color = Color.white;
		if (diff.magnitude > neededDistance) {
			string message = "";
			GUI.color = Color.red;
			if (diff.x > 0) { message = rightMessage; }
			if (diff.x < 0) { message = leftMessage; }
			if (diff.y > 0) { message = downMessage; }
			if (diff.y < 0) { message = upMessage; }
			if (t.phase.IsRelease()) { message += "Release"; }
			if (!sentPress) { message += "Press"; sentPress = true; }
			transform.SendMessage(message, SendMessageOptions.DontRequireReceiver);
			
		}
	}
	
}




















