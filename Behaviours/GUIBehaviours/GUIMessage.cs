using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GUIMessageSettings : ConvertsToTable {
	public Vector2 speed = new Vector2(0, -.2f);
	public Vector2 rndSpeed = new Vector2(0, 0);
	public Vector2 acceleration = new Vector2(0, .2f);
	
	public float time = 2.0f;
	public float fadeTime = 1.0f;
	public float fontSize = 16;
	public float dFontSize = 60;
	public float ddFontSize = -90;
	
	public Color color = Color.white;
	
	public GUIMessageSettings Clone() {
		GUIMessageSettings sets = new GUIMessageSettings();
		sets.speed = speed;
		sets.rndSpeed = rndSpeed;
		sets.acceleration = acceleration;
		
		sets.time = time;
		sets.fadeTime = fadeTime;
		
		sets.fontSize = fontSize;
		sets.dFontSize = dFontSize;
		sets.ddFontSize = ddFontSize;
		
		
		sets.color = color;
		
		return sets;
	}
	
	
}

public class GUIMessage : MonoBehaviour {
	/*
	public string message { 
		get { return msg.str; }
		set { msg.str = value; }
	}
	//*/
	
	
	public Vector2 baseSpeed { get { return sets.speed; } set { sets.speed = value; } }
	public Vector2 rndSpeed { get { return sets.rndSpeed; } set { sets.rndSpeed = value; } }
	public Vector2 acceleration { get { return sets.acceleration; } set { sets.acceleration = value; } }
	public float fadeTime { get { return sets.fadeTime; } set { sets.fadeTime = value; } }
	public float ddFontSize { get { return sets.ddFontSize; } set { sets.ddFontSize = value; } }
	public Color color { get { return sets.color; } set { sets.color = value; } }
	
	//public Message msg;
	public string message;
	public GUIMessageSettings sets;
	
	public float fontSize = 16;
	public float dFontSize;
	
	public Vector2 position = new Vector2(.5f, .5f);
	public Vector2 speed = new Vector2(0, -.2f);
	
	public bool outlined = false;
	
	public int num = 0;
	public int depth = 800000;
	
	
	float time;
	float startTime = 3.0f;
	
	
	public static int count = 0;
	public static int order = 800000;
	public static GUIMessageSettings defaults;
	public static GUISkin skin;
	
	public static Dictionary<string, GUIMessageSettings> settingsMap;
	
	
	
	static GUIMessage() {
		skin = Resources.Load("MSG", typeof(GUISkin)) as GUISkin;
		
		if (skin == null) { 
			Debug.Log("Message skin override not found.");
			skin = Resources.Load("message", typeof(GUISkin)) as GUISkin; 
		}
		
		settingsMap = new Dictionary<string, GUIMessageSettings>();
		
		TextAsset file = Resources.Load("GUIMessageSettings", typeof(TextAsset)) as TextAsset;
		string[] lines = file.text.ConvertNewlines().Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			int index = lines[i].IndexOf(',');
			string name = lines[i].Substring(0, index);
			string rest = lines[i].Substring(index+1);
			
			Table t = new Table(rest);
			
			GUIMessageSettings sets = new GUIMessageSettings();
			sets.asTable = t;
			
			settingsMap.Add(name, sets);
			
		}
		
		
		
	}
	
	void Awake() {
		if (defaults == null) { defaults = new GUIMessageSettings(); }
		if (sets == null) { sets = defaults.Clone(); }
		depth = order - count;
		count++;
		//msg = new Message();
		
	}
	
	void Start() {
		startTime = sets.time;
		time = startTime;
		speed = sets.speed;
		speed.x += sets.rndSpeed.x * (-1 + Random.value * 2);
		speed.y += sets.rndSpeed.y * (-1 + Random.value * 2);
		
		fontSize = sets.fontSize;
		dFontSize = sets.dFontSize;
		
		//Debug.Log(fontSize);
	}
	
	void Update() {
		speed += sets.acceleration * Time.deltaTime;
		position += speed * Time.deltaTime;
		
		fontSize += dFontSize * Time.deltaTime;
		//if (fontSize < 5) { fontSize = 5; }
		dFontSize += ddFontSize * Time.deltaTime;
		
		time -= Time.deltaTime;
		if (time <= 0) { Destroy(gameObject); }
	}
	
	void OnGUI() {
		GUI.skin = skin;
		GUI.depth = depth;
		
		Color c = sets.color;
		c.a = time / sets.fadeTime;
		GUI.color = c;
		
		GUIStyle style = GUI.skin.label.FontSize(fontSize);
		
		Vector2 size = style.CalcSize(new GUIContent(message));
		//Vector2 size = new Vector2(1, 1);
		//size.x += GUI.skin.box.padding.left + GUI.skin.box.padding.right + 2;
		//size.y += GUI.skin.box.padding.top + GUI.skin.box.padding.bottom + 2;
		//position -= size * .5f;
		
		//Debug.Log(size);
		
		/*
		size.x /= Screen.width;
		size.y /= Screen.height;
		Rect area = new Rect(position.x, position.y, size.x, size.y);
		area.x -= size.x *.5f;
		area.y -= size.y *.5f;
		msg.Draw(area);
		//*/
		
		//*
		Rect area = new Rect(position.x * Screen.width, position.y * Screen.height, size.x, size.y);
		area = area.Pad(4.0f);
		area = area.Move(-.5f, -.5f);
		
		if (outlined) {
			GUIF.Label(area, message, style);
		} else {
			GUI.Label(area, message, style);
		}
		//*/
	}
	
	public GUIMessage SetColor(Color c) { 
		color = c;
		return this;
	}
	
	
	public static GUIMessage Create(Vector2 pos, string msg, GUIMessageSettings sets) {
		GUIMessage mess = Create(pos, msg);
		mess.sets = sets;
		return mess;
	}
	
	public static GUIMessage Create(Vector2 pos, string msg) { return Create(pos, msg, "defaultSettings"); }
	public static GUIMessage Create(Vector2 pos, string msg, string style) {
		GameObject obj = new GameObject("GUIMessage");
		GUIMessage mess = obj.AddComponent<GUIMessage>();
		
		if (settingsMap.ContainsKey(style)) {
			mess.sets = settingsMap[style].Clone();
		}
		
		mess.position = pos;
		mess.message = msg;
		
		return mess;
	}
	
}
