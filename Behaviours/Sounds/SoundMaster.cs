using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound {
	public string name;
	public AudioClip[] clips;
	
	public AudioClip GetSound() {
		if (clips.Length == 1) { return clips[0]; }
		if (clips.Length == 0) { return null; }
		int which = (int)(Random.value * clips.Length * .99999999f);
		return clips[which];
	}
	
}


public class SoundMaster : MonoBehaviour {
	public AudioSource audioSet;
	public Sound[] soundSet;
	
	public static Sound[] sounds;
	public static AudioSource audioSettings;
	public static bool started = false;
	
	void Awake() {
		if (started) { return; }
		audioSettings = audioSet;
		sounds = soundSet;
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
	
	public static AudioClip GetSound(string sc) {
		if (audioSettings == null) { return null; }
		if (sounds == null || sounds.Length == 0) { return null; }
		foreach (Sound snd in sounds) {
			if (snd.name == sc) { return snd.GetSound(); }
		}
		return null;
	}
	
}
