using UnityEngine;
using System.Collections;

public class GUIScrollableArea : MonoBehaviour {
	
	public Transform anchor;
	public Vector2 scrollPosition;
	public Vector2 minScrollPosition = new Vector2(0, -10);
	public Vector2 maxScrollPosition = new Vector2(0, 10);
	public float scrollSpeed = 5;
	
	public bool enableTouchScrolling = false;
	public bool enableScrollWheel = true;
	public float scrollSensitivity = 2;
	
	
	#if UNITY_ANDROID || UNITY_IOS
	Vector2 scrollVelocity;
	static float velocityDampening = 8;
	#endif
	
	
	void Start() {
		if (!anchor) { anchor = transform.Find("Anchor"); }
	} 
	
	void Update() {
		#if UNITY_ANDROID || UNITY_IOS
		if (enableTouchScrolling) {
			scrollVelocity += InputF.TouchVelocity(ScreenF.all);
			scrollVelocity = scrollVelocity.TLerp(velocityDampening);
			scrollPosition += scrollVelocity * Time.deltaTime;
		}
		#endif
		
		if (enableScrollWheel) {
			scrollPosition += Vector2.up * Input.GetAxis("MouseWheel") * scrollSensitivity;
		}
		
		scrollPosition = scrollPosition.Clamp(minScrollPosition, maxScrollPosition);
		Vector3 targetPosition = (Vector3)scrollPosition;
		anchor.transform.localPosition = Vector3.Lerp(anchor.transform.localPosition, targetPosition, Time.deltaTime * scrollSpeed);
		
		
	}
}
