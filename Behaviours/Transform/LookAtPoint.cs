using UnityEngine;
using System.Collections;

public class LookAtPoint : MonoBehaviour {
	public Transform target;
	public Vector3 targetV;
	
	void LateUpdate() {
		if (target) { transform.LookAt(target); }
		else { transform.LookAt(targetV); }
	}
}