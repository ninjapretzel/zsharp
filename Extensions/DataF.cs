using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataF {
	
	
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
	
	
	public static int RandomIndex<T>(this T[] array) { return (int)(RandomF.value * array.Length); }
	public static T Choose<T>(this T[] array) { return array[array.RandomIndex()]; }
	public static T Choose<T>(this T[] array, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, array.Length-1);
		return array[index];
	}

	
	
	//Returns the lines as a string array from a csv formatted TextAsset (.txt)
	//Removes all tabs and splits the file by newlines. 
	public static string Load(string filename) {
		TextAsset file = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (file == null) { 
			Debug.Log("Tried to load " + filename + ".txt. File does not exist");
			Debug.Log("If the file is " + filename + ".csv - Fix file extension to .txt");
			return "";
		}
		return file.text.Replace("\t", "");
	}
	
	public static string[] LoadLines(string filename) {
		TextAsset file = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (file == null) { 
			Debug.Log("Tried to load " + filename + ".txt. File does not exist");
			Debug.Log("If the file is " + filename + ".csv - Fix file extension to .txt");
			return null;
		}
		return file.text.Replace("\t", "").Split('\n');
	}
	
	
	
}
