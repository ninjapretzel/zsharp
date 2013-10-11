using UnityEngine;
using System.Collections;

public class MaterialPulse : MonoBehaviour {
	private Color baseColor;
	public string targetChannel = "_Emission";
	public Oscillator osc;
	
	void Start() {
		if (!renderer) { Destroy(this);	return; }
		baseColor = renderer.material.GetColor(targetChannel);
	}
	
	void Update() {
		float val = osc.Update();
		Color c = baseColor * val;
		c.a = baseColor.a;
		renderer.material.SetColor(targetChannel, c);
	}
	
	
}