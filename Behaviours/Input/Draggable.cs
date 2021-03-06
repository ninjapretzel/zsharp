using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
	
	const float touchRadius = .2f;
	
	public bool dragging = false;
	public static Draggable selected;
	public LayerMask mask;
	
	// Use this for initialization
	void Start() {
		gameObject.layer = 8; //Change to a layer called draggable
		//mask = mask | (1 << 8);
	}
	
	// Update is called once per frame
	void Update() {
		gameObject.layer = 8;
		
		//if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			
		//} else {
			if (Input.GetMouseButtonUp(0)) {
			if (selected != null) {
				selected.dragging = false;
				selected = null;
			}
			if (selected == this) {
				dragging = false;
				selected = null;
			}
		}
		
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayhit;
			if (Physics.Raycast(ray, out rayhit, 50, 1 << 8)) {
				if (rayhit.collider == collider) {
					dragging = true;
					selected = this;
				}
			}
			
			
		}
		//}
		
		
		
		
		
		
			
		if (selected == null || selected == this) {
			UpdateTouchDrag();
			
			if (dragging && Application.platform != RuntimePlatform.Android) {
				Vector2 pos = Input.mousePosition;
				pos.y = Screen.height - pos.y;
				Drag(Input.mousePosition);
				
			}
		}
	}
	
	void OnMouseDown() {
		//dragging = true;
		//selected = this;
	}
	
	void OnMouseUp() {
		//dragging = false;
		//selected = null;
	}
	
	void UpdateTouchDrag() {
		if (Input.touches.Length > 0) {
			//Debug.Log("Touched");
			Vector3 pos = GetScreenPosition();
			Vector2 screenpos = new Vector2(pos.x, pos.y);
			
			
			for (int i = 0; i < Input.touches.Length; i++) {
				Touch t = Input.touches[i];
				Vector2 tpos = t.position;
				tpos.y = Screen.height - tpos.y;
				
				if ((tpos - screenpos).magnitude < touchRadius * Screen.width) {
					//Debug.Log("Touched");
					//Debug.Log(screenpos + " " + t.position);
					if (t.phase == TouchPhase.Began) { dragging = true; selected = this; }
					else if (t.phase == TouchPhase.Moved && dragging) { Drag(t.position); }
					else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) { dragging = false; selected = null; }
				}
				
			}
		}
	}
	
	void Drag(Vector2 pos) {
		//Debug.Log("Dragging");
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit rayhit;
		//int mask = 1 << 8 | 1 << 2 | 1 << 1 | 1 << 10;
		//mask = ~mask;
		if (Physics.Raycast(ray, out rayhit, 50, mask.value)) {
			//Debug.Log("Raycast hit" + rayhit.collider.gameObject.name);
			transform.position = rayhit.point;
		}
	}
	
	Vector3 GetScreenPosition() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos.y = Screen.height - pos.y;
		return pos;
	}
}
