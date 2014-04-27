using UnityEngine;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

public class SavesState : MonoBehaviour {
	
	public static bool restore = false;
	public static bool save = false;
	
	// Transform properties
	[NonSerialized] private Vector3 position;
	[NonSerialized] private Quaternion rotation;
	[NonSerialized] private Vector3 scale;
	[NonSerialized] private Transform savedParent;
	// Attached Behaviours
	[NonSerialized] private bool[] enabledBehaviours; // Behaviour.enabled is not a field, it's a property. Store it separately.
	[NonSerialized] private Dictionary<Type, Dictionary<FieldInfo, System.Object>> savedBehaviours = null;
	
	public void Start() {
		SaveState();
		
	}
	
	public void Update() {
		if(restore) {
			Restore();
		} else if(save) {
			SaveState();
		}
		
	}
	
	public void LateUpdate() {
		restore = false;
		save = false;
		
	}
	
	public void SaveState() {
		Behaviour[] behaviours = gameObject.GetComponents<Behaviour>();
		enabledBehaviours = new bool[behaviours.Length - 1];
		savedBehaviours = new Dictionary<Type, Dictionary<FieldInfo, System.Object>>();
		// Save transform properties
		position = transform.localPosition;
		rotation = transform.localRotation;
		scale = transform.localScale;
		savedParent = transform.parent;
		int i = 0;
		foreach(Behaviour c in behaviours) {
			// Use reflection to get the Type of this
			string name = c.GetType().Name;
			if(name != this.GetType().Name) {
				// Get all fields in this Behaviour
				FieldInfo[] fields = c.GetType().GetFields();
				Dictionary<FieldInfo, System.Object> savedFields = new Dictionary<FieldInfo, System.Object>();
				// Save them in a dictionary
				foreach(FieldInfo f in fields) {
					savedFields.Add(f, f.GetValue(c));
				}
				savedBehaviours.Add(c.GetType(), savedFields);
				// Also save the enabled state
				enabledBehaviours[i] = c.enabled;
				i++;
			}
		}
		
	}
	
	public void Restore() {
		// Restore transform properties
		transform.localPosition = position;
		transform.localRotation = rotation;
		transform.localScale = scale;
		transform.parent = savedParent;
		// Get all Behaviours currently attached to this object
		Behaviour[] behaviours = gameObject.GetComponents<Behaviour>();
		foreach(Behaviour c in behaviours) {
			string name = c.GetType().Name;
			if(name != "SavesStateOnCheckpoint") {
				// Delete them all, some may have been added since the last checkpoint
				Behaviour.Destroy(c);
			}
		}
		int i = 0;
		foreach(Type type in savedBehaviours.Keys) {
			// Add back the saved components
			Behaviour current = gameObject.AddComponent(type.Name) as Behaviour;
			foreach(FieldInfo field in savedBehaviours[type].Keys) {
				// Even though "current" is a Behaviour above, setting fields of derivative classes works through reflection, fortunately
				field.SetValue(current, savedBehaviours[type][field]);
			}
			// Restore enabled state
			current.enabled = enabledBehaviours[i];
			i++;
		}
		
	}
}