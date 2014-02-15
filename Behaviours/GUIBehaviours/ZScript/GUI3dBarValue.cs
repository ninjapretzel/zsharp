using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI3dBarValue : MonoBehaviour {
	public string valueName = "player_health";
	
	GUI3dBar bar;
	
	void Start() {
		bar = GetComponent<GUI3dBar>();
		
	}
	
	void Update() {
		bar.value = ZScript.GetValue("player_health");
	}
	
}




















