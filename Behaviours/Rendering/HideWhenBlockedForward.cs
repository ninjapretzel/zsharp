using UnityEngine;
using System.Collections;

public class HideWhenBlockedForward : MonoBehaviour {
	public LensFlare flare;
	public LayerMask mask;
	public float distance = 100;
	
	// Use this for initialization
	void Start () {
		flare = GetComponent<LensFlare>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics.Raycast(transform.position, Camera.main.transform.forward, distance, mask.value)) {
			if (flare != null && flare.enabled) { flare.enabled = false; }
			if (renderer != null && renderer.enabled) { renderer.enabled = false; }
		} else { 
			if (flare != null && !flare.enabled) { flare.enabled = true; }
			if (renderer != null && !renderer.enabled) { renderer.enabled = true; }
		}
	}
}
