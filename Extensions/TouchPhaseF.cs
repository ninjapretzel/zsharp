using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TouchPhaseF {
	
	public static bool IsPress(this TouchPhase t) { return t == TouchPhase.Began; }
	public static bool IsHold(this TouchPhase t) { return t == TouchPhase.Moved || t == TouchPhase.Stationary; } 
	public static bool IsRelease(this TouchPhase t) { return t == TouchPhase.Ended || t == TouchPhase.Canceled; } 
	
	public static bool IsBegan(this TouchPhase t) { return t == TouchPhase.Began; }
	public static bool IsMoved(this TouchPhase t) { return t == TouchPhase.Moved; }
	public static bool IsStationary(this TouchPhase t) { return t == TouchPhase.Stationary; }
	public static bool IsEnded(this TouchPhase t) { return t == TouchPhase.Ended; }
	public static bool IsCanceled(this TouchPhase t) { return t == TouchPhase.Canceled; }
	
	
}



















