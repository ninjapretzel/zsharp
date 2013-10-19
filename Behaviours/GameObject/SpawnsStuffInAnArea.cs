using UnityEngine;
using System.Collections;

public class SpawnsStuffInAnArea : MonoBehaviour {
	public Transform[] things;
	public Bounds area;
	public int min = 5;
	public int max = 10;
	
	public bool makePushable = false;
	
	public RandomType randomness = RandomType.Normal;
	public static int seed = 132541;
	public const int SEEDSCALE = 1230123414;
	
	void Start() {
		if (randomness == RandomType.Seeded) {
			RandomF.Push(seed++);
		} else if (randomness == RandomType.Perlin) {
			Vector3 pos = transform.position;
			RandomF.Push((int)(SEEDSCALE * PerlinNoise.GetValue(pos.x, pos.z)));
		}
		
		
		int num = Random.Range(min, max);
		//Debug.Log(num + " : " + min + "-" + max);
		
		for (int i = 0; i < num; i++) {
			Transform obj = Instantiate(things[(int)(things.Length * Random.value * .99999f)], transform.position + area.RandomInside(), Quaternion.identity) as Transform;
			obj.parent = transform;
			if (makePushable) { 
				obj.gameObject.AddComponent<Pushable>();
			}
		}
		
		if (randomness != RandomType.Normal) { 
			RandomF.Pop();
		}
		Destroy(this);
	}
	
	void OnDrawGizmos() {
		if (Application.isPlaying) { return; }
		Color c = Color.red;
		c.a = .5f;
		Gizmos.color = c;
		Gizmos.DrawCube(transform.position + area.center, area.size);
	}

}
