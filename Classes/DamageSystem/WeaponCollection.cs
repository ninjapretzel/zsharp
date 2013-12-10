using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponCollection : MonoBehaviour, IWeapon {
	public List<IWeapon> weapons;
	
	public float ammo = 1;
	public float maxAmmo = 1;
	public float ammoUse = 0;
	
	public float extraAmmo = 100;
	public float extraAmmoMax = 100;
	
	public float reloadTime = 3;
	public float timeout = 0;
	public bool reloading = false;
	public bool freeReload = false;
	
	public float ammoPercent { get { return ammo / maxAmmo; } }
	public float extraAmmoPercent { get { return extraAmmo / extraAmmoMax; } }
	public float reloadPercent { get { return timeout/reloadTime; } }
	
	public bool hasAmmo { get { return ammo > 0; } }
	public bool canFire { get { return ammo > 0; } }
	public bool canFireBurst { get { return false; } }
	public bool canFireDown { get { return true; } }
	public bool canFireHold { get { return true; } }
	public bool isAuto { get { return true; } }
	public bool isSemi { get { return false; } }
	
	
	private Transform heldBy;
	public Transform holder { get { return heldBy; } set { heldBy = value; } }
	
	void Update() {
		if (reloading) {
			timeout += Time.deltaTime;
			if (timeout > reloadTime) {
				Reloaded();
			}
		}
	}
	
	public void Reloaded() {
		if (freeReload) { ammo = maxAmmo; return; }
		float toRefill = maxAmmo - ammo;
		if (toRefill > extraAmmo) {
			ammo += extraAmmo;
			extraAmmo = 0;
		} else {
			extraAmmo -= toRefill;
			ammo += toRefill;
		}
		
		
		reloading = false;
	}
	
	public string AmmoString() {
		return "" + ammo.Format(0) + " / " + maxAmmo.Format(0);
	}
	
	
	public void Refill() {
		foreach (IWeapon w in weapons) { w.Refill(); } 
	}
	
	public void CreateProjectiles(Transform t) {
		foreach (IWeapon w in weapons) { w.CreateProjectiles(t); }
	}
	
	public bool Fire() { return Fire(holder); }
	public bool Fire(Transform t) { 
		if (reloading) { return false; }
		if (!canFire) { return false; }
		
		if (hasAmmo) {
			ammo -= ammoUse;
			foreach (IWeapon w in weapons) { w.Fire(t); }
			return true;
		}
		return false;
		
	}
	
	public void StartReload() { StartReload(false); }
	public void StartReload(bool free) {
		freeReload = free;
		timeout = 0;
		reloading = true;
		foreach (IWeapon w in weapons) { w.StartReload(free); }
	}
	
}
