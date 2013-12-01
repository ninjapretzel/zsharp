using UnityEngine;
using System.Collections;

public class SetsFlagOnDestroy : MonoBehaviour {
	public string flag;
	public bool value;
	private static bool quitting = false;
	
	void OnApplicationQuit() {
		quitting = true;
	}
	
	void OnDestroy() {
		if (quitting) { return; }
		
		ZScript.SetFlag(flag, value);
	}
	
}
