using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MaterialPanner : MonoBehaviour {
	public Vector2 speed = new Vector2(1, 0);
	public float scale = 1.0f;
	
	public bool doMainTexture = true;
	public string[] targets;
	
	public Material material {
		get { return renderer.material; }
		set { renderer.material = value; }
	}
	
	void Awake() {
		if (!renderer) { Destroy(this); return; }
		material = new Material(renderer.material);
	}
	
	void Start() {
		
	}
	
	void Update() {
		UpdateMaterial();
	}
	
	void UpdateMaterial() {
		if (doMainTexture) { Pan("_MainTex"); }
		foreach (string s in targets) { Pan(s); }
	}
	
	void Pan(string target) {
		if (material.HasProperty(target)) {
			Vector2 offset = material.GetTextureOffset(target);
			offset += speed * scale * Time.deltaTime;
			offset.x %= 1;
			offset.y %= 1;
			material.SetTextureOffset(target, offset);
		}
	}
}
