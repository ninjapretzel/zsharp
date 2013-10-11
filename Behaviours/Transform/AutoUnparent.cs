using UnityEngine;
using System.Collections;

public class AutoUnparent : MonoBehaviour {
	void Start() {
		transform.parent = null;
	}
	
}
