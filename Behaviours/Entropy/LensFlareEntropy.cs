using UnityEngine;
using System.Collections;

public class LensFlareEntropy : MonoBehaviour {
	public BMM brightness;
	public Color[] colors;
	public Flare[] flares;
	
	
	void Start () {
		LensFlare ls = GetComponent<LensFlare>();
		
		ls.color = colors.Lerp(RandomF.value);
		ls.brightness *= brightness.value;
		if (flares.Length > 0) {
			ls.flare = flares.Choose();
		}
	}
	
	void Update () {
	
	}
}
