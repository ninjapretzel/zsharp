using UnityEngine;
using System.Collections;

public class SoundVolume : MonoBehaviour {
	float baseVolume;
	
	void Awake() {
		baseVolume = audio.volume;
	}
	void Update() {
		audio.volume = baseVolume * Settings.soundVolume;
	}
}
