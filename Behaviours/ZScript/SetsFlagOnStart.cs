using UnityEngine;
using System.Collections;

public class SetsFlagOnStart : MonoBehaviour {
	public string flag;
	public bool value;
	
	void Start() {
		ZScript.SetFlag(flag, value);
		Destroy(this);
		
	}
	
}
