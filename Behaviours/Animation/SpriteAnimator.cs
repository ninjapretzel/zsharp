#if !UNITY_FLASH
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SpriteAnimationSettings {
	public string name;
	public float animSpeed;
	
}

public class SpriteAnimator : MonoBehaviour {
	public string characterName;
	public string animName;
	public string anim { get { return animName; } 
		set {
			SetAnim(value);
		}
	}
	
	public List<SpriteAnimationSettings> settings;
	public Dictionary<string, Sprite[]> animations;
	public Dictionary<string, float> animationSpeeds;
	
	public string path { get { return characterName + "/" + animName; } } 
	public string lastPath;
	
	public Sprite[] sprites;
	public int frame;
	public float animSpeed = 10;
	public float timeout = 0;
	
	public SpriteRenderer spriteRenderer;
	
	
	
	// Use this for initialization
	void Start() {
		PopulateSprites();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = animName;
	}
	
	// Update is called once per frame
	void Update () {
		timeout += Time.deltaTime * animSpeed;
		timeout = Mathf.Repeat(timeout, sprites.Length);
		frame = (int)timeout.Floor();
		frame = (int)Mathf.Clamp(frame, 0, sprites.Length-1);
		if (sprites.Length == 0) { return; }
		spriteRenderer.sprite = sprites[frame];
		
		
	}
	
	public void PopulateSprites() {
		SetFrames(characterName);
		animations = new Dictionary<string, Sprite[]>();
		animationSpeeds = new Dictionary<string, float>();
		
		foreach (SpriteAnimationSettings s in settings) {
			List<Sprite> sprs = new List<Sprite>();
			foreach (Sprite sprite in sprites) {
				if (sprite.name.Contains(s.name)) {
					sprs.Add(sprite);
					
				}
				
				
			}
			
			animationSpeeds.Add(s.name, s.animSpeed);
			animations.Add(s.name, sprs.ToArray());
		}
		
		
	}
	
	public void SetAnim(string a) {
		if (animName == a) { return; }
		animName = a;
		sprites = animations[a];
		animSpeed = animationSpeeds[a];
	}
	
	public void SetFrames() { SetFrames(path); }
	public void SetFrames(string p) {
		Sprite[] sprs = Resources.LoadAll<Sprite>(path);
		if (sprs.Length > 0) {
			lastPath = p;
			sprites = sprs;
		}
	}
}
#endif
