using UnityEngine;
using System.Collections;

public class ForceEnabled : MonoBehaviour {
	public string behaviour;
	
	void Awake() {
		MonoBehaviour b = GetComponent(behaviour) as MonoBehaviour;
		b.enabled = true;
		Debug.Log(b.enabled);
	}
	
	void Start() {
		MonoBehaviour b = GetComponent(behaviour) as MonoBehaviour;
		b.enabled = true;
		Destroy(this);
	}
	
	
}
