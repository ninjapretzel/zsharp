using UnityEngine;
using System.Collections;

public class SpawnsStuffInAnAreaOverTime : MonoBehaviour {
	public Transform[] things;
	public Bounds area;
	public float minTime = 1;
	public float maxTime = 1.5f;
	public int minNum = 5;
	public int maxNum = 10;
	public Vector3 orientation = Vector3.zero;
	
	public float timeout;
	public float spawnTime;
	public bool makePushable = false;
	
	public RandomType randomness = RandomType.Normal;
	public static int seed = 132541;
	public const int SEEDSCALE = 1230123414;
	
	void Start() {
		spawnTime = RandomF.Range(minTime, maxTime);
	}
	
	void Update() {
		timeout += Time.deltaTime;
		if (timeout >= spawnTime) {
			timeout -= spawnTime;
			spawnTime = RandomF.Range(minTime, maxTime);
			Spawn();
		}
	}
	
	void Spawn() {
		if (randomness == RandomType.Seeded) {
			RandomF.Push(seed++);
		} else if (randomness == RandomType.Perlin) {
			Vector3 pos = transform.position;
			RandomF.Push((int)(SEEDSCALE * PerlinNoise.GetValue(pos.x, pos.z)));
		}
		
		
		int num = Random.Range(minNum, maxNum);
		
		for (int i = 0; i < num; i++) {
			Transform obj = Instantiate(things[(int)(things.Length * Random.value * .99999f)], transform.position + area.RandomInside(), Quaternion.identity) as Transform;
			obj.Rotate(orientation);
			obj.parent = transform;
			if (makePushable) { 
				obj.gameObject.AddComponent<Pushable>();
			}
		}
		
		if (randomness != RandomType.Normal) { 
			RandomF.Pop();
		}
	}
	
	void OnDrawGizmosSelected() {
		Color c = Color.yellow;
		c.a = .1f;
		Gizmos.color = c;
		Gizmos.DrawCube(transform.position + area.center, area.size);
	}

}
