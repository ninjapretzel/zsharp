using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Achievable {
	
	public string id;
	
	public bool unlocked = false;
	
	public virtual bool earned { get { return Earned(); } }
	public virtual bool Earned() { return false; }
	
	public void OnEvent(string eventName) {
		Type type = this.GetType();
		MethodInfo method = type.GetMethod(eventName, BindingFlags.Public | BindingFlags.Instance);
		
		if (method != null) {
			method.Invoke(this, null);
		}
		
	}
	
	
	
}

