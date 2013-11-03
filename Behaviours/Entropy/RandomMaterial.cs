using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomMaterial : MonoBehaviour {
	public Material[] materials;
	public float[] weights;
	
	public bool onAwake = false;
	public bool useSeed = false;
	public static int seed = 125623;
	
	void Awake() {
		if (onAwake) { SetMaterial(); }
	}
	
	void Start() {
		SetMaterial();
	}
	
	void SetMaterial() { 
		int index = RandomF.WeightedChoose(weights);
		
		renderer.material = materials[index];
		
		Destroy(this);
	}
	
}
