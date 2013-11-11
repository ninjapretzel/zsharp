using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataF {
	
	public static T Choose<T>(this List<T> list) {
		return list[(int)(RandomF.value * list.Count)];
	}
	
	public static T Choose<T>(this List<T> list, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, list.Count-1);
		return list[index];
	}
	
	public static T Choose<T>(this T[] array) {
		return array[(int)(RandomF.value * array.Length)];
	}
	
	public static T Choose<T>(this T[] array, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, array.Length-1);
		return array[index];
	}
	
	public static List<T> Clone<T>(this List<T> list) {
		List<T> clone = new List<T>();
		foreach (T s in list) { clone.Add(s); }
		return clone;
	}
	
	public static void Append<T>(this List<T> list, List<T> add) {
		foreach (T o in add) {
			list.Add(o);
		}
	}
	
	
	//Returns the lines as a string array from a csv formatted TextAsset (.txt)
	//Removes all tabs and splits the file by newlines. 
	public static string Load(string filename) {
		TextAsset file = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (file == null) { 
			Debug.Log("Tried to load " + filename + ".txt. File does not exist");
			Debug.Log("If the file is " + filename + ".csv - Fix file extension to .txt");
			return null;
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
