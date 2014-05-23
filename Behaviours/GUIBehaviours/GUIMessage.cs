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
	
	public Color color = Color.white;
	
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
	public string message { 
		get { return msg.str; }
		set { msg.str = value; }
	}
	
	
	public Vector2 baseSpeed { get { return sets.speed; } set { sets.speed = value; } }
	public Vector2 rndSpeed { get { return sets.rndSpeed; } set { sets.rndSpeed = value; } }
	public Vector2 acceleration { get { return sets.acceleration; } set { sets.acceleration = value; } }
	public float fadeTime { get { return sets.fadeTime; } set { sets.fadeTime = value; } }
	public Color color { get { return sets.color; } set { sets.color = value; } }
	
	public Message msg;
	
	public Vector2 position = new Vector2(.5f, .5f);
	public Vector2 speed = new Vector2(0, -.2f);
	
	public GUIMessageSettings sets;
	
	float time;
	float startTime = 3.0f;
	public bool outlined = false;
	
	public static GUIMessageSettings defaults;
	public static GUISkin skin;
	
	public int num = 0;
	public int depth = 800000;
	
	public static int count = 0;
	public static int order = 800000;
	
	
	
	
	
	static GUIMessage() {
		skin = Resources.Load("message", typeof(GUISkin)) as GUISkin;
		
	}
	
	void Awake() {
		if (defaults == null) { defaults = new GUIMessageSettings(); }
		if (sets == null) { sets = defaults.Clone(); }
		depth = order - count;
		count++;
		msg = new Message();
		
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
		
		
		Vector2 size = GUI.skin.label.CalcSize(new GUIContent(message));
		//Vector2 size = new Vector2(1, 1);
		size.x += GUI.skin.box.padding.left + GUI.skin.box.padding.right + 2;
		size.y += GUI.skin.box.padding.top + GUI.skin.box.padding.bottom + 2;
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
			GUIF.Label(area, message);
		} else {
			GUI.Label(area, message);
		}
		//*/
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
