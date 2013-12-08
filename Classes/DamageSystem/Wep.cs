using UnityEngine;
using System.Collections;

public interface Wep {
	DealsDamage projectile { get; }
	bool uniformDirection { get; }
	bool uniformPosition { get; }
	bool spins { get; }
	Vector3 offset { get; }
	Vector3 mirrorOffset { get; }
	float pellets { get; }
	float kickback { get; }
	float spinPerBurst { get; }
	float repeatAngle { get; }
	float recoilAbsorb { get; }
	float bursts { get; }
	float burstTime { get; }
	float fireTimeHold { get; }
	float fireTimeDown { get; }
	float reloadTime { get; }
	float reloadRounds { get; }
	float spread { get; }
	float ammoUse { get; }
	float maxAmmo { get; }
	float maxExtra { get; }
	
	
	
	Attack damage { get; set; }
	float reloadPercent { get; }
	float ammoPercent { get; }
	float extraAmmoPercent { get; }
	bool hasAmmo { get; }
	bool doesBurst { get; }
	bool canFire { get; }
	bool canFireBurst { get; }
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
	void ReloadTick();
	
	
	
	
	
	
}
