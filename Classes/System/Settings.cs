using UnityEngine;
using System.Collections;

public enum QualitySetting { Low, Medium, High }


public static class SettingsUtils {
	
}


public static partial class Settings {
	public static float overscanRatio = 0;
	public static float musicVolume = 1;
	public static float soundVolume = 1;
	
	//Custom settings can be stored in this table.
	//This will be saved and loaded automatically.
	public static Table custom;
	
	public static bool hints = true;
	
	static Settings() {
		custom = new Table();
		
		
		
		
	}
	
	
	public static void Save() {
		PlayerPrefs.SetFloat("set_overscan", overscanRatio);
		PlayerPrefs.SetFloat("set_musicVolume", musicVolume);
		PlayerPrefs.SetFloat("set_soundVolume", soundVolume);
		
		custom.Save("set_custom");
		
		PlayerPrefsF.SetBool("set_hints", hints);
	}
	
	public static void Load() {
		if (!PlayerPrefs.HasKey("set_musicVolume")) {
			musicVolume = .5f;
			soundVolume = .5f;
			overscanRatio = 0;
			return;
		}
		overscanRatio = PlayerPrefs.GetFloat("set_overscan");
		musicVolume = PlayerPrefs.GetFloat("set_musicVolume");
		soundVolume = PlayerPrefs.GetFloat("set_soundVolume");
		
		custom.Load("set_custom");
		
		hints = PlayerPrefsF.GetBool("set_hints");
		
	}
	
	
}
