using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataF {
	
	public static float ToFloat(this byte[] b, int i) { return BitConverter.ToSingle(b, i); }
	public static int ToInt(this byte[] b, int i) { return BitConverter.ToInt32(b, i); }
	
	 public static T DeepCopy<T>(T obj) {
		MemoryStream ms = new MemoryStream();
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(ms, obj);
		ms.Seek(0, SeekOrigin.Begin);
		T retval = (T)bf.Deserialize(ms);
		ms.Close();
		return retval;
	}
	
	public static Vector3 ToVector3(this byte[] b, int i) {
		Vector3 v = Vector3.zero;
		v.x = b.ToFloat(i);
		v.y = b.ToFloat(i+4);
		v.z = b.ToFloat(i+8);
		return v;
	}
	
	public static Quaternion ToQuaternion(this byte[] b, int i) {
		Quaternion q = Quaternion.identity;
		q.x = b.ToFloat(i);
		q.y = b.ToFloat(i+4);
		q.z = b.ToFloat(i+8);
		q.w = b.ToFloat(i+12);
		return q;
	}
	
	//Quick stupid accessor functions
	public static T LastElement<T>(this List<T> list) { if (list.Count == 0) { return default(T); } return list[list.Count-1]; }
	public static T FirstElement<T>(this List<T> list) { return list[0]; }
	
	//Get the nth element from the end of the list
	public static T FromEnd<T>(this List<T> list, int offset) { return list[list.Count-1-offset]; }
	
	//Add a list to the end of this list.
	public static void Append<T>(this List<T> list, List<T> add) { foreach (T o in add) { list.Add(o); } }
	public static void Append<T>(this List<T> list, T[] add) { foreach (T o in add) { list.Add(o); } }
	
	//Get a random valid index
	public static int RandomIndex<T>(this List<T> list) { return (int)(RandomF.value * list.Count); }
	
	//Choose a random element from the list
	public static T Choose<T>(this List<T> list) { return list[list.RandomIndex()]; }
	
	//Choose an element from the list using weights
	public static T Choose<T>(this List<T> list, float[] weights) {
		int index = (int)Mathf.Clamp(RandomF.WeightedChoose(weights), 0, list.Count-1);
		return list[index];
	}
	
	
	
	//Choose 'num' elements from the list
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
	
	//Generate a shuffled version of the list
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
	
	//Swap two elements
	public static void Swap<T>(this List<T> list, int a, int b) {
		T temp = list[b];
		list[b] = list[a];
		list[a] = temp;
	}
	
	//Generate a new list which has all instances of 'toRemove' removed from it.
	public static List<T> RemoveAll<T>(this List<T> list, T toRemove) {
		List<T> l = new List<T>();
		for (int i = 0; i < list.Count; i++) {
			if (!list[i].Equals(toRemove)) {
				l.Add(list[i]);
			}
		}
		return l;
	}
	
	//Generate an array of strings from a list of objects
	public static string[] ToStringArray<T>(this List<T> list) {
		List<string> strings = new List<string>();
		for (int i = 0; i < list.Count; i++) { strings.Add(list[i].ToString()); }
		return strings.ToArray();
	}
	
	//Generate a list of strings from a list of objects
	public static List<string> ToStringList<T>(this List<T> list) {
		List<string> strings = new List<string>();
		for (int i = 0; i < list.Count; i++) { strings.Add(list[i].ToString()); }
		return strings;
	}
	
	//Generate an array of strings from an array of objects
	public static string[] ToStringArray<T>(this T[] list) {
		string[] strings = new string[list.Length];
		for (int i = 0; i < list.Length; i++) { strings[i] = list[i].ToString(); }
		return strings;
	}
	
	//Create another list containing all of the elements in the same order as another list
	public static List<T> Clone<T>(this List<T> list) {
		List<T> clone = new List<T>();
		for (int i = 0; i < list.Count; i++) { clone.Add(list[i]); }
		return clone;
	}
	
	//Quick stupid accessors for arrays
	public static T LastElement<T>(this T[] list) { return list[list.Length-1]; }
	public static T FirstElement<T>(this T[] list) { return list[0]; }
	
	//Grab the nth element from the end of the list
	public static T FromEnd<T>(this T[] list, int offset) { return list[list.Length-1-offset]; }
	
	//Get a random valid index
	public static int RandomIndex<T>(this T[] array) { return (int)(RandomF.value * array.Length); }
	
	//Choose a random element from an array
	public static T Choose<T>(this T[] array) { return array[array.RandomIndex()]; }
	
	//Choose an element from an array using weights
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
	
	public static void SaveToFile(this byte[] b, string path) {
		Directory.CreateDirectory(path.PreviousDirectory());
		
		File.WriteAllBytes(path, b);
		
		
	}
	
	public static byte[] LoadBytesFromFile(string path) {
		if (File.Exists(path)) {
			return File.ReadAllBytes(path);
		}
		return null;
		
	}
	
	
	
}
