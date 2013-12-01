using UnityEngine;
using System.Collections;

public class GUITickerMessage : MonoBehaviour {
	public Rect area = new Rect(0, 0, 1, 1);
	public bool isBox = false;
	public int fontSize = 32;
	public int padding = 0;
	public string setMessage = "Hello world";
	public TextAsset script;
	public Texture2D tex;
	public GUISkin skin;
	
	public Color color = Color.white;
	public bool skip = false;
	public float tickTime = .1f;
	public float timeout = 0;
	public float waitTime = 3;
	public float fadeTime = 2;
	
	public GUITickerMessage nextMessage;
	
	public static float timeScale = 1;
	
	public string message { get { if (script) { return script.text; } return setMessage; } }
	public float messageTime { get { return message.Length * tickTime; } }
	public float messageWaitTime { get { return messageTime + waitTime; } }
	public float wholeTime { get { return messageTime + waitTime + fadeTime; } }
	public float alpha { get { return 1 - (timeout - messageWaitTime / fadeTime); } }
	
	public bool dismissed { get { return skip || timeout >= wholeTime; } }
	public bool fading { get { return skip || timeout >= messageWaitTime; } }
	public bool doneWriting { get { return timeout >= messageTime; } }
	public string tickMessage {
		get {
			int index = (int)(timeout / tickTime);
			if (index >= message.Length) { return message; }
			return message.Substring(0, index);
		}
	}
	
	
	
	void Update() {
		timeout += Time.deltaTime * timeScale;
		#if UNITY_EDITOR
		if (Input.GetKey("e")) {
			timeout += Time.deltaTime * 45;
		}
		#endif
		if (fading && timeout < messageWaitTime) { timeout = messageWaitTime; }
		if (dismissed) { Finish(); }
		
	}
	
	void OnGUI() {
		GUI.depth = -100000;
		
		GUI.skin = skin;
		GUI.skin.FontSize(fontSize);
		
		GUIContent content;
		if (tex == null) { content = new GUIContent(tickMessage, tex); }
		else { content = new GUIContent(tickMessage); }
		
		Color col = color;
		if (fading) { col.a = alpha; }
		GUI.color = col;
		
		Rect brush = area.Denormalized();
		if (isBox) { GUIF.Box(brush, content, padding); }
		else { GUIF.Label(brush, content, padding); }
		
	}
	
	
	void Finish() {
		if (nextMessage != null) {
			Instantiate(nextMessage, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}
	
	
}

















