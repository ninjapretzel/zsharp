using UnityEngine;
using System.Collections;

public class SpawnsStuffInAnArea : MonoBehaviour {
	public Transform[] things;
	public Bounds area;
	public int min = 5;
	public int max = 10;
	
	public bool makePushable = false;
	
	void Start() {
		int num = Random.Range(min, max);
		
		for (int i = 0; i < num; i++) {
			Transform obj = Instantiate(things[(int)(things.Length * Random.value * .99999f)], transform.position + area.RandomInside(), Quaternion.identity) as Transform;
			obj.parent = transform;
			if (makePushable) { 
				obj.gameObject.AddComponent<Pushable>();
			}
		}
		
	}
	
	void OnDrawGizmos() {
		Color c = Color.red;
		c.a = .5f;
		Gizmos.color = c;
		Gizmos.DrawCube(transform.position + area.center, area.size);
	}
}
