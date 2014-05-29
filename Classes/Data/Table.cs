using UnityEngine;
using System;
using System.Reflection;
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
	
	////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//Constructors and Loading
	#region
	public Table() : base() {}
	public Table(string csv) : base() { LoadCSV(csv); }
	public Table(string csv, char separator) : base() { LoadCSV(csv, separator); }
	public Table(TextAsset textAsset) : base() { LoadCSV(textAsset.text); }
	public Table(TextAsset textAsset, char separator) : base() { LoadCSV(textAsset.text, separator); }
	public Table(Dictionary<string, float> source) : base() { 
		foreach (string s in source.Keys) { 
			this[s] = source[s];
		}
	}
	
	public static Table LoadTextAsset(string name) { return LoadTextAsset(name, ','); }
	public static Table LoadTextAsset(string name, char delim) {
		TextAsset asset = Resources.Load(name, typeof(TextAsset)) as TextAsset;
		if (asset == null) { return null; }
		return new Table(asset, delim);
	}
	
	public Table Clone() { return new Table(this); }
	public static Table CreateFromLine(string line) { return CreateFromLine(line, ','); }
	public static Table CreateFromLine(string line, char separator) { 
		Table tb = new Table();
		tb.LoadLine(line, separator);
		return tb;
	}
	
	public void LoadLine(string line) { LoadLine(line, ','); }
	public void LoadLine(string line, char separator) {
		Clear();
		string[] content = line.Split(separator);
		if (line.Length == 0) { 
			//Debug.LogWarning("Table.LoadLine passed blank string, Table cleared.");
			return;
		}
		
		for (int i = 0; i < content.Length; i += 2) {
			this[content[i]] = float.Parse(content[i+1]);
		}
		
	}
	
	public static Table CreateFromCSV(string csv) { return CreateFromCSV(csv, ','); }
	public static Table CreateFromCSV(string csv, char separator) {
		Table tb = new Table();
		tb.LoadCSV(csv, separator);
		return tb;
	}
	
	
	
	public void LoadCSV(string csv) { LoadCSV(csv, ','); }
	public void LoadCSV(string csv, char separator) {
		Clear();
		string[] lines = csv.Split('\n');
		
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i].Length < 3) { continue; }
			if (lines[i][0] == '#') { continue; }
			string[] content = lines[i].Split(separator);
			for (int j = 0; j < content.Length; j += 2) {
				this[content[j]] = float.Parse(content[j+1]);
			}
		}
		
	}
	#endregion
	
	
	////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//Accessor
	#region
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
	#endregion
	
	////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//Operators and Operator-like functions
	#region
	public static Table operator +(float a, Table b) { return b + a; }
	public static Table operator +(Table a, float b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] + b; }
		return c;
	}
	
	public static Table operator -(float a, Table b) { return b - a; }
	public static Table operator -(Table a, float b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] - b; }
		return c;
	}
	
	public static Table operator *(float a, Table b) { return b * a; }
	public static Table operator *(Table a, float b) {
		Table c = new Table();
		foreach (string key in a.Keys) { c[key] = a[key] * b; }
		return c;
	}
	
	public static Table operator /(float a, Table b) { return b / a; }
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
		foreach (string key in a.Keys) { c[key] += a[key]; }
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
	
	
	public new void Add(string s, float f) { this[s] = f; }
	public void Add(float f, string s) { this[s] = f; }
	public void Add(string s) { this[s] = 0; }
	
	public void Add(float f) { foreach (string s in Keys) { this[s] += f; } }
	public void Add(Table t) { foreach (string s in t.Keys) { this[s] += t[s]; } }
	
	public void Subtract(float f) { foreach (string s in Keys) { this[s] -= f; } }
	public void Subtract(Table t) { foreach (string s in t.Keys) { this[s] -= t[s]; } }
	
	public void AddRandomly(float f) { foreach (string s in Keys) { this[s] += f * RandomF.value; } }
	public void AddRandomly(Table t) { foreach (string s in t.Keys) { this[s] += t[s] * RandomF.value; } }
	
	public void AddRandomNormal(float f) { foreach (string s in Keys) { this[s] += f * RandomF.normal; } }
	public void AddRandomNormal(Table t) { foreach (string s in t.Keys) { this[s] += t[s] * RandomF.normal; } }

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
	
	public Table Mask(string mask) { return Mask(mask, ','); }
	public Table Mask(string mask, char delim) { return Mask(mask.Split(delim)); }
	public Table Mask(string[] fields) {
		Table t = new Table();
		foreach (string field in fields) {
			if (this[field] != 0) { t[field] = this[field]; }
		}
		return t;
	}
	
	#endregion
	
	////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//Support for containing certain data types
	#region
	public Set<string> FieldKeys {
		get {
			Set<string> ukeys = new Set<string>();
			
			foreach (string s in Keys) {
				int index = s.LastIndexOf('.');
				if (index < 0) { ukeys.Add(s); continue; }
				
				string k = s.Substring(0, index);
				if (s.Length - k.Length <= 2) { ukeys.Add(k); }
				else { ukeys.Add(s); }
				
			}
			
			return ukeys;
		}
	}
	
	//Quick check functions
	public bool ContainsColorQ(string s) { return ContainsKey(s+".r"); }
	public bool ContainsVector2Q(string s) { return ContainsKey(s+".y"); }
	public bool ContainsVector3Q(string s) { return ContainsKey(s+".z"); }
	
	//Full check functions
	public bool ContainsColor(string s) { return ContainsKey(s+".r") && ContainsKey(s+".g") && ContainsKey(s+".b") && ContainsKey(s+".a"); }
	public bool ContainsVector2(string s) { return ContainsKey(s+".x") && ContainsKey(s+".y"); }
	public bool ContainsVector3(string s) { return ContainsKey(s+".x") && ContainsKey(s+".y") && ContainsKey(s+".z"); }
		
	public Color GetColor(string s) { return new Color(this[s+".r"], this[s+".g"], this[s+".b"], this[s+".a"]); }
	public Vector2 GetVector2(string s) { return new Vector2(this[s+".x"], this[s+".y"]); }
	public Vector3 GetVector3(string s) { return new Vector3(this[s+".x"], this[s+".y"], this[s+".z"]); }
	
	public void SetColor(string s, Color c) { 
		this[s+".r"] = c.r;
		this[s+".g"] = c.g;
		this[s+".b"] = c.b;
		this[s+".a"] = c.a;
	}
	
	public void SetVector3(string s, Vector3 v) {
		this[s+".x"] = v.x;
		this[s+".y"] = v.y;
		this[s+".z"] = v.z;
	}
	
	public void SetVector2(string s, Vector2 v) {
		this[s+".x"] = v.x;
		this[s+".y"] = v.y;
	}
	#endregion
	
	
	
	////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////
	//Misc functions
	public override string ToString() { return ToString(','); }
	public string ToString(char delim) {
		StringBuilder str = new StringBuilder("#Formatted Table as .csv:");
		foreach (string key in Keys) {
			str.Append("\n");
			str.Append(key);
			str.Append(delim);
			str.Append(this[key]);
		}
		return str.ToString();
	}
	
	public string ToLine() { return ToLine(','); }
	public string ToLine(char delim) {
		StringBuilder str = new StringBuilder();
		foreach (string key in Keys) {
			if (str.Length > 0) { str.Append(delim); }
			str.Append(key);
			str.Append(delim);
			str.Append(this[key]);
		}
		//Debug.Log("ToLine: [" + str.ToString() + "]");
		return str.ToString();
	}
	
	
	public float Sum() {
		float f = 0;
		foreach (string key in Keys) { f += this[key]; }
		return f;
	}
	
	
	public void Set(Table t) {
		foreach (string s in t.Keys) { this[s] = t[s]; }
	}
	
	public void Save(string name) {
		PlayerPrefs.SetString(name, ToString());
	}
	
	public void Load(string name) {
		string str = PlayerPrefs.GetString(name);
		if (str.Length > "#Formatted Table as .csv:".Length) {
			LoadCSV(str);
		} else {
			Debug.Log("Unable to load Table from CSV from player pref: " + name + "\nLoaded:\n" + str);
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

public class ConvertsToTable {

	public Table asTable {
		get { return this.ToTable(); }
		set { this.SetTable(value); }
	}
	
}


public static class TableHelper {
	public static Table ToTable(this object obj) {
		Table t = new Table();
			
		FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
		for (int i = 0; i < fields.Length; i++) {
			FieldInfo field = fields[i];
			if (field.FieldType == typeof(float)) {
				t[field.Name] = (float) field.GetValue(obj);
			}
			
			if (field.FieldType == typeof(double)) {
				t[field.Name] = (float) (double) field.GetValue(obj);
				
			}
			
			if (field.FieldType == typeof(int)) {
				t[field.Name] = (float) (int) field.GetValue(obj);
			}
				
			if (field.FieldType == typeof(bool)) {
				t[field.Name] = ((bool)field.GetValue(obj)) ? 1f : 0f;
			}
			
			if (field.FieldType == typeof(Vector2)) {
				t.SetVector2(field.Name, (Vector2)field.GetValue(obj));
			}
			
			if (field.FieldType == typeof(Vector3)) {
				t.SetVector3(field.Name, (Vector3)field.GetValue(obj));
			}
			
			if (field.FieldType == typeof(Color)) {
				t.SetColor(field.Name, (Color)field.GetValue(obj));
			}
			
			
		}
		
		return t;
		
	}
	
	
	public static void SetTable(this object obj, Table table) {
		foreach (string s in table.FieldKeys) {
			FieldInfo field = obj.GetType().GetField(s, BindingFlags.Public | BindingFlags.Instance);
			if (field != null) {
				if (field.FieldType == typeof(float)) {
					field.SetValue(obj, table[s]);
					continue;
				}
				
				if (field.FieldType == typeof(double)) {
					field.SetValue(obj, (double)table[s]);
					continue;
				}
				
				if (field.FieldType == typeof(int)) {
					field.SetValue(obj, (int)table[s]);
					continue;
				}
				
				if (field.FieldType == typeof(bool)) {
					field.SetValue(obj, (table[s] == 1f) ? true : false);
					continue;
				}
				
				if (field.FieldType == typeof(Vector2)) {
					field.SetValue(obj, table.GetVector2(s));
				}
				
				if (field.FieldType == typeof(Vector3)) {
					field.SetValue(obj, table.GetVector3(s));
				}
				
				if (field.FieldType == typeof(Color)) {
					field.SetValue(obj, table.GetColor(s));
				}
				
			}
		}
	}
	
}


