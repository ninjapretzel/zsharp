using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
	public Vector3 speed = Vector3.zero;
	
	void Update () {
		transform.Rotate(speed * Time.deltaTime);
	}
	
	
}
