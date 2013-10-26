using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/**
Tables can be added, multiplied, and whatnot.
Each of these operations constructs a new table.
When tables (a, b) are added/subtracted, the new table has (a.Keys U b.Keys) elements.
When tables (a, b) are multiplied/divided, the new table has (a.Keys I b.Keys) elements
There are also functions to add a single value to the whole table (or multiply/divide)

As well as functions to do so randomly, making this quite a useful class to easily calculate stats
Desks are a Dictionary<string, Table>, which can be used to store all the calculations needed
To calculate various stats.

Example:
In an RPG, lets say we have a large list of stats. We split these up into two parts:
Base Stats:
STR, DEX, VIT
Combat Stats:
Health, Attack, Defense, Accuracy, Dodge, etc

What we can do, is we can construct a Desk to store the information on how to derive these stats.
Lets say the player has this table as its 'stats' table:
Table {
	"STR":10,
	"DEX":30,
	"VIT":15,
	"WIS":20
}

This desk might look something like this:
Desk {
	"Health" : Table { "STR":2, "VIT":5 },
	"Attack" : Table { "STR":4, "DEX":2 },
	"Defense" : Table { "VIT":2, "STR":1 }
}

Then we can simply loop through all of the keys of the desk, and create new tables
from multiplying the stats table with each of the 'instructional' tables.




*/

[System.Serializable]
public class Table : Dictionary<string, float> {
	public string[] strings;
	public float[] floats;

	
	public void Init() {
		Clear();
		for (int i = 0; i < Mathf.Min(strings.Length, floats.Length); i++) {
			this[strings[i]] = floats[i];
		}
	}
	
	public Table Clone() {
		Table d = new Table();
		foreach (string key in Keys) { d[key] = this[key]; }
		return d;
	}
	
	public new float this[string key] {
		get {
			Dictionary<string, float> goy = this;
			if (!goy.ContainsKey(key)) { return 0; }
			return goy[key];
		}
		
		set {
			Dictionary<string, float> goy = this;
			if (goy.ContainsKey(key)) { goy[key] = value; }
			else { goy.Add(key, value); }
		}
	}
	
	public static Table operator +(Table a, float b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] + b; }
		return c;
	}
	
	public static Table operator -(Table a, float b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] - b; }
		return c;
	}
	
	public static Table operator *(Table a, float b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] * b; }
		return c;
	}
	
	public static Table operator /(Table a, float b) {
		if (b == 0) { Debug.LogWarning("Trying to divide table by zero..."); return a; }
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] / b; }
		return c;
	}
	
	public static Table operator +(Table a, Table b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] += a[key]; }
		foreach (string key in b.Keys) { c[key] += b[key]; }
		return c;
	}
	
	public static Table operator -(Table a, Table b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] -= a[key]; }
		foreach (string key in b.Keys) { c[key] -= b[key]; }
		return c;
	}
	
	public static Table operator *(Table a, Table b) {
		Table c = a.Clone();
		foreach (string key in b.Keys) { 
			if (a.ContainsKey(key)) { c[key] *= b[key]; }
		}
		return c;
	}
	
	public static Table operator /(Table a, Table b) {
		Table c = a.Clone();
		foreach (string key in b.Keys) { 
			if (a.ContainsKey(key)) { c[key] /= b[key]; }
		}
		return c;
	}
	
	
	public void Update() {
		strings = new string[Count];
		floats = new float[Count];
		
		int i = 0;
		foreach (string key in Keys) {
			strings[i] = key;
			floats[i] = this[key];
			i++;
		}
	}
	
	public void Set() {
		Clear();
		int max = Mathf.Min(strings.Length, floats.Length);
		for (int i = 0; i < max; i++) {
			Add(strings[i], floats[i]);
		}
	}
	
	public new void Add(string s, float f) { this[s] = f; }
	public void Add(string s) { this[s] = 0; }
	public void Add(float f, string s) { this[s] = f; }
	
	public void Add(float f) { foreach (string s in Keys) { this[s] += f; } }
	public void Add(Table t) { foreach (string s in t.Keys) { this[s] += t[s]; } }
	
	public void Subtract(float f) { foreach (string s in Keys) { this[s] -= f; } }
	public void Subtract(Table t) { foreach (string s in t.Keys) { this[s] -= t[s]; } }
	
	public void AddRandomly(float f) { foreach (string s in Keys) { this[s] += f; } }
	public void AddRandomly(Table t) { foreach (string s in t.Keys) { this[s] += t[s] * Random.value; } }

	public void Multiply(float f) { foreach (string s in Keys) { this[s] *= f; } }
	public void Multiply(Table t) { 
		foreach (string s in Keys) {
			if (t.ContainsKey(s)) { this[s] *= t[s]; }
		}
	}
	
	public void Divide(float f) { foreach (string s in Keys) { this[s] /= f; } }
	public void Divide(Table t) { 
		foreach (string s in Keys) {
			if (t.ContainsKey(s) && t[s] != 0) { this[s] /= t[s]; }
		}
	}
	
	public override string ToString() {
		StringBuilder str = new StringBuilder("#Formatted Table as .csv:");
		foreach (string key in Keys) {
			str.Append("\n");
			str.Append(key);
			str.Append(",");
			str.Append(this[key]);
		}
		return str.ToString();
	}
	
	public void LoadCSV(string csv) {
		Clear();
		string[] lines = csv.Split('\n');
		
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i][0] == '#') { continue; }
			string[] content = lines[i].Split(',');
			for (int j = 0; j < content.Length; j += 2) {
				this[content[j]] = float.Parse(content[j+1]);
			}
		}
		
	}
	
	public void SaveToPlayerPrefs(string name) {
		int i = 0;
		PlayerPrefs.SetInt(name + "_count", Count);
		foreach (string k in Keys) { 
			PlayerPrefs.SetString(name + "_" + i + "_key", k);
			PlayerPrefs.SetFloat(name + "_" + i + "_float", this[k]);
			i++;
		}
	}
	
	public void LoadFromPlayerPrefs(string name) {
		if (!PlayerPrefs.HasKey(name + "_count")) { Debug.Log("Dictionary " + name + " does not exist in PlayerPrefs"); return; }
		int count = PlayerPrefs.GetInt(name + "_count");
		
		for (int i = 0; i < count; i++) {
			string key = PlayerPrefs.GetString(name + "_" + i + "_key");
			float val = PlayerPrefs.GetFloat(name + "_" + i + "_float");
			if (ContainsKey(key)) { this[key] = val; }
			else { Add(key, val); }
		}
	}
	
}

