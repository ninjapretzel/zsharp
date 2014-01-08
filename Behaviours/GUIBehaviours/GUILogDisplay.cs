using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUILogDisplay : MonoBehaviour {
	
	public float dampening = 15;
	public float offsetRatio = .04f;
	
	public int maxLogs = 10;
	
	public TextMesh text;
	
	List<TextMesh> texts = new List<TextMesh>();
	
	
	void Start() {
		text.gameObject.SetActive(false);
		
	}
	
	
	void Update() {
		for (float i = 0; i < texts.Count; i += 1) {
			Transform t = texts[(int)i].transform;
			t.position = t.position.TLerp(GetTargetPosition(i), dampening);
			
		}
	}
	
	public void Fill(string s) {
		for (int i = 0; i < maxLogs; i++) { Log(s); }
	}
	
	Vector3 GetTargetPosition(float i) {
		return transform.position + Vector3.up * offsetRatio * (maxLogs-1) - Vector3.up * i * offsetRatio;
	}
	
	public void Log(string s) { Log(s, Color.white); }
	public void Log(string s, Color c) { 
		TextMesh copy = text.DuplicateAs<TextMesh>();
		copy.gameObject.SetActive(true);
		copy.text = s;
		copy.color = c;
		
		texts.Add(copy);
		if (texts.Count > maxLogs) {
			Destroy(texts[0].gameObject);
			texts.RemoveAt(0);
		}
	}
	
}
