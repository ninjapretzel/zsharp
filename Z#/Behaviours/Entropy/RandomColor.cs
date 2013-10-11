using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {
	public Color one;
	public Color two;
	public string target;
	
	public bool onAwake = false;
	public bool useSeed = false;
	public static int seed = 1231241;
	
	void Awake() {
		if (onAwake) { SetColor(); }
	}
	
	void Start() {
		SetColor();
	}
	
	void SetColor() {
		int prevSeed = Random.seed;
		if (useSeed) { Random.seed = seed++; }
		
		if (renderer.material.HasProperty(target)) { renderer.material.SetColor(target, Color.Lerp(one, two, Random.value)); }
		
		if (useSeed) { Random.seed = prevSeed; }
		Destroy(this);
	}
	
}
