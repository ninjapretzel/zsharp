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
		if (renderer.material.shader.name == "GUI/Text Shader") {
			baseShader = "GUI/Text Shader";
			transparentShader = "GUI/Text Shader";
		}
		alpha = renderer.material.color.a;
	}
	
	void Start() {
		if (wantsToBeVisible) { time = fadeTime; }
		else { time = 0; }
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
			if (alpha * percentage >= 1) { renderer.material.shader = Shader.Find(baseShader); }
			
		}
		
		else if (time < fadeTime && renderer.material.shader.name != transparentShader) {
			renderer.material.shader = Shader.Find(transparentShader);
			
		}
		
		Color c = renderer.material.color;
		c.a = percentage * alpha;
		renderer.material.color = c;
		
		
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


























