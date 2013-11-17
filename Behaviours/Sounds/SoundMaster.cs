using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sound {
	public string name;
	public List<AudioClip> clips;
	
	public Sound() { clips = new List<AudioClip>(); }
	
	public Sound(string n, AudioClip c) {
		name = n;
		clips = new List<AudioClip>();
		clips.Add(c);
	}
	
	public AudioClip GetSound() {
		if (clips.Count == 1) { return clips[0]; }
		if (clips.Count == 0) { return null; }
		int which = (int)(RandomF.value * clips.Count);
		return clips[which];
	}
	
	public void AddClips(List<AudioClip> list) {
		foreach (AudioClip clip in list) { clips.Add(clip); }
	}
	
}


public class SoundMaster : MonoBehaviour {
	public AudioSource audioSet;
	public List<Sound> soundSet;
	
	public static Dictionary<string, Sound> sounds;
	public static AudioSource audioSettings;
	public static bool started = false;
	
	static SoundMaster() {
		sounds = new Dictionary<string, Sound>();
		
	}
	
	void Awake() {
		if (started) { return; }
		audioSettings = audioSet;
		foreach (Sound s in soundSet) { Add(s); }
		
		started = true;
	}
	
	public static AudioSource Play(AudioClip sc) {
		if (audioSettings == null) { return null; }
		Vector3 pos = Vector3.zero;
		if (Camera.main) { pos = Camera.main.transform.position; }
		return Play(sc, pos);
	}
	
	public static AudioSource Play(AudioClip sc, Vector3 pos) {
		if (audioSettings == null) { return null; }
		AudioSource source = Instantiate(audioSettings, pos, Quaternion.identity) as AudioSource;
		source.clip = sc;
		source.Play();
		return source;
	}
	
	public static AudioSource Play(string sc) { return Play(GetSound(sc)); }
	public static AudioSource Play(string sc, Vector3 pos) { return Play(GetSound(sc), pos); }
	
	public static AudioClip Get(string sc) { return GetSound(sc); }
	public static AudioClip GetSound(string sc) {
		if (audioSettings == null) { return null; }
		if (sounds == null || !sounds.ContainsKey(sc)) { return null; }
		return sounds[sc].GetSound();
	}
	
	public static bool Has(string sc) { return sounds.ContainsKey(sc); }
	
	public static void Add(Sound sc) {
		if (sounds.ContainsKey(sc.name)) { sounds[sc.name].AddClips(sc.clips); }
		else { sounds.Add(sc.name, sc); }
	}
	
}
