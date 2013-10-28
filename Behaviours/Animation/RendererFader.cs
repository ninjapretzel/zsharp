using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RendererFader : MonoBehaviour {
	
	public string baseShader = "Diffuse";
	public string transparentShader = "Transparent/Diffuse";
	
	public float fadeTime = 1;
	public bool wantsToBeVisible = true;
	
	float time = 1;
	
	public float percentage {
		get { return time / fadeTime; }  
	}
	
	void Awake() {
		if (wantsToBeVisible) { time = fadeTime; }
		else { time = 0; }
		
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
		
		if (time == 1 && renderer.material.shader.name != baseShader) {
			renderer.material.shader = Shader.Find(baseShader);
			
		}
		
		else if (time < 1 && renderer.material.shader.name != transparentShader) {
			renderer.material.shader = Shader.Find(transparentShader);
			
		}
		
		Color c = renderer.material.color;
		c.a = percentage;
		renderer.material.color = c;
		
		
	}
	
	void SetTime(float timeToFade) {
		float p = percentage;
		fadeTime = timeToFade;
		time = p * timeToFade;
	}
	
	void FadeOut() {
		wantsToBeVisible = false;
	}
	
	void FadeOut(float timeToFade) {
		SetTime(timeToFade);
		wantsToBeVisible = false;
	}
	
	void FadeIn() {
		wantsToBeVisible = true;
	}
	
	void FadeIn(float timeToFade) {
		SetTime(timeToFade);
		wantsToBeVisible = false;
	}
	
	
	
	
}


























