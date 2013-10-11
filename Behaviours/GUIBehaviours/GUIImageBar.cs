using UnityEngine;
using System.Collections;

public class GUIImageBar : MonoBehaviour {
	
	public float val = 0;
	public string skin;
	public int depth = 0;
	public Rect area = new Rect(.3f, .9f, .4f, .1f);
	public bool lockWidthToHeight = true;

	public Color tint = Color.white;

	public SpriteAnimation ani;
	
	void Start() {
		ani = SpriteAnimations.Get(skin);
	}
	
	void OnGUI() {
		if (ani == null || ani.name == "") {
			ani = SpriteAnimations.Get(skin);
			if (ani == null) { return; }
		}
		
		GUI.depth = depth;
		Rect brush = area;
		if (lockWidthToHeight) {
			brush = area.MiddleCenter(area.height/area.width, 1);
		}
		
		GUI.DrawTexture(brush, ani.GetImageNormalized(val));
		
		
	}
}
