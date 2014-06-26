using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

//This is the delegate type to create.
public delegate Achievable AchievableAction(string args);
public delegate void AchievableEarnedCallback(bool success);

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
	
	public AchievableEarnedCallback earnedCallback;
	
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
	//Calls an overridable 
	public Achievable() {
		Init();
	}
	
	//Helper function for inside Register()
	public void Register(string name, AchievableAction action) { Achievables.AddEvent(name, action); }
	
	public void OnEarnedResponse(bool success) {
		if (success) { 
			if (earnedCallback != null) {
				earnedCallback(success); 
			}
			
		} else {
			unlocked = false;
			Debug.Log("Achievable " + id + " earned, but sending failed");
		}
	}
	
	public void SaveData() { 
		Save();
		PlayerPrefs.SetInt(id + "_unlocked", unlocked ? 0 : 1);
	}
	
	public void LoadData() {
		Load();
		unlocked = (PlayerPrefs.GetInt(id + "_unlocked") == 1);
	}
	
	
	//This can be overridden if one doesn't want the default behaviour of immediately registering the achievable.
	//This is useful if you want to provide extra constructors.
	public virtual void Init() { Achievables.Register(this); }
	
	//Override this function with the proper logic for unlocking the achievable.
	public virtual bool Earned() { return false; }
	
	//Override this function to register all events with the Achievables class
	public virtual void Register() { }
	
	//Overload these to save/load relevant information for the progress of the achievable
	public virtual void Save() { }
	public virtual void Load() { }
	
}

#region EXAMPLES

//Example A is awarded when triggered.
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

//Example B implements a counter to track achievable progress.
public class ExampleAchievableB : Achievable {
	
	int poopCount = 0;
	
	public override bool Earned() { 
		return poopCount >= 10;
	}
	
	//This is where we add the delegates to the list in achievables
	public override void Register() {
		id = "sirpoopypants";
		display = "Sir Poopy Pants";
		
		//call Trigger on "Poop" or "Trigger"
		Register("Poop", Triggered);
		Register("Trigger", Triggered);
		
		//call Reset on "CleanUpPoop"
		Register("CleanUpPoop", Reset);
		
	}
	
	public Achievable Reset(string args) {
		poopCount = 0;
		return this;
	}
	
	public Achievable Triggered(string args) {
		poopCount += 1;
		Debug.Log("B POOPED: " + poopCount);
		return this;
	}	
	
}

//Example C overrides the Init() function and provides its own constructor.
public class ExampleAchievableC : Achievable {
	
	int poopCount = 0;
	int i;
	
	public ExampleAchievableC(int index) {
		i = index;
		id = "exampleC_id" + i;
		Achievables.Register(this);
	}
	
	public override void Init() { }
	
	public override bool Earned() { 
		return poopCount >= i;
	}
	
	//This is where we add the delegates to the list in achievables
	public override void Register() {
		Debug.Log("C: " + id);
		Register("Trigger"+i, Triggered);
		
	}
	
	public Achievable Triggered(string args) {
		poopCount += 1;
		Debug.Log("C" + i + " Triggered: " + poopCount);
		return this;
	}	
	
}

#endregion 

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
	
	//Save/Load Functions
	public static void Save() {
		foreach (Achievable achievable in achievables.Values) {
			achievable.SaveData();
		}
	}
	
	public static void Load() {
		foreach (Achievable achievable in achievables.Values) {
			achievable.LoadData();
		}
	}
	
}