using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Weapon {
	[System.Serializable]
	public class Damage {
		public string type = "slash";
		public float amount = 10;
	}

	[System.Serializable]
	public class Settings {
		public DealsDamage projectile;
		
		public float pellets = 1;
		public float spread = 3;
		public float minRecoil = 2;
		public float maxRecoil = 5;
		public float recoilAbsorb = 5;
		
		public float burst = 1;
		public float burstTime = .01f;
		
		public float fireTimeHold = .6f;
		public float fireTimeDown = .3f;
		
		public float reloadStart = .5f;
		public float reloadTick = .3f;
		public float reloadRounds = -1;
		
		public float ammoUse = 1;
		public float maxAmmo = 30;
		public float maxExtra = 180;
		
		public bool uniform = false;
		
	}
	
	public string name = "M16";
	public Settings settings;
	public Transform holder;
	//Wrapper accessors
	public DealsDamage projectile { get { return settings.projectile; } }
	public float pellets { get { return settings.pellets; } }
	public float kickback { get { return Random.Range(settings.minRecoil, settings.maxRecoil); } }
	public float recoilAbsorb { get { return settings.recoilAbsorb; } }
	public float burst { get { return settings.burst; } }
	public float burstTime { get { return settings.burstTime; } }
	public float fireTimeHold { get { return settings.fireTimeHold; } }
	public float fireTimeDown { get { return settings.fireTimeDown; } }
	public float reloadTime { get { return reloadTicked ? settings.reloadTick : settings.reloadStart; } }
	public float reloadRounds { get { return settings.reloadRounds; } }
	public float spread { get { return settings.spread + recoil; } }
	public float ammoUse { get { return settings.ammoUse; } }
	public float maxAmmo { get { return settings.maxAmmo; } }
	public float maxExtra { get { return settings.maxExtra; } }
	
	public bool uniform { get { return settings.uniform; } }
	
	public float ammo = 30;
	public float extraAmmo = 180;
	public float recoil = 0;
	public float timeout = 0;
	
	public int numBurst = 0;
	
	public bool bursting = false;
	public bool reloading = false;
	public bool reloadTicked = false;
	public bool freeReload = false;
	
	public List<Damage> damages;
	private Attack calcedDamages;
	private bool syncedDamages = false;
	
	public Attack damage { 
		get {
			if (syncedDamages) { return calcedDamages; }
			
			Attack dmg = new Attack();
			dmg.Clear();
			foreach (Damage s in damages) { dmg.Add(s.type, s.amount); }
			calcedDamages = dmg;
			syncedDamages = true;
			return dmg;
		}
		
		set {
			damages = new List<Damage>();
			foreach (string s in value.Keys) {
				Damage dmg = new Damage();
				dmg.type = s;
				dmg.amount = value[s];
			}
			syncedDamages = false;
		}
	}
	
	public float reloadPercent { get { return timeout / reloadTime; } }
	public float ammoPercent { get { return ammo / maxAmmo; } } 
	public float extraAmmoPercent { get { return extraAmmo / maxExtra; } }
	
	public bool hasAmmo { get { return ammo-ammoUse >= 0; } } 
	public bool doesBurst { get { return settings.burst > 1; } }
	public bool canFire { get { return canFireDown || canFireHold || canFireBurst; } } 
	public bool canFireBurst { get { return bursting && timeout >= burstTime; } }
	public bool canFireHold { get { return timeout >= fireTimeHold; } }
	public bool canFireDown { get { return timeout >= fireTimeDown; } }
	public bool isAuto { get { return settings.fireTimeHold > 0; } }
	public bool isSemi { get { return settings.fireTimeHold <= 0; } }
	
	
	public Weapon() {
		
		
	}
	
	
	//Update the gun and see if it can fire 
	public bool Update() { return Update(Time.deltaTime); }
	public bool Update(float time) {
		recoil = Mathf.Lerp(recoil, 0, Time.deltaTime * recoilAbsorb); 
		timeout += time;
		
		if (reloading) {
			if (timeout > reloadTime) { ReloadTick(); }
			return false;
		}
		
		if (bursting && doesBurst) {
			if (timeout > burstTime) {
				Fire();
				return false;
			}
		}
		
		return isAuto && canFireHold;
		
	}
	
	public string AmmoString() {
		string str = name;
		str += " - " + ammo + " / " + maxAmmo;
		str += " | " + extraAmmo + " / " + maxExtra;
		return str;
	}
	
	public bool Fire() { return Fire(holder); }
	public bool Fire(Transform transform) {
		if (reloading) { return false; }
		if (!canFire) { return false; }
		
		if (hasAmmo) {
			timeout = 0;
			ammo -= ammoUse;
			
			
			
			if (projectile) {
				
				for (int i = 0; i < pellets; i++) {
					float rot = RandomF.unit * spread * .5f;
					if (uniform) { 
						rot = -spread/2f + ((float)i+.5f) * (spread / pellets); 
						if (pellets == 1) { rot = 0; }
					}
					
					DealsDamage bullet = Transform.Instantiate(projectile, transform.position, transform.rotation) as DealsDamage;
					bullet.source = holder.GetComponent<Unit>();
					bullet.atk = damage;
					
					Vector3 forward = bullet.transform.forward;
					forward.y = 0;
					bullet.transform.forward = forward;
					bullet.transform.Rotate(0, rot, 0);
					
				}
				
				
			}
			
			if (doesBurst  && !bursting) {
				bursting = true;
				numBurst = 1;
			} else if (doesBurst && bursting) {
				numBurst++;
				if (numBurst >= burst) {
					numBurst = 0;
					bursting = false;
				}
			}
			
			recoil += kickback;
			return true;
		}
		return false;
	}
	
	
	public void StartReload() { StartReload(false); }
	public void StartReload(bool free) {
		if (reloading) { return; }
		if (ammo == maxAmmo) { return; }
		timeout = 0;
		reloadTicked = false;
		reloading = true;
		bursting = false;
		freeReload = free;
	}
	
	public void ReloadTick() {
		float toRefill = maxAmmo - ammo;
		if (reloadRounds > 0) { toRefill = Mathf.Min(reloadRounds, toRefill); }
		
		timeout = 0;
		reloadTicked = true;
		
		if (toRefill > extraAmmo) {
			ammo += extraAmmo;
			extraAmmo = 0;
		} else {
			extraAmmo -= toRefill;
			ammo += toRefill;
		}
		
		if (ammo == maxAmmo || extraAmmo == 0) { reloading = false; }
	}
	
	public void DrawAmmoCounter(Vector2 position, Texture2D tex, int lines) {
		Bars.graphic = tex;
		Rect brush = new Rect(position.x, position.y, tex.width * maxAmmo / lines, tex.height * lines);
		Bars.Draw(brush, new Vector2(maxAmmo/lines, lines), ammoPercent);
		
	}
	
}



























