using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class ZScript {
	public static Table data;
	public static Table flags;
	
	static ZScript() {
		data = new Table();
		flags = new Table();
		
	}
	
	public static float Get(string name) {
		if (data.ContainsKey(name)) { return data[name]; }
		return flags[name];
	}
	
	public static void SetData(string name, float val) { data[name.ToLower()] = val; }
	public static float GetData(string name) { return data[name.ToLower()]; }
	public static void SetValue(string name, float val) { data[name.ToLower()] = val; }
	public static float GetValue(string name) { return data[name.ToLower()]; }
	
	public static void Add(string name) { Add(name.ToLower(), 1); }
	public static void Add(string name, float val) { data[name.ToLower()] += val; }
	public static void Add(Table t) { foreach(string s in t.Keys) { data[s.ToLower()] += t[s];} }
	
	public static void Sub(string name) { Sub(name.ToLower(), 1); }
	public static void Sub(string name, float val) { data[name.ToLower()] -= val; }
	public static void Sub(Table t) { foreach(string s in t.Keys) { data[s.ToLower()] -= t[s];} }
	
	
	
	public static bool HasAtLeast(string name, float val) { return data[name] >= val; }
	public static bool HasAtLeast(Table t) {
		foreach (string s in t.Keys) { if (!HasAtLeast(s, t[s])) { return false; } }
		return true;
	}
	
	
	public static void SetFlag(string flag, bool state) { flags[flag] = state ? 1 : 0; } 
	public static void SetFlag(string flag) { flags[flag] = 1; }
	public static bool GetFlag(string flag) { return flags[flag] == 1; }
	public static void UnsetFlag(string flag) { flags.Remove(flag); }
	
	
	public static void Save(string slot) {
		data.Save(slot + "_data");
		flags.Save(slot + "_flags");
	}
	
	public static void Load(string slot) {
		data.Load(slot + " _data");
		flags.Load(slot + "_flags");
	}
	
	
}