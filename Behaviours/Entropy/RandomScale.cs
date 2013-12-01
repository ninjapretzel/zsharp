using UnityEngine;
using System.Collections;

public class RandomScale : MonoBehaviour {
	public Vector3 min = Vector3.one;
	public Vector3 max = Vector3.one;
	public bool uniformXY;
	public bool uniformXZ;
	
	public bool onAwake = false;
	public bool useSeed = false;
	public static int seed = 1234211;

	void Awake() {
		if (onAwake) { SetScales(); }
	}
	
	void Start() {
		SetScales();
	}
	
	void SetScales() {
		int prevSeed = Random.seed;
		if (useSeed) { Random.seed = seed++; }

		Vector3 scales = Vector3.zero;
		
		scales.x = Random.Range(min.x, max.x);
		
		if (uniformXY) { scales.y = scales.x; }
		else { scales.y = Random.Range(min.y, max.y); }
		
		if (uniformXZ) { scales.z = scales.x; }
		else { scales.z = Random.Range(min.z, max.z); }
		
		transform.localScale = Vector3.Scale(transform.localScale, scales);
		
		if (useSeed) { Random.seed = prevSeed; }
		Destroy(this);
	}
	
}
