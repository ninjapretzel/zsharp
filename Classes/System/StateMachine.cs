using UnityEngine;
using System;
using System.Collections;

//Simple state machine implementation
//
//
public class StateMachine<T> where T : Component {
	public State<T> currentState;
	public State<T> previousState;
	public T owner;
	
	private bool switchedLastFrame = false;
	private bool doneSwitching = false;
	
	public StateMachine(T target) {
		currentState = State<T>.baseInstance;
		owner = target;
		currentState.Enter();
	}
	
	public StateMachine(State<T> initialState, T target) {
		currentState = initialState;
		owner = target;
		currentState.Enter();
	}
	
	//Switch and return if state was actually switched.
	public bool Switch(State<T> s) {
		if (s == null) { return Switch(State<T>.baseInstance); }
		if (s == currentState) { return false; }
		s.target = owner;
		
		previousState = currentState;
		currentState = s;
		previousState.Exit();
		currentState.Enter();		
		
		switchedLastFrame = true;
		doneSwitching = false;
		
		return true;
	}
	
	public void Update() { 
		if (doneSwitching) { doneSwitching = false; switchedLastFrame = false; }
		if (switchedLastFrame) { currentState.EnterFrame(); doneSwitching = true; }
		currentState.Update();
		if (previousState != null) { previousState.OffUpdate(); }
	}
	
	public void LateUpdate() { 
		currentState.LateUpdate(); 
		if (previousState != null) { previousState.OffLateUpdate(); }
		
	}
	
	public void FixedUpdate() { 
		currentState.FixedUpdate();
		if (previousState != null) { previousState.OffFixedUpdate(); }
		
	}
	
	public void OnGUI() {
		currentState.OnGUI(); 
		if (switchedLastFrame) { currentState.EnterGUI(); GUI.FocusControl("nothing"); }
		if (previousState != null) { previousState.OffGUI(); }
		
	}
	
	public void OnCollisionEnter(Collision c) { currentState.OnCollisionEnter(c); }
	public void OnCollisionStay(Collision c) { currentState.OnCollisionStay(c); }
	public void OnCollisionExit(Collision c) { currentState.OnCollisionExit(c); }
	
	public void OnTriggerEnter(Collider c) { currentState.OnTriggerEnter(c); }
	public void OnTriggerStay(Collider c) { currentState.OnTriggerStay(c); }
	public void OnTriggerExit(Collider c) { currentState.OnTriggerExit(c); }
	
	public void OnMouseEnter() { currentState.OnMouseEnter(); }
	public void OnMouseOver() { currentState.OnMouseOver(); }
	public void OnMouseExit() { currentState.OnMouseExit(); }
	public void OnMouseDown() { currentState.OnMouseDown(); }
	public void OnMouseUp() { currentState.OnMouseUp(); }
	public void OnMouseUpAsButton() { currentState.OnMouseUpAsButton(); }

}

//State blueprint
public class State<T> where T : Component {
	public static State<T> baseInstance = new State<T>();
	public T target;
	
	public State() { target = null; }
	
	public virtual void Action(string a) { Debug.LogWarning("Missing override to Action(string a) in " + GetType()); }
	
	public virtual void Enter() {}
	public virtual void Exit() {}
	
	public virtual void EnterGUI() {}
	public virtual void EnterFrame() {}
	
	public virtual void Update() {}
	public virtual void LateUpdate() {}
	public virtual void FixedUpdate() {}
	public virtual void OnGUI() {}
	
	public virtual void OffUpdate() {}
	public virtual void OffLateUpdate() {}
	public virtual void OffFixedUpdate() {}
	public virtual void OffGUI() {}
	
	public virtual void OnTriggerEnter(Collider c) {}
	public virtual void OnTriggerStay(Collider c) {}
	public virtual void OnTriggerExit(Collider c) {}
	
	public virtual void OnCollisionEnter(Collision c) {}
	public virtual void OnCollisionStay(Collision c) {}
	public virtual void OnCollisionExit(Collision c) {}
	
	public virtual void OnMouseEnter() {}
	public virtual void OnMouseOver() {}
	public virtual void OnMouseExit() {}
	public virtual void OnMouseDown() {}
	public virtual void OnMouseUp() {}
	public virtual void OnMouseUpAsButton() {}

	///Depreciated functions
	[Obsolete("No longer need to pass in the owner. Use Enter() ")] public virtual void Enter(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use Exit() ")] public virtual void Exit(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use EnterGUI() ")] public virtual void EnterGUI(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use EnterFrame() ")] public virtual void EnterFrame(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use Update() ")] public virtual void Update(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use LateUpdate() ")] public virtual void LateUpdate(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use FixedUpdate() ")] public virtual void FixedUpdate(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnGUI() ")] public virtual void OnGUI(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnTriggerEnter(Collider c) ")] public virtual void OnTriggerEnter(T owner, Collider c) {}
	[Obsolete("No longer need to pass in the owner. Use OnTriggerStay(Collider c) ")] public virtual void OnTriggerStay(T owner, Collider c) {}
	[Obsolete("No longer need to pass in the owner. Use OnTriggerExit(Collider c) ")] public virtual void OnTriggerExit(T owner, Collider c) {}
	[Obsolete("No longer need to pass in the owner. Use OnCollisionEnter(Collision c) ")] public virtual void OnCollisionEnter(T owner, Collision c) {}
	[Obsolete("No longer need to pass in the owner. Use OnCollisionStay(Collision c) ")] public virtual void OnCollisionStay(T owner, Collision c) {}
	[Obsolete("No longer need to pass in the owner. Use OnCollisionExit(Collision c) ")] public virtual void OnCollisionExit(T owner, Collision c) {}
	[Obsolete("No longer need to pass in the owner. Use OnMouseEnter() ")] public virtual void OnMouseEnter(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnMouseOver() ")] public virtual void OnMouseOver(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnMouseExit() ")] public virtual void OnMouseExit(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnMouseDown() ")] public virtual void OnMouseDown(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnMouseUp() ")] public virtual void OnMouseUp(T owner) {}
	[Obsolete("No longer need to pass in the owner. Use OnMouseUpAsButton() ")] public virtual void OnMouseUpAsButton(T owner) {}
	
}







