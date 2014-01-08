using UnityEngine;
using System.Collections;

public class DisplaysString : MonoBehaviour {
	TextMesh display;
	
	void Awake() {
		display = GetComponent<TextMesh>();
		
	}
	
	void Update() {
		display.text = GetString();
		
	}
	
	public virtual string GetString() {
		return "butts";
	}
	
}
