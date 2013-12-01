using UnityEngine;
using System.Collections;

public class ActivatesBehaviourOnFlag : MonoBehaviour {
	public string flag;
	public string behaviour;
	private MonoBehaviour target;
	
	void Start() {
		target = GetComponent(behaviour) as MonoBehaviour;
		if (!target) { 
			Debug.LogWarning("Did not find component " + behaviour + " on " + gameObject.name);
			Destroy(this);
		}
	}
	
	void Update() {
		if (ZScript.GetFlag(flag)) {
			if (!target.enabled) { target.enabled = true; }
			
		} else {
			if (target.enabled) { target.enabled = false; }
		}
		
	}
}
