using UnityEngine;
using System.Collections;
using System.Text;

[System.Serializable]
public class Item {
	public string name = "Crystalized Error";
	public string baseName = "";
	public string desc = "Somewhere, something went wrong";
	public string type = "Loot";
	public string iconName = "";
	public Texture2D iconLoaded;
	public Color color;
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
	
	public bool locked { get { return properties["locked"] == 1; } set { properties["locked"] = value ? 1 : 0; } }
	public bool stacks { get { return properties["stacks"] == 1; } set { properties["stacks"] = value ? 1 : 0; } }
	public bool equip { get { return properties["equip"] == 1; } set { properties["equip"] = value ? 1 : 0; } }
	
	public int count { get { return (int)properties["count"]; } set { properties["count"] = value; } }
	public int equipSlot { get { return (int)properties["equipSlot"]; } set { properties["equipSlot"] = value; } }
	
	public float value { get { return properties["value"]; } set { properties["value"] = value; } }
	public float rarity { get { return properties["rarity"]; } set { properties["rarity"] = value; } }
	public float quality { get { return properties["quality"]; } set { properties["quality"] = value; } }
	public float speed { get { return properties["speed"]; } set { properties["speed"] = value; } }
	public float mpCost { get { return properties["mpCost"]; } set { properties["mpCost"] = value; } }
	public float power { get { return properties["power"]; } set { properties["power"] = value; } }
	public float size { get { return properties["size"]; } set { properties["size"] = value; } }
	//public float xxxx { get { return properties["xxxx"]; } set { properties["xxxx"] = value; } }
	
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
		
	}
	
	public Item Clone() {
		Item clone = new Item();
		clone.name = name;
		clone.baseName = baseName;
		clone.desc = desc;
		clone.type = type;
		clone.iconName = iconName;
		clone.color = color;
		clone.stats = stats.Clone();
		clone.properties = properties.Clone();
		return clone;
	}
	
	
	
	public override string ToString() { return ToString('|'); }
	public string ToString(char delim) {
		StringBuilder str = new StringBuilder("");
		str.Append(name + delim);
		str.Append(baseName + delim);
		str.Append(desc + delim);
		str.Append(type + delim);
		str.Append(iconName + delim);
		str.Append(color.ToString(',') + delim);
		str.Append(stats.ToString(',') + delim);
		str.Append(properties.ToString(',') + delim);
		return str.ToString();
	}
	
	public void Load(string s, char delim) {
		string[] content = s.Split(delim);
	}
	
	
}
