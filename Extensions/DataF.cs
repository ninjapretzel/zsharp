using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataF {
	
	public static Object Choose(this Object[] objs) {
		return (objs[(int)(Random.value * objs.Length)]);
	}
	
	public static Object Choose(this Object[] objs, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, objs.Length-1);
		return objs[index];
	}
	
	public static string Choose(this string[] strs) {
		return (strs[(int)(Random.value * strs.Length)]);
	}
	
	public static string Choose(this string[] strs, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, strs.Length-1);
		return strs[index];
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
