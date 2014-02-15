using UnityEngine;
using System.Collections;

public class DisplaysString : MonoBehaviour {
	TextMesh display;
	
	void Awake() {
		display = GetComponent<TextMesh>();
		
	}
	
	void Update() {
		string s = GetString();
		if (s != display.text) {
			display.text = s;
		}
	}
	
	public virtual string GetString() {
		return "butts";
	}
	
}
