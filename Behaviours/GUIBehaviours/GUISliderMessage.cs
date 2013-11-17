using UnityEngine;
using System.Collections;

public class GUISliderMessage : MonoBehaviour {
	public Rect area = new Rect(0, 0, 1, 1);
	public Cardinal direction = Cardinal.Up;
	public Slider slider;
	public float slidePower = 1;
	public int fontSize = 0;
	public string message = "Hello World";
	public Texture2D tex;
	public bool dismissed = false;
	public GUISkin skin;
	
	
	public static GUISliderMessage Factory(Rect a) { return Factory(a, 1, "Hello World", Cardinal.Up); }
	public static GUISliderMessage Factory(Rect a, string m) { return Factory(a, 1, m, Cardinal.Up); }
	public static GUISliderMessage Factory(Rect a, float power, string m) { return Factory(a, power, m, Cardinal.Up); }
	public static GUISliderMessage Factory(Rect a, string m, Cardinal direction) { return Factory(a, 1, m, direction); }
	public static GUISliderMessage Factory(Rect a, float power, string m, Cardinal direction) {
		GameObject g = new GameObject("GUISliderMessage");
		GUISliderMessage sm = g.AddComponent<GUISliderMessage>();
		sm.slidePower = power;
		sm.area = a;
		sm.message = m;
		sm.direction = direction;
		sm.skin = Resources.Load("message", typeof(GUISkin)) as GUISkin;
		return sm;
	}
	
	void Start() {
		slider = new Slider(area);
		slider.Slide(direction, slidePower);
		
	}
	
	void Update() {
		slider.Update();
		if (dismissed && slider.done) { Destroy(gameObject); }
	}
	
	void OnGUI() {
		GUI.depth = -1;
		
		if (dismissed) { Draw(slider.OUT); }
		else { Draw(slider.IN); }
		
	}
	
	void Draw(Rect a) {
		GUI.skin = skin;
		GUI.skin.FontSize(fontSize);
		
		GUIContent c;
		if (tex != null) { c = new GUIContent(message); }
		else { c = new GUIContent(message, tex); }
		
		GUIF.Box(a, c);
		
		if (GUIF.Button(a.BottomCenter(.3f, .2f), "Ok")) { 
			dismissed = true; 
			slider.Slide(direction.Flip(), slidePower);
		}
		
	}
	
	
	
}
