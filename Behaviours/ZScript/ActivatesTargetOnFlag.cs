using UnityEngine;
using System.Collections;

public class ActivatesTargetOnFlag : MonoBehaviour {
	public string flag = "show";
	public GameObject target;
	public string targetName = "";
	
	
	void Start() {
		if (!target) {
			GameObject check = GameObject.Find(targetName);
			if (check != null) { target = check; }
		
		}
	}
	
	void Update() {
		if (target == null) { return; }
		
		
		if (ZScript.GetFlag(flag)) {
			if (!target.activeSelf) { target.SetActive(true); }
		} else {
			if (target.activeSelf) { target.SetActive(false); } 
		}
		
	}
	
}
