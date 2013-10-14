using UnityEngine;
using System.Collections;

public static class ZScript {
	public static Table values;
	public static Table flags;

	public static void SetFlag(string name) { flags[name] = 1; }
	public static void UnsetFlag(string name) { flags.Remove(name); }
	public static bool GetFlag(string name) { return (flags[name] == 1); }
	
	public static void SetValue(string name, float value) { values[name] = value; }
	public static float GetValue(string name) { return values[name]; }
	
	
	
	
}
