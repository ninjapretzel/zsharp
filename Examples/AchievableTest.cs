using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievableTest : MonoBehaviour {

	void Start() {
		//Load achievements simply constructing them
		//They add themselves into the static database after they are constructed.
		new ExampleAchievableA();
		new ExampleAchievableB();
		new ExampleAchievableC(5);
		new ExampleAchievableC(8);
		
		//Send events to the achievement system with this function.
		//You can also send arguments using arguments if you want.
		Achievables.Event("Trigger");
		Achievables.Event("Trigger", "blah");
		
		
		for (int i = 0; i < 16; i++) {
			if (i == 5) { 
				Achievables.Event("CleanUpPoop");
			}
			Achievables.Event("Poop");
			Achievables.Event("Trigger"+i);
		}
		
		
	}
	
}
