using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class DateF {

	public static float TimeUntilNow(this DateTime date) {
		return (float)DateTime.Now.Subtract(date).TotalSeconds;
	}
	
}
