using UnityEngine;
using System.Collections;

public class CreateRandomObjectOnStart : MonoBehaviour {
	public Transform[] targets;
	public Vector3 offset;
	public bool parentIt = true;
	
	void Awake() {
		Transform target = targets[(int)(targets.Length * Random.value * 0.9999f)];
		Transform obj = Instantiate(target, transform.position + offset, transform.rotation) as Transform;
		if (parentIt) { obj.parent = transform; }
		
		Destroy(this);
	}
	
}
