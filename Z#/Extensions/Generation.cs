using UnityEngine;
using System.Collections;

public static class Generation {

	public static int WeightedChoose(float[] weights) {
		float total = 0;
		int i;
		for (i = 0; i < weights.Length; i++) { total += weights[i]; }
		
		float choose = Random.value * total * .9999f; //offset the value slightly because of the range of random being [0, 1] instead of [0, 1)
		float check = 0;
		for (i = 0; i < weights.Length; i++) {
			check += weights[i];
			if (choose < check) { return i; }
		}
		return weights.Length-1;
	}
	
}
