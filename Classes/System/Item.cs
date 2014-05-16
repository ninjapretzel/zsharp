using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

public class Inventory : List<Item> {
	
	public Inventory() : base() { }
	public Inventory(string s) : base() { LoadString(s); }
	
	
	
	public Item Get(string name) { return GetNamed(name); }
	public Item GetNamed(string name) {
		foreach (Item i in this) { if (i.name == name) { return i; } }
		return null;
	}
	
	public List<Item> stackables { get { return this.Where(item => item.stacks).ToList(); } }
	public List<Item> nonStackables { get { return this.Where(item => !item.stacks).ToList(); } }
	
	public List<Item> unlocked { get { return this.Where(item => !item.locked).ToList(); } }
	public List<Item> locked { get { return this.Where(item => item.locked).ToList(); } }
	
	
	public List<Item> SelectType(string type) { return this.Where(item => item.type == type).ToList(); }
	
	public List<Item> SelectOverRarity(float rarity) { return this.Where(item => item.rarity > rarity).ToList(); }
	public List<Item> SelectUnderRarity(float rarity) { return this.Where(item => item.rarity < rarity).ToList(); }
	
	public void Save(string key) {
		PlayerPrefs.SetString(key, ToString());
		
	}
	
	public void Load(string key) {
		LoadString(PlayerPrefs.GetString(key));
		
	}
	
	public void LoadString(string str) {
		string[] lines = str.Split('\n');
		
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i].Length <= 3) { continue; }
			if (lines[i][0] == '#') { continue; }
			Add(new Item(lines[i]));
		}
		
		
	}
	
	public override string ToString() {
		StringBuilder str = new StringBuilder("");
		for (int i = 0; i < Count; i++) {
			str.Append(this[i].ToString());
			if (i < Count-1) { str.Append("\n"); }
		}
		return str.ToString();
	}
	
	
	
	
	
}


[System.Serializable]
public class Item : IComparable<Item> {
	public Texture2D iconLoaded;
	public Table stats;
	public Table properties;
	public StringMap strings;
	
	public static Inventory database;
	
	static Item() {
		database = new Inventory();
		TextAsset file = Resources.Load("Items", typeof(TextAsset)) as TextAsset;
		if (file != null) {
			database = new Inventory(file.text);
			
		}
		
		
	}
	
	public void ReloadIcon() { iconLoaded = Resources.Load(iconName, typeof(Texture2D)) as Texture2D; }
	public Texture2D icon {
		get {
			if (iconLoaded != null) { return iconLoaded; }
			Texture2D i = Resources.Load(iconName, typeof(Texture2D)) as Texture2D;
			if (i != null) { iconLoaded = i; }
			return i;
		}
	}
	
	public string name { get { return strings["name"]; } set { strings["name"] = value; } }
	public string desc { get { return strings["desc"]; } set { strings["desc"] = value; } }
	public string type { get { return strings["type"]; } set { strings["type"] = value; } }
	public string baseName { get { return strings["baseName"]; } set { strings["baseName"] = value; } }
	public string iconName { get { return strings["iconName"]; } set { strings["iconName"] = value; } }
	
	public Color color { get { return properties.GetColor("color"); } set { properties.SetColor("color", value); }  }
	public bool locked { get { return properties["locked"] == 1; } set { properties["locked"] = value ? 1 : 0; } }
	public bool stacks { get { return properties["stacks"] == 1; } set { properties["stacks"] = value ? 1 : 0; } }
	public bool equip { get { return properties["equip"] == 1; } set { properties["equip"] = value ? 1 : 0; } }
	public bool equipInWholeRange { get { return properties["equipInWholeRange"] == 1; } set { properties["equipInWholeRange"] = value ? 1 : 0; } }
	
	
	public int count { get { return (int)properties["count"]; } set { properties["count"] = value; } }
	public int maxStack { get { return (int)properties["maxStack"]; } set { properties["maxStack"] = value; } }
	public int equipSlot { get { return (int)properties["equipSlot"]; } set { properties["equipSlot"] = value; } }
	public int maxSlot { get { return (int)properties["maxSlot"]; } set { properties["maxSlot"] = value; } }
	public int minSlot { get { return (int)properties["minSlot"]; } set { properties["minSlot"] = value; } }
	
	public float value { get { return properties["value"]; } set { properties["value"] = value; } }
	public float rarity { get { return properties["rarity"]; } set { properties["rarity"] = value; } }
	public float quality { get { return properties["quality"]; } set { properties["quality"] = value; } }
	public float speed { get { return properties["speed"]; } set { properties["speed"] = value; } }
	public float mpCost { get { return properties["mpCost"]; } set { properties["mpCost"] = value; } }
	public float power { get { return properties["power"]; } set { properties["power"] = value; } }
	public float size { get { return properties["size"]; } set { properties["size"] = value; } }
	//public float xxxx { get { return properties["xxxx"]; } set { properties["xxxx"] = value; } }
	
	public Color rarityColor { get { return RarityColor(rarity); } }
	public static Color RarityColor(float v) { return RarityColor(v, 10, 0); }
	public static Color RarityColor(float v, float s) { return RarityColor(v, s, 0); }
	public static Color RarityColor(float v, float s, float o) {
		if (v+o < 0) { return new Color(0.4f, 0.4f, 0.4f); }
		else if (v+o < s * 01) { return new Color(0.7f, 0.7f, 0.7f); }
		else if (v+o < s * 02) { return new Color(1.0f, 1.0f, 1.0f); }
		else if (v+o < s * 03) { return new Color(0.5f, 1.0f, 0.5f); }
		else if (v+o < s * 04) { return new Color(0.0f, 1.0f, 0.0f); }
		else if (v+o < s * 05) { return new Color(0.0f, 1.0f, 0.5f); }
		else if (v+o < s * 06) { return new Color(0.0f, 1.0f, 1.0f); }
		else if (v+o < s * 07) { return new Color(0.0f, 0.5f, 1.0f); }
		else if (v+o < s * 08) { return new Color(0.0f, 0.0f, 1.0f); }
		else if (v+o < s * 09) { return new Color(0.5f, 0.0f, 1.0f); }
		else if (v+o < s * 10) { return new Color(1.0f, 0.0f, 1.0f); }
		else if (v+o < s * 11) { return new Color(1.0f, 0.0f, 0.5f); }
		else if (v+o < s * 12) { return new Color(1.0f, 0.0f, 0.0f); }
		else if (v+o < s * 13) { return new Color(1.0f, 0.5f, 0.0f); }
		else if (v+o < s * 14) { return new Color(1.0f, 1.0f, 0.0f); }
		return new Color(1.0f, 1.0f, .75f);
	}
	
	
	public Item() {
		stats = new Table();
		properties = new Table();
		strings = new StringMap();
		
		
		name = "Crystalized Error";
		desc = "Somewhere, something went wrong";
		type = "Loot";
		baseName = "";
		iconName = "";
		
	}
	
	public Item(string str) {
		stats = new Table();
		properties = new Table();
		strings = new StringMap();
		color = Color.white;
		
		name = "Crystalized Error";
		desc = "Somewhere, something went wrong";
		type = "Loot";
		baseName = "";
		iconName = "";
		color = Color.white;
		
		LoadFromString(str);
	}
	
	public Item Clone() {
		Item clone = new Item();
		clone.name = name;
		clone.baseName = baseName;
		clone.desc = desc;
		clone.type = type;
		clone.iconName = iconName;
		clone.stats = stats.Clone();
		clone.properties = properties.Clone();
		clone.strings = strings.Clone();
		
		return clone;
	}
	
	public static int Compare(Item a, Item b) {
		if (a.type == b.type) {
			if (a.rarity == b.rarity) { 
				return a.name.CompareTo(b.name);
			} else { return (int)(a.rarity - b.rarity); }
		} else { return a.type.CompareTo(b.type); }
	}
	
	public int CompareTo(Item other) { return Compare(this, other); }
	
	
	public override string ToString() { return ToString('|'); }
	public string ToString(char delim) {
		StringBuilder str = new StringBuilder("");
		str.Append(strings.ToString('`') + delim);
		str.Append(stats.ToLine(',') + delim);
		str.Append(properties.ToLine(',') + delim);
		return str.ToString();
	}
	
	
	
	public static Item FromString(string s) { return FromString(s, '|'); }
	public static Item FromString(string s, char delim) {
		Item it = new Item();
		it.LoadFromString(s, delim);
		return it;
	}
	
	public void LoadFromString(string s) { LoadFromString(s, '|'); }
	public void LoadFromString(string s, char delim) {
		string[] content = s.Split(delim);
		if (content.Length < 3) {
			Debug.LogWarning("Tried to load a malformed string as an item.\nDelim: " + delim + "\n" + s);
			return;
		}
		
		strings = 		content[0].ParseStringMap('`');
		stats = 		content[1].ParseTable(',');
		properties =	content[2].ParseTable(',');
		
	}
	
	
	
	public void Save(string key) { PlayerPrefs.SetString(key, ToString()); }
	
	public void Load(string key) {
		if (PlayerPrefs.HasKey(key)) {
			LoadFromString(PlayerPrefs.GetString(key));
		}
	}
	
	
}
