using UnityEngine;
using System.Collections;

public class MonoBehaviourActivator : MonoBehaviour {
	public string watchThis;
	public Transform onThisObject;
	private MonoBehaviour watched;
	
	public string[] activateThese;
	private MonoBehaviour[] toActivate;
	
	public void Awake() {
		if (onThisObject != null) {
			watched = onThisObject.GetComponent(watchThis) as MonoBehaviour;
		}
		
		toActivate = new MonoBehaviour[activateThese.Length];
		for (int i = 0; i < activateThese.Length; i++) {
			toActivate[i] = GetComponent(activateThese[i]) as MonoBehaviour;
		}
		
	}
	
	public void Update() {
		if (!watched) { return; }
		
		bool s = watched.enabled;
		foreach (MonoBehaviour m in toActivate) {
			m.enabled = s;
		}
		
	}
	
	
}
