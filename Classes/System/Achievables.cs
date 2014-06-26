using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

//This is the delegate type to create.
public delegate Achievable AchievableAction(string args);

[System.Serializable]
public class Achievable {
	
	//The ID of this achievable within some API
	public string id = "achievement_dummy";
	
	//The display name of this achievement
	public string display = "Dummy Achievable";
	
	//Has this achievable been unlocked?
	public bool unlocked = false;
	
	//Is this achievable visible to the users?
	public bool visible = true;
	
	//Was this achievable JUST earned?
	public bool justEarned { 
		get { 
			if (unlocked) { return false; }
			unlocked = earned;
			return unlocked;
		}
	}
	//Property to wrap Earned() function.
	public bool earned { get { return Earned(); } }
	
	//Constructor
	public Achievable() {
		Achievables.Register(this);
	}
	
	//Helper function for inside Register()
	public void Register(string name, AchievableAction action) { Achievables.AddEvent(name, action); }
	
	//Override this function with the proper logic for unlocking the achievable.
	public virtual bool Earned() { return false; }
	
	//Override this function to register all events with the Achievables class
	public virtual void Register() { }
	
	public virtual void Save() { }
	public virtual void Load() { }
	
}

public class ExampleAchievableA : Achievable {
	
	bool wasTriggered = false;
	
	public override bool Earned() { 
		return wasTriggered;
	}
	
	//This is where we add the delegates to the list in achievables
	//Only works on this one class
	public override void Register() {
		id = "triggerfinger";
		display = "Trigger Finger";
		
		Register("Trigger", Triggered);
		
	}
	
	public Achievable Triggered(string args) {
		Debug.Log("A: " + args);
		wasTriggered = true;
		return this;
	}	
	
}

public class ExampleAchievableB : Achievable {
	
	int poopCount = 0;
	
	public override bool Earned() { 
		return poopCount >= 10;
	}
	
	//This is where we add the delegates to the list in achievables
	public override void Register() {
		id = "sirpoopypants";
		display = "Sir Poopy Pants";
		
		Register("Poop", Triggered);
		Register("Trigger", Triggered);
		
	}
	
	public Achievable Triggered(string args) {
		poopCount += 1;
		Debug.Log("B POOPED: " + poopCount);
		return this;
	}	
	
}


public static class Achievables {
	
	static Achievables() {
		achievables = new Dictionary<string, Achievable>();
		events = new Dictionary<string, AchievableAction>();
		
	}
	
	public static Dictionary<string, Achievable> achievables;
	public static Dictionary<string, AchievableAction> events;
	
	public static void Register(Achievable achievable) {
		achievable.Register();
		achievables.Add(achievable.id, achievable);
	}
	
	public static void AddEvent(string name, AchievableAction action) {
		if (events.ContainsKey(name)) {
			events[name] += action;
		} else {
			events[name] = new AchievableAction(action);
		}
		
	}
	
	public static void Event(string name) { Event(name, ""); }
	public static void Event(string name, string args) {
		if (events.ContainsKey(name)) {
			int i = 0;
			Debug.Log("Event " + name + " passed with args:\n" + args);
			foreach (AchievableAction action in events[name].GetInvocationList()) {
				Debug.Log("Calling action #" + ++i);
				Achievable achievable = action(args);
				
				if (achievable.justEarned) {
					//TBD: Send achievement earned message to API
					Debug.Log("Achievable Earned " + achievable.display + "!");
				}
				
				
			}
		} else {
			Debug.Log("Event " + name + " passed with args:\n" + args);
			Debug.Log("But " + name + " has not been registered.");
		}
	}
	
	public static void Save() {
		
	}
	
	public static void Load() {
		
	}
	
}