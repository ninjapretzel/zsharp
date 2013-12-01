using UnityEngine;
using System.Collections;

public class HoverEntropy : MonoBehaviour {
	public static int seed = 123919541;
	public BMM time = new BMM();
	public BMM scale = new BMM();
	public BMM offsetx = new BMM();
	public BMM offsety = new BMM();
	public BMM offsetz = new BMM();
	
	public bool useSeed = true;
	
	void Start() {
		int oldSeed = Random.seed;
		if (useSeed) { Random.seed = seed; }
		
		Hover hover = GetComponent<Hover>();
		if (hover != null) {
			int num = Mathf.Min(hover.offsets.Length, hover.oscis.Length);
			for (int i = 0; i < num; i++) {
				Oscillator osci = hover.oscis[i];
				
				osci.maxTime *= time.value;
				float val = scale.value;
				osci.minVal *= val;
				osci.maxVal *= val;
				
				Vector3 offset = hover.offsets[i];
				Vector3 scales = new Vector3(offsetx.value, offsety.value, offsetz.value);
				hover.offsets[i] = Vector3.Scale(offset, scales);
				if (Random.value < .5) { hover.offsets[i] *= -1; }
			}
		}
		
		if (useSeed) { Random.seed = oldSeed; seed++; }
		Destroy(this);
	}
	
	void Update() {
	
	}
}
