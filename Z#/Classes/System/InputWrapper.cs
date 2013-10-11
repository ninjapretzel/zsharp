using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class InputWrapper {

	public static InputState[] states = new InputState[0];
	public static float buttonDownThreshold = 0.5f;
	
	public static bool GetButton(string name) {
		for(int i=0;i<states.Length;i++) {
			if(states[i].name==name) {
				return Input.GetAxisRaw(name)>=buttonDownThreshold;
			}
		}
		AddState(name);
		return Input.GetAxisRaw(name)>=buttonDownThreshold;
	}
	
	public static bool GetButtonDown(string name) {
		for(int i=0;i<states.Length;i++) {
			if (states[i].name==name) {
				return Input.GetAxisRaw(name)>=buttonDownThreshold && states[i].value<buttonDownThreshold;
				//states[i].Poll();
				//return down;
			}
		}
		AddState(name);
		return Input.GetAxisRaw(name)>=buttonDownThreshold;
	}
	
	public static bool GetButtonUp(string name) {
		for(int i=0;i<states.Length;i++) {
			if(states[i].name==name) {
				return Input.GetAxisRaw(name)<=buttonDownThreshold && states[i].value>buttonDownThreshold;
			}
		}
		AddState(name);
		return false;
	}
	
	public static float GetAxis(string name) {
		for(int i=0;i<states.Length;i++) {
			if(states[i].name==name) {
				return Input.GetAxisRaw(name);
			}
		}
		AddState(name);
		return Input.GetAxisRaw(name);
	}
	
	public static bool GetAxisUp(string name) {
		for(int i=0;i<states.Length;i++) {
			if(states[i].name==name) {
				return Input.GetAxisRaw(name)<=-buttonDownThreshold && states[i].value>-buttonDownThreshold;
			}
		}
		AddState(name);
		return Input.GetAxisRaw(name)<=-buttonDownThreshold;
	}
	
	public static bool GetAxisDown(string name) {
		return GetButtonDown(name);
	}
	
	public static void AddState(string name) {
		InputState[] newList = new InputState[states.Length+1];
		for (int i = 0; i < states.Length; i++) {
			newList[i]=states[i];
		}
		newList[newList.Length-1] = new InputState(name);
		states=newList;
	}
	
	public static void UpdateStates() {
		for (int i = 0; i < states.Length; i++) {
			states[i].Poll();
		}
		
	}
	
	public static void ClearStates() {
		for (int i = 0; i < states.Length; i++) {
			states[i].value = 0;
		}
	}
	
	public class InputState {
		public string name;
		public float value = 0.0f;
		
		public InputState(string name) {
			this.name = name;
			this.value = Input.GetAxisRaw(name);
		}
		
		public void Poll() {
			value = Input.GetAxisRaw(name);
		}
	}
	
}
