using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	
	public Mortal mortality { get { return GetComponent<Mortal>(); } }
	private IWeapon wp;
	public IWeapon weapon { 
		get {
			if (wp != null) { return wp; }
			wp = transform.Require<Weapon>() as IWeapon;
			return wp;
		}
		
		set {
			wp = value;
		}
	}
	
	public string team = "Enemy";
	
	public bool useLateUpdate = false;
	public Transform deadEffect;
	public Transform hitEffect;
	
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
		if (!useLateUpdate) { UpdateFire(); }
	}
	
	void LateUpdate() {
		if (useLateUpdate) { UpdateFire(); }
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
	
	public virtual void Die() {
		if (deadEffect != null) {
			Instantiate(deadEffect, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}
	
	public void Equip(IWeapon w) {
		weapon = w;
		weapon.holder = transform;
	}
	
	public void HitEffect(Vector3 v) {
		if (hitEffect == null) { return; }
		Instantiate(hitEffect, v, Quaternion.identity);
		
	}
	
	
	
	
}
