using UnityEngine;
using System.Collections;

public class GUIStatText : MonoBehaviour {
	
	public string message = "Butts: ";
	public string statName = "butts";
	public bool floored = true;
	TextMesh textMesh;
	
	public string content {
		get {
			return "" + (floored ? ZScript.Get(statName).Floor() : ZScript.Get(statName));
		}
	}
	
	void Awake() {
		InitText();
	}
	
	void InitText() {
		TextMesh test = GetComponent<TextMesh>();
		
		if (test == null) {
			textMesh = transform.Require<TextMesh>();
			textMesh.font = Resources.Load("Courier New", typeof(Font)) as Font;
			renderer.material = textMesh.font.material;
		} else {
			textMesh = test;
			renderer.material = textMesh.font.material;
		}
	}
	
	
	void Update() {
		textMesh.text = message + content;
	
	}
	
}
