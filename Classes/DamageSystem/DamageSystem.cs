using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Attack : Table {
	
	public Attack() {
		Add("slash", 10);
	}
	
	public float total {
		get {
			return this.Sum();
		}
	}
	
	public float GetTotal(Table mods) {
		float sum = 0;
		foreach (string key in Keys) {
			sum += this[key] * (mods.ContainsKey(key) ? mods[key] : 1);
		}
		return sum;
	}
	
	
	
	
}

public static class HealthModList {
	public static Desk mods;
	
	static HealthModList() {
		mods = new Desk();
		string str = DataF.Load("HealthMods");
		if (str == "") { str = DataF.Load("BaseHealthMods"); }
		mods.LoadCSV(str);
		
	}
	
}

[System.Serializable]
public class Health {
	public string name;
	private Table mods;
	
	public float max;
	public float value;
	
	public float percentage { get { return value / max; } }
	
	public float regen;
	public float armor;
	
	public bool protective;
	
	public float timeSinceHit;
	public float regenThreshold;
	
	public void Update() { Update(Time.deltaTime); }
	public void Update(float time) { 
		timeSinceHit += Time.deltaTime;
		if (timeSinceHit > regenThreshold) {
			value = Mathf.Min(value + regen * time, max);
		}
	}
	
	private void Defaults() {
		name = "Health";
		max = 100;
		value = 100;
		regen = 0;
		armor = 0;
		timeSinceHit = 0;
		regenThreshold = 0;
		protective = false;
	}	
	
	public Health() {
		Defaults();
	}
	
	public Health(string n) {
		Defaults();
		name = n;
	}
	
	public Health(string n, float m) {
		Defaults();
		name = n;
		max = m;
		value = max;
	}
	
	public Health(string n, float m, Table modz) {
		Defaults();
		name = n;
		max = m;
		value = max;
		mods = modz;
	}
	
	public void Fill() {
		value = max;
	}
	
	public float Hit(string s, float a) {
		timeSinceHit = 0;
		float mod = 1;
		if (mods != null && mods.ContainsKey(s)) { mod = mods[s]; }
		return Hit(a * mod);
	}
	
	public float Hit(float damage) {
		timeSinceHit = 0;
		float p = damage * armor;
		float f = damage - p;
		
		p += GiveDamage(f);
		if (protective) { return 0; }
		return p;
	}
	
	public float GiveDamage(float f) {
		if (f >= value) {
			f -= value;
			value = 0;
		} else {
			value -= f;
			f = 0;
		}
		return f;
	}

	public float GetMod(string s) { return mods.ContainsKey(s) ? mods[s] : 1.0f; }
	
	
}
































