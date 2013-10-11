using UnityEngine;
using System.Collections.Generic;

public static class SpriteAnimations {
	public static Dictionary<string, SpriteAnimation> animations;
	
	
	public static SpriteAnimation Get(string s) {
		if (animations.ContainsKey(s)) { return animations[s]; }
		return null;
	}
}

[System.Serializable]
public class SpriteAnimation {
	public string name;
	public Texture2D[] frames;
	public float[] times;
	
	float total = 0;
	
	public SpriteAnimation Clone() {
		SpriteAnimation clone = new SpriteAnimation();
		clone.name = name;
		clone.frames = frames;
		clone.times = times;
		return clone;
	}
	
	public Texture2D GetImage() { return GetImage(Time.time); }
	public Texture2D GetImage(float time) {
		float t = Mathf.Repeat(time, GetTotalTime());
		for (int i = 0; i < times.Length; i++) {
			t -= times[i];
			if (t < 0) { return frames[i]; }
		}
		return null;
	}
	
	public Texture2D GetImageNormalized(float time) {
		if (time <= .001) { return frames[0]; }
		if (time >= .999) { return frames[frames.Length-1]; }
		float t = Mathf.Repeat(time, 1);
		float totalTime = GetTotalTime();
		for (int i = 0; i < times.Length; i++) {
			t -= times[i]/totalTime;
			if (t < 0) { return frames[i]; }
		}
		return null;
	}
	
	public float GetTotalTime() {	
		if (total > 0) { return total; }
		float t = 0;
		for (int i = 0; i < times.Length; i++) { t += times[i]; }
		total = t;
		return t;
	}	
	
}