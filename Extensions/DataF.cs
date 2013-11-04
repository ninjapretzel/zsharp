using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataF {
	
	public static Object Choose(this List<Object> list) {
		return list[(int)(RandomF.value * list.Count)];
	}
	
	public static string Choose(this List<string> list) {
		return list[(int)(RandomF.value * list.Count)];
	}
	
	
	public static Object Choose(this Object[] array) {
		return array[(int)(RandomF.value * array.Length)];
	}
	
	public static Object Choose(this Object[] array, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, array.Length-1);
		return array[index];
	}
	
	public static string Choose(this string[] array) {
		return array[(int)(RandomF.value * array.Length)];
	}
	
	public static string Choose(this string[] array, float[] weights) {
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
