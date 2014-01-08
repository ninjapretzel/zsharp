using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTextureOffset : MonoBehaviour {
	
	public Vector2 amount;
	
	void Start() {
		Vector2 offset = renderer.material.mainTextureOffset;
		offset.x += Random.value * amount.x;
		offset.y += Random.value * amount.y;
		renderer.material.mainTextureOffset = offset;
		Destroy(this);
	}
	
	
}




















