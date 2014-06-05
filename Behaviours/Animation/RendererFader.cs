using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RendererFader : MonoBehaviour {
	
	public string baseShader = "Diffuse";
	public string transparentShader = "Transparent/Diffuse";
	
	public float fadeTime = 1;
	public bool wantsToBeVisible = true;
	
	float alpha;
	
	public float time = 1;

	
	public float percentage {
		get { return time / fadeTime; }  
	}
	
	void Awake() {
		if (wantsToBeVisible) { time = fadeTime; }
		else { time = 0; }
		if (renderer.material.shader.name == "Vertex Lit") {
			baseShader = "Vertex Lit";
			transparentShader = "Transparent/Vertex Lit";
		}
		
		if (renderer.material.shader.name == "GUI/Text Shader") {
			
			baseShader = "GUI/Text Shader";
			transparentShader = "GUI/Text Shader";
		}
		alpha = renderer.material.color.a;
	}
	
	void Start() {
		
	}
	
	void Update() {
		if (wantsToBeVisible) {
			time = Mathf.Min(fadeTime, time + Time.deltaTime);
			
		} else {
			time = Mathf.Max(0, time - Time.deltaTime);
			
		}
		
		if (time == 0 && renderer.enabled) { renderer.enabled = false; }
		else if (time > 0 && !renderer.enabled) { renderer.enabled = true; }
		
		if (time == fadeTime && renderer.material.shader.name != baseShader) {
			if (alpha * percentage >= 1) { SetBaseShader(); }
			
		} else if (time < fadeTime && renderer.material.shader.name != transparentShader) {
			SetTransparentShader();
			
		}
		
		if (renderer.materials.Length > 1) {
			for (int i = 0; i < renderer.materials.Length; i++) {
				Color c = renderer.materials[i].color;
				c.a = percentage * alpha;
				renderer.materials[i].color = c;
			}
		} else {
			Color c = renderer.material.color;
			c.a = percentage * alpha;
			renderer.material.color = c;
		}
		
	}
	
	
	void SetBaseShader() { SetShader(baseShader); }
	void SetTransparentShader() { SetShader(transparentShader); }
	
	void SetShader(string shader) {
		if (renderer.materials.Length > 1) {
			foreach (Material m in renderer.materials) {
				m.shader = Shader.Find(shader);
			}
		} else {
			renderer.material.shader = Shader.Find(shader);
		}
	}
	
	public void SetTime(float timeToFade) {
		float p = percentage;
		fadeTime = timeToFade;
		time = p * timeToFade;
	}
	
	public void FadeOut() {
		wantsToBeVisible = false;
	}
	
	public void FadeOut(float timeToFade) {
		SetTime(timeToFade);
		wantsToBeVisible = false;
	}
	
	public void FadeOut(float timeToFade, float position) {
		SetTime(timeToFade);
		time = position;
		wantsToBeVisible = false;
	}
	
	public void FadeIn() {
		wantsToBeVisible = true;
	}
	
	public void FadeIn(float timeToFade) {
		SetTime(timeToFade);
		wantsToBeVisible = true;
	}
	
	public void FadeIn(float timeToFade, float position) {
		SetTime(timeToFade);
		time = position;
		wantsToBeVisible = true;
	}
	
	
	
	
}


























