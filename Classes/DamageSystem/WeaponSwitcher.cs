using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour, IWeapon {
	
	public List<IWeapon> weapons;
	public int index = 0;
	
	private Transform heldBy;
	public Transform holder { get { return heldBy; } set { heldBy = value; } }
	
	public float reloadPercent { get { return activeWeapon.reloadPercent; } }
	public float ammoPercent { get { return activeWeapon.ammoPercent; } }
	public float extraAmmoPercent { get { return activeWeapon.extraAmmoPercent; } }
	
	public bool hasAmmo { get { return activeWeapon.hasAmmo; } }
	public bool canFire { get { return activeWeapon.canFire; } }
	public bool canFireBurst { get { return activeWeapon.canFireBurst; } }
	public bool canFireHold { get { return activeWeapon.canFireHold; } }
	public bool canFireDown { get { return activeWeapon.canFireDown; } }
	public bool isAuto { get { return activeWeapon.isAuto; } }
	public bool isSemi { get { return activeWeapon.isSemi; } }
	
	public IWeapon activeWeapon { 
		get {
			index = (int)index.Clamp(0, weapons.Count-1);
			return weapons[index];
		}
	}
	
	public void CreateProjectiles(Transform tr) { activeWeapon.CreateProjectiles(tr); }
	
	public string AmmoString() { return activeWeapon.AmmoString(); }
	
	public bool Fire() { return activeWeapon.Fire(); }
	public bool Fire(Transform tr) { return activeWeapon.Fire(tr); }
	
	public void Refill() { activeWeapon.Refill(); }
	public void StartReload() { activeWeapon.StartReload(false); }
	public void StartReload(bool free) { activeWeapon.StartReload(free); }
	
	
	
}
