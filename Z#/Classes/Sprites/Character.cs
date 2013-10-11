using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Character {
	public string name;
	public string pose;
	public Portrait[] portraits;
	Dictionary<string, Texture2D> table;
	
	
	public Texture2D GetImage() {
		SpriteAnimation ani = SpriteAnimations.Get(name+pose);
		if (ani != null) { return ani.GetImage(); }
		if (table.ContainsKey(pose)) { return table[pose]; }
		return null;
	}
	
	public void LoadImages() {
		table = new Dictionary<string, Texture2D>();
		foreach (Portrait p in portraits) { table.Add(p.name, p.image); }
	}
	
}

[System.Serializable]
public class Portrait {
	public string name;
	public Texture2D image;
}
