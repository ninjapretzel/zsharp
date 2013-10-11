using UnityEngine;
using System.Collections;

public class ForceRotation : MonoBehaviour {
	public Vector3 euler = Vector3.zero;
	void Start() {
		transform.rotation = Quaternion.Euler(euler);
	}
	
}
