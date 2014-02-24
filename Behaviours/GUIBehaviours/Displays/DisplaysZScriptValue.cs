using UnityEngine;
using System.Collections;

public class DisplaysZScriptValue : DisplaysString {
	public string field = "butts";
	public string prefix = "Butts: ";
	public bool floor = true;
	public bool time = false;
	public int timePrecision = 2;
	
	public override string GetString() {
		string s = prefix;
		if (time) {
			s += ZScript.Get(field).TimeFormat(timePrecision);
		} else {
			if (floor) {
				s += ZScript.Get(field).Floor();
			} else {
				s += ZScript.Get(field);
			}
		}
		return s;
	}
	
}
