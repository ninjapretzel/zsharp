using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeepsConsistantTextureRepeat : MonoBehaviour {
	
	Vector2 initialTextureScale;
	Vector3 initialWorldScale;
	
	void Start() {
		initialTextureScale = renderer.material.mainTextureScale;
		initialWorldScale = transform.lossyScale;
		
	}
	
	void Update() {
		Vector2 scale = new Vector2(0, 0);
		Vector2 offset = renderer.material.mainTextureOffset;
		 
		scale.x = initialWorldScale.x / initialTextureScale.x * transform.lossyScale.x;
		scale.y = initialWorldScale.y / initialTextureScale.y * transform.lossyScale.y;
		renderer.material.mainTextureScale = scale;
		renderer.material.mainTextureOffset = offset;
	}
	
}




















