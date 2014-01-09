using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class DataF {
	
	public static T LastElement<T>(this List<T> list) { if (list.Count == 0) { return default(T); } return list[list.Count-1]; }
	public static T FromEnd<T>(this List<T> list, int offset) { return list[list.Count-1-offset]; }
	public static T FirstElement<T>(this List<T> list) { return list[0]; }
	
	public static void Append<T>(this List<T> list, List<T> add) { foreach (T o in add) { list.Add(o); } }
	public static int RandomIndex<T>(this List<T> list) { return (int)(RandomF.value * list.Count); }
	public static T Choose<T>(this List<T> list) { return list[list.RandomIndex()]; }
	public static T Choose<T>(this List<T> list, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, list.Count-1);
		return list[index];
	}
	public static List<T> Shuffled<T>(this List<T> list) {
		List<T> stuff = list.Clone();
		List<T> shuffled = new List<T>();
		for (int i = 0; i < list.Count; i++) {
			int index = stuff.RandomIndex();
			shuffled.Add(stuff[index]);
			stuff.RemoveAt(index);
		}
		return shuffled;
	}
	public static void Swap<T>(this List<T> list, int a, int b) {
		T temp = list[b];
		list[b] = list[a];
		list[a] = temp;
	}
	public static List<T> RemoveAll<T>(this List<T> list, T toRemove) {
		List<T> l = new List<T>();
		for (int i = 0; i < list.Count; i++) {
			if (!list[i].Equals(toRemove)) {
				l.Add(list[i]);
			}
		}
		return l;
	}
	
	public static List<string> ToStringArray<T>(this List<T> list) {
		List<string> strings = new List<string>();
		for (int i = 0; i < list.Count; i++) { strings.Add(list[i].ToString()); }
		return strings;
	}
	
	public static string[] ToStringArray<T>(this T[] list) {
		string[] strings = new string[list.Length];
		for (int i = 0; i < list.Length; i++) { strings[i] = list[i].ToString(); }
		return strings;
	}
	
	
	public static List<T> Choose<T>(this List<T> list, int num) {
		if (num >= list.Count) { return list.Shuffled(); }
		List<T> stuff = list.Clone();
		List<T> chosen = new List<T>();
		for (int i = 0; i < num; i++) {
			int index = stuff.RandomIndex();
			chosen.Add(stuff[index]);
			stuff.RemoveAt(index);
		}
		
		return chosen;
	}
	public static List<T> Clone<T>(this List<T> list) {
		List<T> clone = new List<T>();
		foreach (T s in list) { clone.Add(s); }
		return clone;
	}
	
	public static T LastElement<T>(this T[] list) { return list[list.Length-1]; }
	public static T FromEnd<T>(this T[] list, int offset) { return list[list.Length-1-offset]; }
	public static T FirstElement<T>(this T[] list) { return list[0]; }
	
	public static int RandomIndex<T>(this T[] array) { return (int)(RandomF.value * array.Length); }
	public static T Choose<T>(this T[] array) { return array[array.RandomIndex()]; }
	public static T Choose<T>(this T[] array, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, array.Length-1);
		return array[index];
	}
	
	
	//Returns the lines as a string array from a csv formatted TextAsset (.txt)
	public static string Load(string filename) {
		TextAsset file = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (file == null) { 
			Debug.Log("Tried to load " + filename + ".txt/" + filename + ".csv - File does not exist");
			return "";
		}
		return file.text.Replace("\t", "");
	}
	
	//Removes all tabs and splits the file by newlines. 
	public static string[] LoadLines(string filename) {
		return Load(filename).Replace("\t", "").Split('\n');
	}
	
	//Further converts it into a list by newlines and ','
	public static List<string> LoadList(string filename) {
		string text = Load(filename).ConvertNewlines().Replace(",\n","\n").Replace("\n", ",");
		return text.Split(',').ToList();
		
	}
	
	
}
