using UnityEngine;
using System.Collections;

public class SimulatedButton : MonoBehaviour {
	public Vector2 position;
	public float size = 0.225f;
	
	public bool hidden = false;
	
	public bool tapped = false;
	public bool doubleTapped = false;
	public bool released = false;
	public bool held = false;
	public float hitTime = 0;
	public float releaseTime = 0;
	public float doubleTapTime = .2f;
	
	public Color color = Color.white;
	public Color hitColor = Color.red;
	public Texture2D graphic;
	
	public bool frameAlready = false;
	
	private	Vector2 pixelPosition;
	private Vector2 pixelCenter;
	private float pixelSize = 0.0f;
	
	void LateUpdate() {
		frameAlready = false;
		hitTime += Time.deltaTime;
		releaseTime += Time.deltaTime;
	}
	
	void OnGUI() {
		if (hidden) { return; }
		pixelSize = Screen.height * size;
		pixelCenter = new Vector2(position.x * Screen.width, position.y * Screen.height);
		pixelPosition = new Vector2(position.x * Screen.width - pixelSize/2, position.y * Screen.height - pixelSize/2);
		
		Rect brush = new Rect(pixelPosition.x, pixelPosition.y, pixelSize, pixelSize);
		
		if (frameAlready) {
			GUI.color = color;
			if (held) { GUI.color = hitColor; }
			GUI.DrawTexture(brush, graphic);
		} else {
			bool wasHeld = held;
			doubleTapped = false;
			tapped = false;
			held = false;
			released = false;
			GUI.color = color;
			bool hadGoodTouch = false;
			
			foreach (Touch t in Input.touches) {
				Vector2 realTouchPosition = t.position;
				realTouchPosition.y = Screen.height - t.position.y;
				Vector2 difference = pixelCenter - realTouchPosition;
				
				if (difference.magnitude < pixelSize / 2.0f) {
					hadGoodTouch = true;
					if (t.phase == TouchPhase.Canceled) { continue; }
					if (t.phase == TouchPhase.Began) {
						if (hitTime < doubleTapTime) { doubleTapped = true; }
						tapped = true;
						hitTime = 0;
					} else if ((t.phase == TouchPhase.Stationary) || (t.phase == TouchPhase.Moved)) {
						held = true;
						GUI.color = hitColor;
					} else if (t.phase == TouchPhase.Ended) {
						released = true;
						releaseTime = 0;
					}
					
					if (wasHeld && !held) { released = true; }
					
					break;
				}
				
			}
			
			GUI.DrawTexture(brush, graphic);
			frameAlready = true;
			
		}
	}
}






















