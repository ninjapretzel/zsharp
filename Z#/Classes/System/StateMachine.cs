using UnityEngine;
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
		currentState.Enter(owner);
	}
	
	public StateMachine(State<T> initialState, T target) {
		currentState = initialState;
		owner = target;
		currentState.Enter(owner);
	}
	
	//Switch and return if state was actually switched.
	public bool Switch(State<T> s) {
		if (s == currentState) { return false; }
		
		previousState = currentState;
		currentState = s;
		previousState.Exit(owner);
		currentState.Enter(owner);		
		
		switchedLastFrame = true;
		doneSwitching = false;
		
		return true;
	}
	
	public void Update() { 
		if (doneSwitching) { doneSwitching = false; switchedLastFrame = false; }
		if (switchedLastFrame) { currentState.EnterFrame(owner); doneSwitching = true; }
		currentState.Update(owner); 
		
	}
	
	public void LateUpdate() { currentState.LateUpdate(owner); }
	public void FixedUpdate() { currentState.FixedUpdate(owner); }
	
	public void OnGUI() {		
		currentState.OnGUI(owner); 
		if (switchedLastFrame) { currentState.EnterGUI(owner); GUI.FocusControl("nothing"); }
		
	}
	
	public void OnCollisionEnter(Collision c) { currentState.OnCollisionEnter(owner, c); }
	public void OnCollisionStay(Collision c) { currentState.OnCollisionStay(owner, c); }
	public void OnCollisionExit(Collision c) { currentState.OnCollisionExit(owner, c); }
	
	public void OnTriggerEnter(Collider c) { currentState.OnTriggerEnter(owner, c); }
	public void OnTriggerStay(Collider c) { currentState.OnTriggerStay(owner, c); }
	public void OnTriggerExit(Collider c) { currentState.OnTriggerExit(owner, c); }
	
	public void OnMouseEnter() { currentState.OnMouseEnter(owner); }
	public void OnMouseOver() { currentState.OnMouseOver(owner); }
	public void OnMouseExit() { currentState.OnMouseExit(owner); }
	public void OnMouseDown() { currentState.OnMouseDown(owner); }
	public void OnMouseUp() { currentState.OnMouseUp(owner); }
	public void OnMouseUpAsButton() { currentState.OnMouseUpAsButton(owner); }

}

//State blueprint
public class State<T> where T : Component {
	public static State<T> baseInstance = new State<T>();
	
	public virtual void Enter(T owner) {}
	public virtual void Exit(T owner) {}
	
	public virtual void EnterGUI(T owner) {}
	public virtual void EnterFrame(T owner) {}
	
	public virtual void Update(T owner) {}
	public virtual void LateUpdate(T owner) {}
	public virtual void FixedUpdate(T owner) {}
	
	public virtual void OnGUI(T owner) {}
	
	public virtual void OnTriggerEnter(T owner, Collider c) {}
	public virtual void OnTriggerStay(T owner, Collider c) {}
	public virtual void OnTriggerExit(T owner, Collider c) {}
	
	public virtual void OnCollisionEnter(T owner, Collision c) {}
	public virtual void OnCollisionStay(T owner, Collision c) {}
	public virtual void OnCollisionExit(T owner, Collision c) {}
	
	public virtual void OnMouseEnter(T owner) {}
	public virtual void OnMouseOver(T owner) {}
	public virtual void OnMouseExit(T owner) {}
	public virtual void OnMouseDown(T owner) {}
	public virtual void OnMouseUp(T owner) {}
	public virtual void OnMouseUpAsButton(T owner) {}
	
}








