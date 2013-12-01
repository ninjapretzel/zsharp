using UnityEngine;
using System.Collections;

public class AutoMoveEntropy : MonoBehaviour {
	public static int seed = 123412341;
	public bool x;
	public bool y;
	public bool z;
	public Vector3 min;
	public Vector3 max;
	
	public bool useSeed;
	public bool normalDist;
	
	void Start() {
		int oldSeed = Random.seed;
		if (useSeed) { Random.seed = seed; }
		
		AutoMove mover = GetComponent<AutoMove>();
		if (mover != null) {
			Vector3 v = mover.velocity;
			v.x *= Eval(x, min.x, max.x);
			v.y *= Eval(y, min.y, max.y);
			v.z *= Eval(z, min.z, max.z);
			mover.velocity = v;
			
			
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
