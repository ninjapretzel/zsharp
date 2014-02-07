using UnityEngine;
using System.Collections;

public static partial class Settings {
	public static float overscanRatio = 0;
	public static float musicVolume  = 1;
	public static float soundVolume = 1;
	
	public static bool hints = true;
	
	public static void Save() {
		PlayerPrefs.SetFloat("set_overscan", overscanRatio);
		PlayerPrefs.SetFloat("set_musicVolume", musicVolume);
		PlayerPrefs.SetFloat("set_soundVolume", soundVolume);
		
		PlayerPrefsF.SetBool("set_hints", hints);
	}
	
	public static void Load() {
		overscanRatio = PlayerPrefs.GetFloat("set_overscan");
		musicVolume = PlayerPrefs.GetFloat("set_musicVolume");
		soundVolume = PlayerPrefs.GetFloat("set_soundVolume");
		
		hints = PlayerPrefsF.GetBool("set_hints");
		
	}
	
	
}
