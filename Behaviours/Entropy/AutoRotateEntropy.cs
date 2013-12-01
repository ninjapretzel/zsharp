using UnityEngine;
using System.Collections;

public class AutoRotateEntropy : MonoBehaviour {
	public static int seed = 125423511;
	public bool x;
	public bool y;
	public bool z;
	public Vector3 min;
	public Vector3 max;
	
	public bool useSeed = false;
	public bool normalDist = false;
	
	void Start() {
		int oldSeed = Random.seed;
		if (useSeed) { Random.seed = seed; }
		
		AutoRotate rotater = GetComponent<AutoRotate>();
		if (rotater != null) {
			Vector3 v = rotater.speed;
			v.x *= Eval(x, min.x, max.x);
			v.y *= Eval(y, min.y, max.y);
			v.z *= Eval(z, min.z, max.z);
			rotater.speed = v;
			
			
		}
		
		if (useSeed) { Random.seed = oldSeed; seed++; }
		Destroy(this);
	}

	float Eval(bool b, float min, float max) {
		if (!b) { return 1.0f; }
		if (normalDist) { return RandomF.Normal(min, max); }
		return RandomF.Range(min, max);
	}
}
