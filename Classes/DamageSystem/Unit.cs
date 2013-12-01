using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	
	public Mortal mortality { get { return GetComponent<Mortal>(); } }
	public Weapon weapon;
	public string team = "Enemy";
	
	public Transform deadEffect;
	
	public bool triggerHeld = false;
	public bool triggerDown = false;
	public bool startReload = false;
	
	
	void Start() {
		weapon.holder = transform;
		
	}
	
	void Update() {
		weapon.Update();
		UpdateFire();
	}
	
	void UpdateFire() {
		if (weapon.isAuto && triggerHeld && weapon.canFireHold) {
			weapon.Fire();
		}
		
		if (triggerDown && weapon.canFireDown) {
			weapon.Fire();
		}
		
		if (startReload) {
			startReload = false;
			weapon.StartReload();
		}
	}
	
	
	void Die() {
		if (deadEffect != null) {
			Instantiate(deadEffect, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}
	
}
