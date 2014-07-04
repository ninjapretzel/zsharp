
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SortChildrenByName : MonoBehaviour {
	
	
	[MenuItem ("Edit/Sort Children By Name")]
	public static void Sort() {
		Transform selection = Selection.activeTransform;
		Sort(selection);
	}
	
	public static void Sort(Transform root) {
		Dictionary<string, List<Transform>> children = new Dictionary<string, List<Transform>>();
		List<string> nameList = new List<string>();
		
		foreach (Transform t in root.GetChildren()) { 
			string name = t.gameObject.name;
			if (!nameList.Contains(name)) { nameList.Add(name); }
			if (children.ContainsKey(name)) {
				children[name].Add(t);
			} else {
				List<Transform> newList = new List<Transform>();
				newList.Add(t);
				children.Add(name, newList);
			}
		}
		
		nameList.Sort();
		
		int num = root.childCount;
		int i = 0;
		foreach (string name in nameList) {
			List<Transform> transforms = children[name];
			foreach (Transform t in transforms) {
				t.SetSiblingIndex(i++);
			}
			
		}
		
		
	}
	
	
}

#endif