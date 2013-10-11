using UnityEngine;
using System.Collections;

public static class ScanlineEffect {
	public static Texture2D scanline1;
	public static Texture2D scanline2;
	
	public static Texture2D staticTex1;
	public static Texture2D staticTex2;
	
	public static float offset1 = 0;
	public static float offset2 = 0;
	
	public static float speed1 = 1;
	public static float speed2 = -.25f;
	
	public static float repeat1 = 120;
	public static float repeat2 = .57f;
	
	public static float alpha1 = .2f;
	public static float alpha2 = .3f;
	
	public static float alphaAdd1 = .05f;
	public static float alphaAdd2 = .1f;
	
	public static float staticPos1 = 0;
	public static float staticPos2 = 0;
	
	public static bool displayStatic1 = false;
	public static bool displayStatic2 = false;
	
	public static void Awake() {
		scanline1 = Resources.Load("Scanlines1", typeof(Texture2D)) as Texture2D;
		scanline2 = Resources.Load("Scanlines2", typeof(Texture2D)) as Texture2D;
		
		staticTex1 = Resources.Load("StaticTex1", typeof(Texture2D)) as Texture2D;
		staticTex2 = Resources.Load("StaticTex2", typeof(Texture2D)) as Texture2D;
	}
	
	public static void Update() {
		UpdateSlide();
	}
	
	public static void UpdateSlide() {
		offset1 += Time.deltaTime * speed1;
		offset2 += Time.deltaTime * speed2;
	}
	
	
	public static void UpdateFlicker() {
		if (Random.value < .5f * Time.fixedDeltaTime) {
			offset2 += Random.value * .3f * (-1 + Random.value * 2);
		}
		
		if (Random.value < 8 * Time.fixedDeltaTime) {
			offset2 += Random.value * .05f * (-1 + Random.value * 2);
		}
		
		displayStatic1 = (Random.value < .5f * Time.fixedDeltaTime);
		displayStatic2 = (Random.value < .5f * Time.fixedDeltaTime);
	}
	
	public static void Draw() {
		Rect screen = new Rect(0, 0, Screen.width, Screen.height);
		float ratio = (0.0f + Screen.height) / 480.0f;
		Rect off1 = new Rect(0, offset1, 1, repeat1 * ratio);
		Rect off2 = new Rect(0, offset2, 1, repeat2 * ratio);
		
		Color c = Color.white;
		c.a = alpha1 + alphaAdd1 * Random.value;
		GUI.color = c;
		GUI.DrawTextureWithTexCoords(screen, scanline1, off1);
		
		c.a = alpha2 + alphaAdd2 * Random.value * Random.value;
		GUI.color = c;
		GUI.DrawTextureWithTexCoords(screen, scanline2, off2);
		
		Rect brush;
		staticPos1 = Random.value;
		staticPos2 = Random.value;
		if (displayStatic1) {
			brush = new Rect(0, staticPos1 * Screen.height, Screen.width, Screen.height * .05f * Random.value);
			brush.y -= brush.height/2;
			c.a = .5f * (alpha1 + alphaAdd1 * Random.value);
			GUI.color = c;
			GUI.DrawTexture(brush, staticTex1);
		}
		
		if (displayStatic2) {
			brush = new Rect(0, staticPos2 * Screen.height, Screen.width, Screen.height * .05f * Random.value);
			brush.y -= brush.height/2;
			c.a = .5f * (alpha1 + alphaAdd1 * Random.value);
			GUI.color = c;
			GUI.DrawTexture(brush, staticTex2);
		}
		//*/
		
	}
}
































