using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour, Wep {
	
	public List<Weapon> weapons;
	public int index = 0;
	
	public DealsDamage projectile { get { return activeWeapon.projectile; } }
	public bool uniformDirection { get { return activeWeapon.uniformDirection; } }
	public bool uniformPosition { get { return activeWeapon.uniformPosition; } }
	public bool spins { get { return activeWeapon.spins; } }
	public Vector3 offset { get { return activeWeapon.offset; } }
	public Vector3 mirrorOffset { get { return activeWeapon.mirrorOffset; } }
	public float pellets { get { return activeWeapon.pellets; } }
	public float kickback { get { return activeWeapon.kickback; } }
	public float spinPerBurst { get { return activeWeapon.spinPerBurst; } }
	public float repeatAngle { get { return activeWeapon.repeatAngle; } }
	public float recoilAbsorb { get { return activeWeapon.recoilAbsorb; } }
	public float bursts { get { return activeWeapon.bursts; } }
	public float burstTime { get { return activeWeapon.burstTime; } }
	public float fireTimeHold { get { return activeWeapon.fireTimeHold; } }
	public float fireTimeDown { get { return activeWeapon.fireTimeDown; } }
	public float reloadTime { get { return activeWeapon.reloadTime; } }
	public float reloadRounds { get { return activeWeapon.reloadRounds; } }
	public float spread { get { return activeWeapon.spread; } }
	public float ammoUse { get { return activeWeapon.ammoUse; } }
	public float maxAmmo { get { return activeWeapon.maxAmmo; } }
	public float maxExtra { get { return activeWeapon.maxExtra; } }
	
	public Attack damage { 
		get { return activeWeapon.damage; }
		set { activeWeapon.damage = value; }
	}
	
	public float reloadPercent { get { return activeWeapon.reloadPercent; } }
	public float ammoPercent { get { return activeWeapon.ammoPercent; } }
	public float extraAmmoPercent { get { return activeWeapon.extraAmmoPercent; } }
	
	public bool hasAmmo { get { return activeWeapon.hasAmmo; } }
	public bool doesBurst { get { return activeWeapon.doesBurst; } }
	public bool canFire { get { return activeWeapon.canFire; } }
	public bool canFireBurst { get { return activeWeapon.canFireBurst; } }
	public bool canFireHold { get { return activeWeapon.canFireHold; } }
	public bool isAuto { get { return activeWeapon.isAuto; } }
	public bool isSemi { get { return activeWeapon.isSemi; } }
	
	public Weapon activeWeapon { 
		get {
			index = (int)index.Clamp(0, weapons.Count-1);
			return weapons[index];
		}
	}
	public Weapon.Settings settings { get { return activeWeapon.settings; }  }
	
	
	public void CreateProjectiles(Transform tr) { activeWeapon.CreateProjectiles(tr); }
	
	public string AmmoString() { return activeWeapon.AmmoString(); }
	
	public bool Fire() { return activeWeapon.Fire(); }
	public bool Fire(Transform tr) { return activeWeapon.Fire(tr); }
	
	public void Refill() { activeWeapon.Refill(); }
	public void ReloadTick() { activeWeapon.ReloadTick(); }
	public void StartReload() { activeWeapon.StartReload(false); }
	public void StartReload(bool free) { activeWeapon.StartReload(free); }
	
	
	
}
