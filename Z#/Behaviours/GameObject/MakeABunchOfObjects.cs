using UnityEngine;
using System.Collections;

public class MakeABunchOfObjects : MonoBehaviour {
	public Transform thingToMake;
	
	public Vector3 offset = new Vector3(1, 1, 1);
	//(3, 1, 1) will create 3 objects, (3, 3, 3) will create 3 * 3 * 3 = 27 objects
	public Vector3 numToCreate = new Vector3(3, 1, 1);
	
	public Vector3 rotation = new Vector3(-90, 0, 0);
	
	
	// Use this for initialization
	void Start() {
		Vector3 center = transform.position;
		Vector3 off = numToCreate;
		off.x -= 1; off.y -= 1; off.z -= 1;
		off *= .5f;
		
		Transform obj;
		Vector3 corner = center - Vector3.Scale(off, offset);
		for (int xx = 0; xx < numToCreate.x; xx++) {
			for (int yy = 0; yy < numToCreate.y; yy++) {
				for (int zz = 0; zz < numToCreate.z; zz++) {
					off = Vector3.Scale(offset, new Vector3(xx, yy, zz));
					obj = Instantiate(thingToMake, corner + off, Quaternion.identity) as Transform;
					obj.Rotate(rotation);
					obj.parent = transform;
				}
			}
		}
		
		
		Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
