using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetsSprites : MonoBehaviour {
	public SpriteAnimation[] animations;
	
	void Awake() {
		if (SpriteAnimations.animations == null) { 
			SpriteAnimations.animations = new Dictionary<string, SpriteAnimation>();
		}
		
		foreach (SpriteAnimation ani in animations) {
			if (!SpriteAnimations.animations.ContainsKey(ani.name)) {
				SpriteAnimations.animations.Add(ani.name, ani);
			}
		}
	}
	
}
