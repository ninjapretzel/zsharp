using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class RandomF {
	
	private static Stack<int> seedStack;
	
	public static void Push(int seed) {
		if (seedStack == null) { seedStack = new Stack<int>(); }
		seedStack.Push(Random.seed);
		Random.seed = seed;
	}
	
	public static int Pop() {
		int ret = Random.seed;
		if (seedStack.Count > 0) { 
			Random.seed = seedStack.Pop();
		} else {
			Debug.Log("RandomF : Tried to pop seed when no seed was present");
		}
		return ret;
	}
	
	public static Vector3 insideUnitCube {
		get {
			return new Vector3(Range(-.5f, .5f), 
								Range(-.5f, .5f),
								Range(-.5f, .5f));
		}
	}
	
	
	public static int Range(int a, int b) { return (a + (int)((float)(b-a) * value)); }
	public static float Range(float a, float b) { return (a + (b-a) * value); }
	
	public static float value { get { return Random.value * .9999999f; } }
	public static float unit { get { return -1 + (2 * RandomF.value); } }
	public static float normal { get { return (value + value + value) / 3.0f; } }
	
	public static char alpha { get { return (char)((int)'a'+(int)(value * 26)); } }
	public static char lowercase { get { return (char)((int)'a'+(int)(value * 26)); } }
	public static char uppercase { get { return (char)((int)'A'+(int)(value * 26)); } }
	public static char numeric { get { return (char)((int)'0'+(int)(value * 10)); } }
	
	public static float Normal(float min, float max) { 
		float distance = max - min;
		float center = min + distance;
		return center + distance * normal;
	}
	
	public static int WeightedChoose(float[] weights) {
		float total = 0;
		int i;
		for (i = 0; i < weights.Length; i++) { total += weights[i]; }
		
		float choose = value * total; 
		float check = 0;
		for (i = 0; i < weights.Length; i++) {
			check += weights[i];
			if (choose < check) { return i; }
		}
		return weights.Length-1;
	}
	
	public static int WeightedChoose(List<float> weights) {
		float total = 0;
		int i;
		for (i = 0; i < weights.Count; i++) { total += weights[i]; }
		
		float choose = value * total; 
		float check = 0;
		for (i = 0; i < weights.Count; i++) {
			check += weights[i];
			if (choose < check) { return i; }
		}
		return weights.Count-1;
	}
	
	
}
