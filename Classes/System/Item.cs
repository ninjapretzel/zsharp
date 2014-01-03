using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class Inventory : List<Item> {

	public static Inventory database;
	
	static Inventory() {
		database = new Inventory();
		TextAsset file = Resources.Load("Items", typeof(TextAsset)) as TextAsset;
		if (file != null) {
			string[] lines = file.text.Split('\n');
			database = LoadLines(lines);
			
		}
		
		
	}
	
	public Item GetNamed(string name) {
		foreach (Item i in this) { if (i.name == name) { return i; } }
		return null;
	}
	
	
	public List<Item> SelectType(string type) {
		List<Item> list = new List<Item>();
		foreach (Item i in this) {
			if (i.type == type) { list.Add(i); }
		}
		return list;
	}
	
	public List<Item> SelectOverRarity(float rarity) {
		List<Item> list = new List<Item>();
		foreach (Item i in this) {
			if (i.rarity > rarity) { list.Add(i); }
		}
		return list;
	}
	
	public List<Item> SelectUnderRarity(float rarity) {
		List<Item> list = new List<Item>();
		foreach (Item i in this) {
			if (i.rarity < rarity) { list.Add(i); }
		}
		return list;
	}
	
	
	public static Inventory LoadLines(string[] lines) {
		Inventory items = new Inventory();
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i].Length <= 3) { continue; }
			if (lines[i][0] == '#') { continue; }
			items.Add(Item.FromString(lines[i]));
		}
		return items;
	}
	
	
}


[System.Serializable]
public class Item : IComparable<Item> {
	public string name = "Crystalized Error";
	public string baseName = "";
	public string desc = "Somewhere, something went wrong";
	public string type = "Loot";
	public string iconName = "";
	public Texture2D iconLoaded;
	public Table stats;
	public Table properties;
	
	public void ReloadIcon() { iconLoaded = Resources.Load(iconName, typeof(Texture2D)) as Texture2D; }
	public Texture2D icon {
		get {
			if (iconLoaded != null) { return iconLoaded; }
			Texture2D i = Resources.Load(iconName, typeof(Texture2D)) as Texture2D;
			if (i != null) { iconLoaded = i; }
			return i;
		}
	}
	
	public Color color { get { return properties.GetColor("color"); } set { properties.SetColor("color", value); }  }
	public bool locked { get { return properties["locked"] == 1; } set { properties["locked"] = value ? 1 : 0; } }
	public bool stacks { get { return properties["stacks"] == 1; } set { properties["stacks"] = value ? 1 : 0; } }
	public bool equip { get { return properties["equip"] == 1; } set { properties["equip"] = value ? 1 : 0; } }
	
	
	public int count { get { return (int)properties["count"]; } set { properties["count"] = value; } }
	public int maxStack { get { return (int)properties["maxStack"]; } set { properties["maxStack"] = value; } }
	public int equipSlot { get { return (int)properties["equipSlot"]; } set { properties["equipSlot"] = value; } }
	
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
		color = Color.white;
		
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
		str.Append(name + delim);
		str.Append(baseName + delim);
		str.Append(desc + delim);
		str.Append(type + delim);
		str.Append(iconName + delim);
		str.Append(stats.ToLine(',') + delim);
		str.Append(properties.ToLine(',') + delim);
		return str.ToString();
	}
	
	public static List<Item> LoadLinesAsList(string[] lines) {
		return (List<Item>)Inventory.LoadLines(lines);
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
		if (content.Length < 8) {
			Debug.LogWarning("Tried to load a malformed string as an item.\nDelim: " + delim + "\n" + s);
			return;
		}
		
		name = 			content[0];
		baseName = 		content[1];
		desc = 			content[2];
		type = 			content[3];
		iconName = 		content[4];
		stats = 		content[5].ParseTable(',');
		properties =	content[6].ParseTable(',');
		
	}
	
	
	
	public void Save(string key) { PlayerPrefs.SetString(key, ToString()); }
	
	public void Load(string key) {
		if (PlayerPrefs.HasKey(key)) {
			LoadFromString(PlayerPrefs.GetString(key));
		}
	}
	
	
}
