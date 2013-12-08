using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public enum AchievableType {
	Flag,
	Value,
	Action,
}

[System.Serializable]
public class AchievableRequirement {		
	public AchievableType type;
	public bool done;
	
	public string name;
	public float value;
	
	public NumberCompare relationship;
	
	public bool Check() {
		if (done) { return false; }
		
		switch (type) {
			case AchievableType.Flag: 
				return ZScript.GetFlag(name);
			case AchievableType.Value:
				return relationship.Comparator()(ZScript.GetData(name), value);
			case AchievableType.Action:
				return done;
			default:
				return false;
		}
	}

	
}

[System.Serializable]
public class Achievable {
	public string name = "DID A THING";
	
	public AchievableRequirement[] requirements;
	
	public int earned = 0;
	public bool awardable = false;
	public System.DateTime firstEarned;
	public System.DateTime lastEarned;
	
	
	
	public void CheckAchieved() {
		if (!awardable) { return; }
		foreach (AchievableRequirement ar in requirements) {
			
			if (!ar.done) { ar.Check(); }
		}
		
		
		
	}
	
	public override string ToString() { return ToString(','); }
	public string ToString(char splitter) {
		StringBuilder str = new StringBuilder();
		str.Append(name);
		str.Append(splitter + earned);
		str.Append(splitter + (awardable ? 1 : 0));
		
		//TBD: append the times as their strings
		//str.Append(splitter + firstEarned.
		//TBD: append other parts of the achivement that need to be saved.
		
		return str.ToString();
	}
	
	public void LoadFromString(string str) { LoadFromString(str, ','); }
	public void LoadFromString(string str, char splitter) {
		string[] content = str.Split(splitter);
		if (content.Length < 2) { 
			Debug.Log("Loading from string failed.\n" + splitter + "\n" + content.Length + "\n" + str); 
			return;
		}
		//TBD: load all elements of the content
		
	}
	
	
	public void Save(string key) { Save(key, ','); }
	public void Save(string key, char splitter) {
		PlayerPrefs.SetString(key + name, ToString(splitter));
	}
	
	public void Load(string key) { Load(key, ','); }
	public void Load(string key, char splitter) {
		string str = PlayerPrefs.GetString(key + name);
		LoadFromString(str, splitter);
	}
	
	
}

public class Achievables {
	public List<Achievable> achievables;
	
	
	public void CheckAll() {
		foreach (Achievable a in achievables) {
			a.CheckAchieved();
		}
	}
	
	
	
	
	public void Save(string key) {
		foreach (Achievable a in achievables) {
			a.Save(key + "_");
		}
	}
	
	public void Load(string key) {
		foreach (Achievable a in achievables) {
			a.Load(key + "_");
		}
	}	
}
