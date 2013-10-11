using UnityEngine;
using System.Collections;

public class ParralaxMassModify : MonoBehaviour {
	public float distanceScale = 1.0f;
	
	public int seed = 1432154;
	public float minScale = .9f;
	public float maxScale = 1.1f;
	
	
	void Start() {
		int prevSeed = Random.seed;
		Random.seed = seed;
		Component[] parralaxi = GetComponentsInChildren<Parralax>() as Component[];
		
		//Debug.Log("Mass modify found " + parralaxi.Length + " Parralax");
		foreach (Component c in parralaxi) {
			Parralax p = c.GetComponent<Parralax>();
			//Debug.Log("Distance: " + p.distance + " - now : " + p.distance * distanceScale);
			p.distance *= distanceScale * Random.Range(minScale, maxScale);
		}
		
		Random.seed = prevSeed;
		Destroy(this);
	}
	
}
