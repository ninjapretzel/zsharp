using UnityEngine;
using System.Collections;

public static class QuaternionF {
	
	public static Quaternion FlattenZ(this Quaternion q) {
		Vector3 e = q.eulerAngles;
		e.x = 0;
		return Quaternion.Euler(e);
	}
	
	public static Quaternion FlattenY(this Quaternion q) {
		Vector3 e = q.eulerAngles;
		e.x = 0;
		return Quaternion.Euler(e);
	}
	
	public static Quaternion FlattenX(this Quaternion q) {
		Vector3 e = q.eulerAngles;
		e.z = 0;
		return Quaternion.Euler(e);
	}
	
	
}
