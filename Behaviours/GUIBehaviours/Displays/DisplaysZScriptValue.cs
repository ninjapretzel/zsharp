using UnityEngine;
using System.Collections;

public class DisplaysZScriptValue : DisplaysString {
	public string field = "butts";
	public string prefix = "Butts: ";
	public bool floor = true;
	
	public override string GetString() {
		string s = prefix;
		if (floor) {
			s += ZScript.Get(field).Floor();
		} else {
			s += ZScript.Get(field);
		}
		return s;
	}
	
}
