﻿using UnityEngine;
using System.Collections;

public class RandomLineRenderer : MonoBehaviour {
	public bool changeColor = true;
	public Color[] startColors;
	public Color[] endColors;
	
	public BMM length;
	public BMM width;
	
	public bool fade = true;
	public BMM fadeTime;
	
	void Start() {
		LineRenderer lr = GetComponent<LineRenderer>();
		if (lr == null) { Destroy(this); return; }
		
		Color c1 = startColors.Lerp(RandomF.value);
		Color c2 = endColors.Lerp(RandomF.value);
		if (changeColor) {
			lr.SetColors(c1, c2);
		}
		
		float w = width.value;
		lr.SetWidth(w, w);
		lr.SetPosition(0, Vector3.zero);
		lr.SetPosition(1, Vector3.forward * length.value);
		
		if (fade) {
			LineRendererFader fader = lr.gameObject.AddComponent<LineRendererFader>();
			fader.time = fadeTime.value;
			if (changeColor) {
				fader.startColor = c1;
				fader.endColor = c2;
			} else {
				Destroy(fader);
			}
		}
		
		Destroy(this);
	}
	
}
