using UnityEngine;
using System.Collections;


//Bool Min Max
//Common data for lots of randomization stuff
[System.Serializable]
public class BMM {
	public bool randomize = false;
	public float min = 0.9f;
	public float max = 1.1f;
	
	public float value {
		get {
			if (!randomize) { return 1.0f; }
			return Random.Range(min, max);
		}
	}
	 
	
	public BMM() {
		randomize = false;
		min = 0.9f;
		max = 1.1f;
	}
	
	public BMM(bool bb, float mmin, float mmax) {
		randomize = bb;
		min = mmin;
		max = mmax;
	}
	
}