using UnityEngine;
using System.Collections;

public class SimulatedButton : MonoBehaviour {
	public Vector2 position;
	public Vector2 pixelPosition;
	public Vector2 pixelCenter;
	
	public float size = 0.225f;
	public float pixelSize = 0.0f;
	
	public bool tapped  = false;
	public bool wasTapped = false;
	public bool held = false;
	public float hitTime = 0;
	
	public Color color = Color.white;
	public Color hitColor = Color.red;
	
	public Texture2D graphic;
	
	public bool frameAlready = false;
	
	public bool hidden = false;
	
	void LateUpdate() {
		frameAlready = false;
		hitTime += Time.deltaTime;
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
			tapped = false;
			held = false;
			
			GUI.color = color;
			
			foreach (Touch t in Input.touches) {
				Vector2 realTouchPosition = t.position;
				realTouchPosition.y = Screen.height - t.position.y;
				Vector2 difference = pixelCenter - realTouchPosition;
				
				if (difference.magnitude < pixelSize / 2.0f) {
					if (t.phase == TouchPhase.Canceled) { continue; }
					if (t.phase == TouchPhase.Began) {
						tapped = true;
						wasTapped = true;
						hitTime = 0;
					} else if ((t.phase == TouchPhase.Stationary) || (t.phase == TouchPhase.Moved)) {
						held = true;
						GUI.color = hitColor;
					}
					
				}
			}
			GUI.DrawTexture(brush, graphic);
			frameAlready = true;
			
		}
	}
}






















