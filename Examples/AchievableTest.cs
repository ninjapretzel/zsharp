using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievableTest : MonoBehaviour {

	void Start() {
		//Register Achievables with the system.
		//Different APIs may have different IDs for each achievement
		//This allows them to be created and associated as needed per platform.
		Achievables.Register("dfhjerthas23124sdgwer", new ExampleAchievableA());
		Achievables.Register("dhdfjfw2345f123124df1", new ExampleAchievableB());
		Achievables.Register("sdsdfh2352dr233412ss5", new ExampleAchievableC(5));
		Achievables.Register("dfjhjkewr1assrtgs2352", new ExampleAchievableC(8));
		
		//Send events to the achievement system with this function.
		//You can also send arguments using arguments if you want.
		
		//Empty argument call
		Achievables.Event("Trigger");
		
		//Call to same event, passing an argument
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
