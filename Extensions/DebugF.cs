using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DebugF {
	public static void DLog(this System.Object o) { Debug.Log(o); }
	
	#if UNITY_DEBUG
	public static void UDLog(this System.Object o) { o.DLog(); }
	#else
	public static void UDLog(this System.Object o) {  }
	#endif
	
}
