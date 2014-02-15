using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public interface IWeapon {
	float ammoPercent { get; }
	float extraAmmoPercent { get; }
	float reloadPercent { get; }
	Transform holder { get; set; }
	bool hasAmmo { get; } 
	bool canFire { get; }
	bool canFireBurst { get; } 
	bool canFireDown { get; }
	bool canFireHold { get; }
	bool isAuto { get; }
	bool isSemi { get; } 
	
	void CreateProjectiles(Transform tr);
	string AmmoString();
	bool Fire();
	bool Fire(Transform tr);
	
	void StartReload();
	void StartReload(bool free);
	
	void Refill();
	
}


[System.Serializable]
public class Weapon : MonoBehaviour, IWeapon {
	[System.Serializable]
	public class Damage {
		public string type = "slash";
		public float amount = 10;
	}

	[System.Serializable]
	public class Settings {
		public DealsDamage projectile;
		
		//Distribution Settings
		public bool uniformDirection = false;
		public bool uniformPosition = true;
		public bool spins = false;
		public Vector3 offset = Vector3.zero;
		public Vector3 mirrorOffset = Vector3.zero;
		public float pellets = 1;
		public float spread = 3;
		
		public float minRecoil = 2;
		public float maxRecoil = 5;
		public float recoilEffect = 1;
		public float spin = 15;
		public float repeatAngle = 720;
		public float recoilAbsorb = 5;
		
		public float bursts = 1;
		public float burstTime = .01f;
		
		public float fireTimeHold = .6f;
		public float fireTimeDown = .3f;
		
		public float reloadStart = .5f;
		public float reloadTick = .3f;
		public float reloadRounds = -1;
		
		public float ammoUse = 1;
		public float maxAmmo = 30;
		public float maxExtra = 180;
		
		public Settings Clone() { return new Settings(this); }
		public Settings() { }
		public Settings(Settings source) {
			projectile = source.projectile;
			
			uniformDirection = source.uniformDirection;
			uniformPosition = source.uniformPosition;
			spins = source.spins;
			offset = source.offset;
			mirrorOffset = source.mirrorOffset;
			pellets = source.pellets;
			spread = source.spread;
			
			minRecoil = source.minRecoil;
			maxRecoil = source.maxRecoil;
			spin = source.spin;
			repeatAngle = source.repeatAngle;
			recoilAbsorb = source.recoilAbsorb;
			
			bursts = source.bursts;
			burstTime = source.burstTime;
			
			fireTimeHold = source.fireTimeHold;
			fireTimeDown = source.fireTimeDown;
			reloadStart = source.reloadStart;
			reloadTick = source.reloadTick;
			reloadRounds = source.reloadRounds;
			
			ammoUse = source.ammoUse;
			maxAmmo = source.maxAmmo;
			maxExtra = source.maxExtra;
		}
		
		public static Settings LoadFromTable(Table t) {
			Settings s = new Settings();
			s.Load(t);
			return s;
		}
		
		public void Load(Table t) {
			projectile = null;
			
			uniformDirection = t["uniformDirection"] == 1;
			uniformPosition = t["uniformPosition"] == 1;
			spins = t["spins"] == 1;
			offset = t.GetVector3("offset");
			mirrorOffset = t.GetVector3("mirrorOffset");
			pellets = t["pellets"];
			spread = t["spread"];
			
			minRecoil = t["minRecoil"];
			maxRecoil = t["maxRecoil"];
			spin = t["spin"];
			repeatAngle = t["repeatAngle"];
			recoilAbsorb = t["recoilAbsorb"];
			
			bursts = t["bursts"];
			burstTime = t["burstTime"];
			
			fireTimeHold = t["fireTimeHold"];
			fireTimeDown = t["fireTimeDown"];
			reloadStart = t["reloadStart"];
			reloadTick = t["reloadTick"];
			reloadRounds = t["reloadRounds"];
			
			ammoUse = t["ammoUse"];
			maxAmmo = t["maxAmmo"];
			maxExtra = t["maxExtra"];
		}
		
		public Table GetTable() {
			Table t = new Table();
			t["uniformDirection"] = uniformDirection ? 1 : 0;
			t["uniformPosition"] = uniformPosition ? 1 : 0;
			t["spins"] = spins ? 1 : 0;
			t.SetVector3("offset", offset);
			t.SetVector3("mirrorOffset", mirrorOffset);
			t["pellets"] = pellets;
			t["spread"] = spread;
			
			t["minRecoil"] = minRecoil;
			t["maxRecoil"] = maxRecoil;
			t["spin"] = spin;
			t["repeatAngle"] = repeatAngle;
			t["recoilAbsorb"] = recoilAbsorb;
			
			t["bursts"] = bursts;
			t["burstTime"] = burstTime;
			
			t["fireTimeHold"] = fireTimeHold;
			t["fireTimeDown"] = fireTimeDown;
			t["reloadStart"] = reloadStart;
			t["reloadTick"] = reloadTick;
			t["reloadRounds"] = reloadRounds;
			
			t["ammoUse"] = ammoUse;
			t["maxAmmo"] = maxAmmo;
			t["maxExtra"] = maxExtra;
			
			return t;
		}
		
	}
	
	public string wepName = "M16";
	public Settings settings;
	public Transform bulletSourceOverride;
	
	private Transform heldBy;
	public Transform bulletSource { get { return bulletSourceOverride != null ? bulletSourceOverride : holder; } }
	public Transform holder { get { return heldBy; } set { heldBy = value; }  }
	//Wrapper accessors
	
	public DealsDamage projectile { get { return settings.projectile; } set { settings.projectile = value; } }
	public bool uniformDirection { get { return settings.uniformDirection; } set { settings.uniformDirection = value; } }
	public bool uniformPosition { get { return settings.uniformPosition; } set { settings.uniformPosition = value; } }
	public bool spins { get { return settings.spins; } set { settings.spins = value; } }
	public Vector3 offset { get { return settings.offset; } set { settings.offset = value; } } 
	public Vector3 mirrorOffset { get { return settings.mirrorOffset; } set { settings.mirrorOffset = value; } }
	public float pellets { get { return settings.pellets; } set { settings.pellets = value; } }
	
	public float kickback { get { return Random.Range(settings.minRecoil, settings.maxRecoil); } }
	public float spinPerBurst { get { return settings.spin; } set { settings.spin = value; } }
	public float repeatAngle { get { return settings.repeatAngle; } set { settings.repeatAngle = value; } }
	public float recoilEffect { get { return settings.recoilEffect; } set { settings.recoilEffect = value;; } }
	public float recoilAbsorb { get { return settings.recoilAbsorb; } set { settings.recoilAbsorb = value; } }
	public float bursts { get { return settings.bursts; } set { settings.bursts = value; } }
	public float burstTime { get { return settings.burstTime; } set { settings.burstTime = value; } }
	public float fireTimeHold { get { return settings.fireTimeHold; } set { settings.fireTimeHold = value; } }
	public float fireTimeDown { get { return settings.fireTimeDown; } set { settings.fireTimeDown = value; } }
	
	public float reloadTime { get { return reloadTicked ? settings.reloadTick : settings.reloadStart; } }
	
	public float reloadRounds { get { return settings.reloadRounds; } set { settings.reloadRounds = value; } }
	public float spread { get { return settings.spread + recoil * recoilEffect; } }
	public float ammoUse { get { return settings.ammoUse; } set { settings.ammoUse = value; } }
	public float maxAmmo { get { return settings.maxAmmo; } set { settings.maxAmmo = value; } }
	public float maxExtra { get { return settings.maxExtra; } set { settings.maxExtra = value; } }
	
	
	
	public float ammo = 30;
	public float extraAmmo = 180;
	public float recoil = 0;
	public float spin = 0;
	public float timeout = 0;
	
	public int numBurst = 0;
	
	public bool bursting = false;
	public bool reloading = false;
	public bool doneReloading = false;
	public bool reloadTicked = false;
	public bool freeReload = false;
	
	
	public List<Damage> damages;
	private Attack calcedDamages;
	private bool syncedDamages = false;
	
	
	public static bool use2dRotation = true;
	
	public Weapon() {
		settings = new Settings();
	}
	
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
	public bool doesBurst { get { return bursts > 1; } }
	public bool canFire { get { return canFireDown || canFireHold || canFireBurst; } } 
	public bool canFireBurst { get { return bursting && timeout >= burstTime; } }
	public bool canFireHold { get { return timeout >= fireTimeHold; } }
	public bool canFireDown { get { return timeout >= fireTimeDown; } }
	public bool isAuto { get { return fireTimeHold > 0; } }
	public bool isSemi { get { return fireTimeHold <= 0; } }
	
	
	public void CreateProjectiles(Transform tr) {
		
		int num = (int)pellets;
		//Debug.Log("" + num.MidA() + "-" + num.MidB());
		
		for (int i = 0; i < num; i++) {
			float rot = RandomF.unit * spread * .5f;
			float f = ((float)i+.5f) / pellets;
			
			if (uniformDirection) {
				rot = -spread/2f + spread * f;
				if (num == 1) { rot = 0; }
				
			}
			
			if (spins) { rot += spin; }
			Vector3 off = Vector3.Scale(Random.insideUnitSphere, offset * .5f);
			
			if (uniformPosition) {
				//Adds offset for index position
				off = -offset/2f + offset * f;
				
				//Adds mirrorOffset for index position
				int n = 0;
				if (i < num.MidA()) { n = i - num.MidA(); } 
				else if (i > num.MidB()) { n = i - num.MidB(); }
				off += (-.5f + ((float)n).Abs() * 2f / pellets) * mirrorOffset;
			}
			DealsDamage bullet;
			if (projectile == null) {
				bullet = HitscanProjectile.Factory();
				bullet.transform.position = tr.position;
				bullet.transform.rotation = tr.rotation;
			} else {
				bullet = Transform.Instantiate(projectile, tr.position, tr.rotation) as DealsDamage;
			}
			
			if (bullet.sticksToSource) { 
				bullet.transform.parent = tr;
			}
			bullet.source = holder.GetComponent<Unit>();
			bullet.atk = damage;
			bullet.transform.Translate(off);
			
			if (use2dRotation) {
				
				Vector3 forward = bullet.transform.forward;
				forward.y = 0;
				bullet.transform.forward = forward;
				bullet.transform.Rotate(0, rot, 0);				
			} else {
				Vector3 axis = Random.onUnitSphere;
				bullet.transform.Rotate(axis * rot);
			}
			
		}
	}
	
	public void Update() { Update(Time.deltaTime); }
	//Update the gun and see if it can fire 
	public bool Update(float time) {
		doneReloading = false;
		
		if (uniformDirection) {
			recoil = recoil % (repeatAngle * pellets);
		} else {
			recoil = Mathf.Lerp(recoil, 0, Time.deltaTime * recoilAbsorb); 
		}
		timeout += time;
		
		if (reloading) {
			reloadTicked = false;
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
	
	public bool Fire() { return Fire(bulletSource); }
	public bool Fire(Transform tr) {
		//Debug.Log("Weapon.Fire() called");
		if (reloading) { return false; }
		if (!canFire) { return false; }
		
		if (hasAmmo) {
			timeout = 0;
			ammo -= ammoUse;
			
			CreateProjectiles(tr);
			
			if (doesBurst  && !bursting) {
				bursting = true;
				numBurst = 1;
			} else if (doesBurst && bursting) {
				numBurst++;
				if (numBurst >= bursts) {
					numBurst = 0;
					bursting = false;
				}
			}
			
			spin = (spin + spinPerBurst) % 360;
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
	
	public void Refill() {
		ammo = maxAmmo;
		extraAmmo = maxExtra;
		reloading = false;
		bursting = false;
		reloadTicked = false;
		timeout = 0;
		
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
		
		if (ammo == maxAmmo || extraAmmo == 0) { reloading = false; doneReloading = true; }
	}
	
	public void DrawAmmoCounter(Vector2 position, Texture2D tex, int lines) {
		Bars.graphic = tex;
		Rect brush = new Rect(position.x, position.y, tex.width * maxAmmo / lines, tex.height * lines);
		Bars.Draw(brush, new Vector2(maxAmmo/lines, lines), ammoPercent);
	}
	
	
	
	
	
}



























