using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MaterialPanner : MonoBehaviour {
	public Vector2 speed = new Vector2(1, 0);
	public float scale = 1.0f;
	
	public bool doMainTexture = true;
	public string[] targets;
	
	private Material mat;
	public Material material {
		get { return mat; }
		set {
			mat = value;
			renderer.material = mat;
		}
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
		if (mat.HasProperty(target)) {
			Vector2 offset = mat.GetTextureOffset(target);
			offset += speed * scale * Time.deltaTime;
			offset.x %= 1;
			offset.y %= 1;
			mat.SetTextureOffset(target, offset);
		}
	}
}
