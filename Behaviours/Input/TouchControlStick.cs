using UnityEngine;
using System.Collections;

public class TouchControlStick : MonoBehaviour {
	
	
	public Vector2 value;
	public Rect normalizedArea = new Rect(0, 0, .5f, 1f);
	public Rect area { get { return normalizedArea.Denormalized(); } }
	
	public string stickName = "LeftStick";
	public float sensitivity = 1;
	
	public bool invertX = false;
	public bool invertY = false;
	
	Vector2 touchDown;
	bool hasTouch = false;
	
	public Texture2D touchDownGraphic;
	public Texture2D holdGraphic;
	
	Touch closestTouch {
		get {
			float distance = System.Single.MaxValue;
			
			Touch closest = default(Touch);
			
			foreach (Touch t in Input.touches) {
				float dist = (t.position - touchDown).magnitude;
				if (dist < distance) {
					distance = dist;
					closest = t;
				}
				

				
			}
			
			return closest;
		}
	}
	
	void Start() {
		if (!GameSys.isMobile) {
			Destroy(this);
		}
		
	}
	
	void Update() {
		
	}
	
	void OnGUI() {
		
		if (Input.touches.Length == 0) { 
			hasTouch = false; 
			value = Vector2.zero; 
			return;
		}
		
		if (!hasTouch) {
			value = Vector2.zero;
			
			FindTouch();
		} else {
			Touch t = closestTouch;
			if (area.Contains(t)) {
			
				Vector2 pos = t.ScreenPosition();
				Vector2 diff = pos - touchDown;
				
				diff /= (Screen.width * .15f);
				diff *= sensitivity;
				
				//GUI.Label(area, "" + diff + "." + diff.magnitude);
				
				Rect brush = new Rect(touchDown.x, touchDown.y, 0, 0).Pad(60, 60);
				GUI.DrawTexture(brush, touchDownGraphic ? touchDownGraphic : GUIF.pixel);
				
				brush = new Rect(pos.x, pos.y, 0, 0).Pad(Screen.height * .07f);
				GUI.DrawTexture(brush, holdGraphic ? holdGraphic : GUIF.pixel);
					
				
				if (diff.magnitude > 1) {
					value = diff.normalized;
				} else {
					value = diff;
				}
				
				
				if (!invertY) { value.y *= -1; }
				if (invertX) { value.x *= -1; }
				
				if (t.IsRelease()) {
					hasTouch = false;
				}
				
			}
			
		}
		
	}
	
	
	void FindTouch() {
		foreach (Touch t in Input.touches) {
			if (t.IsPress()) {
				
				if (area.Contains(t)) {
					touchDown = t.ScreenPosition();
					hasTouch = true;
					
				}
				
			}
			
		}
		
	}
	
	
	
}