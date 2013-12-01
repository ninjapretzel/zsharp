using UnityEngine;
using System.Collections;
using System;

public class ActivatesBehaviourOnValue : MonoBehaviour {
	public string when;
	public NumberCompare relation;
	public float val;
	
	private MonoBehaviour target;
	public string behaviour;
	
	public Func<float, float, bool> compare { get { return relation.Comparator(); } }
	public bool activated { get { return compare(ZScript.GetValue(when), val); } }
	
	void Start() {
		target = GetComponent(behaviour) as MonoBehaviour; 
	}
	
	void Update() {
		bool state = activated;
		if (state == true && !target.enabled) { target.enabled = true; }
		else if (state == false && target.enabled) { target.enabled = false; } 
	}
}
