using UnityEngine;
using System.Collections;
//using RectF;

[System.Serializable]
public class GUIMessageSettings {
	public Vector2 speed = new Vector2(0, -.2f);
	public Vector2 rndSpeed = new Vector2(0, 0);
	public Vector2 acceleration = new Vector2(0, .2f);
	
	public float time = 2.0f;
	public float fadeTime = 1.0f;
	
	public Color color;
	
	public GUIMessageSettings Clone() {
		GUIMessageSettings sets = new GUIMessageSettings();
		sets.speed = speed;
		sets.rndSpeed = rndSpeed;
		sets.acceleration = acceleration;
		
		sets.time = time;
		sets.fadeTime = fadeTime;
		
		sets.color = color;
		
		return sets;
	}
}

public class GUIMessage : MonoBehaviour {
	public string message = "message";
	
	public Vector2 position = new Vector2(.5f, .5f);
	public Vector2 speed = new Vector2(0, -.2f);
	
	public GUIMessageSettings sets;
	
	float time;
	float startTime = 3.0f;
	
	public static GUIMessageSettings defaults;
	public static GUISkin skin;
	public static Texture2D pixel;
	
	public int num = 0;
	public int depth = 800000;
	
	public static int count = 0;
	public static int order = 800000;
	
	
	void Awake() {
		if (defaults == null) { defaults = new GUIMessageSettings(); }
		if (sets == null) { sets = defaults.Clone(); }
		if (!pixel) { pixel = Resources.Load("pixel", typeof(Texture2D)) as Texture2D; }
		depth = order - count;
		count++;
	}
	
	void Start() {
		startTime = sets.time;
		time = startTime;
		speed = sets.speed;
		speed.x += sets.rndSpeed.x * (-1 + Random.value * 2);
		speed.y += sets.rndSpeed.y * (-1 + Random.value * 2);
		
	}
	
	void Update() {
		speed += sets.acceleration * Time.deltaTime;
		position += speed * Time.deltaTime;
		
		time -= Time.deltaTime;
		if (time <= 0) { Destroy(gameObject); }
	}
	
	void OnGUI() {
		GUI.skin = skin;
		GUI.depth = depth;
		
		Color c = sets.color;
		c.a = time / sets.fadeTime;
		GUI.color = c;
		
		
		Vector2 size = skin.label.CalcSize(new GUIContent(message));
		Rect area = new Rect(position.x * Screen.width, position.y * Screen.height, size.x, size.y);
		area = area.Pad(4.0f);
		area = area.Move(-.5f, -.5f);
		GUIF.Label(area, message);
	}
	
	public static GUIMessage Create(Vector2 pos, string msg, GUIMessageSettings sets) {
		GUIMessage mess = Create(pos, msg);
		mess.sets = sets;
		return mess;
	}
	
	public static GUIMessage Create(Vector2 pos, string msg) {
		GameObject obj = new GameObject("GUIMessage");
		GUIMessage mess = obj.AddComponent<GUIMessage>();
		
		mess.position = pos;
		mess.message = msg;
		
		return mess;
	}
	
}
