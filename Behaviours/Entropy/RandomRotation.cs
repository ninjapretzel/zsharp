using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour {
	public bool x = false;
	public bool y = false;
	public bool z = false;
	
	public Vector3 min = Vector3.zero;
	public Vector3 max = Vector3.one;
	
	public bool onAwake = false;
	public bool useSeed = false;
	public static int seed = 12236921;
	
	void Awake() {
		if (onAwake) { SetRotation(); }
	}
	
	void Start() {
		SetRotation();
	}
	
	void SetRotation() {
		int prevSeed = Random.seed;
		if (useSeed) { Random.seed = seed++; }
		
		Vector3 rotation = transform.rotation.eulerAngles;
		if (x) { rotation.x = Random.Range(min.x, max.x); }
		if (y) { rotation.y = Random.Range(min.y, max.y); }
		if (z) { rotation.z = Random.Range(min.z, max.z); }
		transform.rotation = Quaternion.Euler(rotation);
		
		if (useSeed) { Random.seed = prevSeed; }
		Destroy(this);
	}
	
}
