using UnityEngine;
using System.Collections;

public class ForceRotationConstant : MonoBehaviour {
	public Vector3 euler = Vector3.zero;
	
	void LateUpdate() {
		transform.rotation = Quaternion.Euler(euler);
	}
	
}
