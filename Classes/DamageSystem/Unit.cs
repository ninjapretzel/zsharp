using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	
	public Mortal mortality { get { return GetComponent<Mortal>(); } }
	private Weapon wp;
	public Weapon weapon { 
		get {
			if (wp != null) { return wp; }
			return transform.Require<Weapon>();
		}
		
		set {
			wp = value;
		}
	}
	
	public string team = "Enemy";
	
	public Transform deadEffect;
	
	public bool triggerHeld = false;
	public bool triggerDown = false;
	public bool startReload = false;
	
	
	void Start() {
		if (weapon == null) {
			weapon = transform.Require<Weapon>();
		}
		Equip(weapon);
		
	}
	
	void Update() {
		
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
	
	public void Equip(Weapon w) {
		weapon = w;
		weapon.holder = transform;
	}
	
	
	
	
}
