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
			return new Vector3(Random.Range(-.5f, .5f), 
								Random.Range(-.5f, .5f),
								Random.Range(-.5f, .5f));
		}
	}
	
	public static float value {
		get { return Random.value * .99999999f; }
	}
	
	public static float normal {
		get { return (Random.value + Random.value + Random.value) / 3.0f; }
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
	
	
}
