using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// InputWrapper class
// Provides lower-level access to input, using Unity's built-in Input Manager
// by default, or allowing people to create their own configurations.
public static class InputWrapper {

	public const float axisDownMagnitude = 0.5f;
	public static Dictionary<string, PreviousAxis> prevAxes = new Dictionary<string, PreviousAxis>();
	public static ControlBindings bindings = new ControlBindings();
	public static Dictionary<string, string> binddict;

	public static void Start() {
		binddict = new Dictionary<string, string>();
		TextAsset file;
		if(SystemInfo.deviceModel == "OUYA OUYA Console") {
			file = Resources.Load("OuyaControls", typeof(TextAsset)) as TextAsset;
			if(file == null) {
				file = Resources.Load("OuyaDefaultControls", typeof(TextAsset)) as TextAsset;
			}
		} else {
			file = Resources.Load("Controls", typeof(TextAsset)) as TextAsset;
			if(file == null) {
				file = Resources.Load("DefaultControls", typeof(TextAsset)) as TextAsset;
			}
		}
		if(file != null) {
			binddict.LoadCSV(file.text);
		} else {
			Debug.Log("WARNING: DEFAULT CONTROLS FILE NOT FOUND!");
		}
		foreach (string key in binddict.Keys) {
			bindings.Load(key);
			if(key[key.Length-1] == '+' || key[key.Length-1] == '-') {
				prevAxes[key.Substring(0, key.Length-1)] = new PreviousAxis(key.Substring(0, key.Length-1));
			} else {
				prevAxes[key] = new PreviousAxis(key);
			}
		}
	}

	public static void LateUpdate() {
		foreach (string key in prevAxes.Keys) {
			prevAxes[key].val = GetAxis(key);
		}
	}

	public static float GetAxis(string axis) {
		float val = 0.0f;
		ControlBinding positiveBinding = null;
		ControlBinding negativeBinding = null;
		if(!bindings.ContainsKey(axis+"+") && !bindings.ContainsKey(axis+"-")) {
			positiveBinding = bindings[axis]; // A button, in this case
		} else {
			positiveBinding = bindings[axis+"+"];
			negativeBinding = bindings[axis+"-"];
		}
		if (positiveBinding.key != KeyCode.None) {
			if (Input.GetKey(positiveBinding.key)) { val += 1.0f; }
		} else {
			string boundPosAxis = positiveBinding.axis;
			if(boundPosAxis != null) {
				float postemp = Input.GetAxisRaw(boundPosAxis);
				bool posgtZero = positiveBinding.positive;
				if(posgtZero && postemp > 0) {
					val += postemp;
				} else if(!posgtZero && postemp < 0) {
					val -= postemp;
				}
			} else {
				Debug.Log("WARNING: Axis "+axis+"+ is unbound!");
			}
		}
		if(negativeBinding != null) {
			if (negativeBinding.key != KeyCode.None) {
				if (Input.GetKey(negativeBinding.key)) { val -= 1.0f; }
			} else {
				string negboundAxis = bindings[axis+"-"].axis;
				if(negboundAxis != null) {
					float negtemp = Input.GetAxisRaw(negboundAxis);
					bool neggtZero = bindings[axis+"-"].positive;
					if(neggtZero && negtemp > 0) {
						val -= negtemp;
					} else if(!neggtZero && negtemp < 0) {
						val += negtemp;
					}
				} else {
					Debug.Log("WARNING: Axis "+axis+"- is unbound!");
				}
			}
		}
		return val;
	}

	public static bool GetButton(string key) {
		try {
			return (Mathf.Abs(prevAxes[key].val) > axisDownMagnitude);
		} catch(KeyNotFoundException) {
			Debug.Log("The control "+key+" is undefined!");
			return false;
		}
	}

	public static bool GetButtonDown(string key) {
		try {
			return (Mathf.Abs(prevAxes[key].val) < axisDownMagnitude && Mathf.Abs(GetAxis(key)) >= axisDownMagnitude);
		} catch(KeyNotFoundException) {
			Debug.Log("The control "+key+" is undefined!");
			return false;
		}
	}

	public static bool GetButtonUp(string key) {
		try {
			return (Mathf.Abs(prevAxes[key].val) >= axisDownMagnitude && Mathf.Abs(GetAxis(key)) < axisDownMagnitude);
		} catch(KeyNotFoundException) {
			Debug.Log("The control "+key+" is undefined!");
			return false;
		}
	}
	
	// Iterates through the ENTIRE KeyCode enum to detect a key press.
	public static KeyCode GetPressedKey() {
		foreach (KeyCode kcode in KeyCode.GetValues(typeof(KeyCode))) {
			switch(kcode) {
				// Exclude generic joystick keycodes. We want specific joysticks.
				case KeyCode.JoystickButton0:
				case KeyCode.JoystickButton1:
				case KeyCode.JoystickButton2:
				case KeyCode.JoystickButton3:
				case KeyCode.JoystickButton4:
				case KeyCode.JoystickButton5:
				case KeyCode.JoystickButton6:
				case KeyCode.JoystickButton7:
				case KeyCode.JoystickButton8:
				case KeyCode.JoystickButton9:
				case KeyCode.JoystickButton10:
				case KeyCode.JoystickButton11:
				case KeyCode.JoystickButton12:
				case KeyCode.JoystickButton13:
				case KeyCode.JoystickButton14:
				case KeyCode.JoystickButton15:
				case KeyCode.JoystickButton16:
				case KeyCode.JoystickButton17:
				case KeyCode.JoystickButton18:
				case KeyCode.JoystickButton19:
					break;
				case KeyCode.Mouse0:
				case KeyCode.Mouse1:
				case KeyCode.Mouse2:
				case KeyCode.Mouse3:
				case KeyCode.Mouse4:
				case KeyCode.Mouse5:
				case KeyCode.Mouse6:
					if(Input.GetKey(kcode)) {
						if(Input.touches.Length == 0) {
							return kcode;
						}
					}
					break;
				default:
					if(Input.GetKey(kcode)) {
						return kcode;
					}
					break;
			}
			// Short circuit the function based on how many joysticks are connected
			if(kcode == KeyCode.Mouse6 && Input.GetJoystickNames().Length < 1) {
				return KeyCode.None;
			}
			if(kcode == KeyCode.Joystick1Button19 && Input.GetJoystickNames().Length < 2) {
				return KeyCode.None;
			}
			if(kcode == KeyCode.Joystick2Button19 && Input.GetJoystickNames().Length < 3) {
				return KeyCode.None;
			}
			if(kcode == KeyCode.Joystick3Button19 && Input.GetJoystickNames().Length < 4) {
				return KeyCode.None;
			}
		}
		return KeyCode.None;
	}
	
	public static string GetPressedAxis() {
		for(int i = 0; i < Input.GetJoystickNames().Length; i++) {
			for(int j = 0; j < 10; j++) {
				// Exclude broken, stuck, or useless axes here, by controller name
				#if UNITY_EDITOR
				if(j == 2 && (Input.GetJoystickNames()[i] == "XBOX 360 For Windows (Controller)" || Input.GetJoystickNames()[i] == "Controller (Xbox 360 Wireless Receiver for Windows)")) { continue; }
				#elif UNITY_ANDROID
				if(j == 2 && Input.GetJoystickNames()[i] == "Microsoft X-Box 360 pad") { continue; }
				#endif
				string axis = "Joystick"+(i+1)+"Axis"+j;
				if(Input.GetAxisRaw(axis) > axisDownMagnitude) {
					return axis+"+";
				}
				if(Input.GetAxisRaw(axis) < -axisDownMagnitude) {
					return axis+"-";
				}
			}
		}
		// For some reason, Unity detects touches on smartphone screens as mouse movement events. Ensure this doesn't become a problem.
		if(Input.touches.Length == 0) {
			if(Input.GetAxisRaw("MouseAxisX") > axisDownMagnitude) {
				return "MouseAxisX+";
			}
			if(Input.GetAxisRaw("MouseAxisX") < -axisDownMagnitude) {
				return "MouseAxisX-";
			}
			if(Input.GetAxisRaw("MouseAxisY") > axisDownMagnitude) {
				return "MouseAxisY+";
			}
			if(Input.GetAxisRaw("MouseAxisY") < -axisDownMagnitude) {
				return "MouseAxisY-";
			}
			if(Input.GetAxisRaw("MouseWheel") > axisDownMagnitude) {
				return "MouseWheel+";
			}
			if(Input.GetAxisRaw("MouseWheel") < -axisDownMagnitude) {
				return "MouseWheel-";
			}
		}
		return null;
	}

	// Attempts to detect input on any axis or button.
	// Returns true if input was captured. False otherwise.
	// Only creates the binding if captured input wasn't escape key/back button.
	public static bool CaptureInput(string inputFor) {
		string axis = GetPressedAxis();
		if(axis != null) {
			bool positive = (axis[axis.Length-1] == '+');
			bindings[inputFor] = new ControlBinding(axis.Substring(0, axis.Length-1), positive);
			return true;
		} else {
			KeyCode kcode = InputWrapper.GetPressedKey();
			if(kcode != KeyCode.None) {
				if(kcode != KeyCode.Escape) {
					bindings[inputFor] = new ControlBinding(kcode);
				}
				return true;
			}
		}
		return false;
	}
	
	public static void ClearBind(string inputFor) {
		bindings[inputFor].axis = null;
		bindings[inputFor].key = KeyCode.None;
		bindings[inputFor].Save(inputFor);
	}
	
	public static string GetHardwareName(string nameOf) {
		ControlBinding bind = bindings[nameOf];
		if(bind.axis == null) {
			return bind.key.ToString();
		} else {
			return bind.axis+(bind.positive ? "+" : "-");
		}
	}
	
	//Stores info of previous values to be able to treat sticks as buttons.
	public class PreviousAxis {
		public string name;
		public float val;
		
		public PreviousAxis(string name, float val) {
			this.name = name;
			this.val = val;
		}
		
		public PreviousAxis(string name) {
			this.name = name;
			this.val = 0;
		}
	}
}

// ControlBindings class
// Static class for all custom controls.

public class ControlBindings:Dictionary<string, ControlBinding> {
	
	public void Load(string name) {
		if(this.ContainsKey(name)) {
			this[name].Load(name);
		} else {
			this.Add(name, new ControlBinding(name));
		}
		
	}
	
	public bool Delete(string name) {
		if(!this.ContainsKey(name)) {
			return false;
		}
		this[name].Destroy(name);
		return true;
	}
	
	public void DeleteAll() {
		foreach(string key in this.Keys) {
			Delete(key);
		}
	}
	
	public void Save() {
		foreach (string name in this.Keys) {
			this[name].Save(name);
		}
	}

}

public class ControlBinding {
	public KeyCode key;
	public string axis;
	public bool positive = false;
	
	public ControlBinding() {
		this.key = KeyCode.None;
		this.axis = null;
	}
	
	public ControlBinding(string name) {
		if(PlayerPrefs.HasKey("controls_"+name+"_key")) {
			Load(name);
		} else {
			LoadDefault(name);
		}
	}
	
	public ControlBinding(KeyCode key) {
		this.key = key;
		this.axis = null;
	}
	
	public ControlBinding(string axis, bool positive) {
		this.axis = axis;
		this.key = KeyCode.None;
		this.positive = positive;
	}
	
	public void LoadDefault(String name) {
		/*if(binddict == null) {
			binddict = new Dictionary<string, string>();
			if(SystemInfo.deviceModel == "OUYA OUYA Console") {
				TextAsset file = Resources.Load("OuyaDefaultControls", typeof(TextAsset)) as TextAsset;
			} else {
				TextAsset file = Resources.Load("DefaultControls", typeof(TextAsset)) as TextAsset;
			}
			if(file == null) {
				Debug.Log("Default controls file not found!");
			} else {
				binddict.LoadCSV(file.text);
			}
		}*/
		string val = InputWrapper.binddict[name];
		this.key = KeyCode.None;
		this.axis = null;
		if(val[val.Length-1]=='+') {
			this.axis = val.Substring(0, val.Length - 1);
			this.positive = true;
		}
		else if(val[val.Length-1]=='-') {
			this.axis = val.Substring(0, val.Length - 1);
			this.positive = false;
		} else {
			try {
				this.key = (KeyCode)Enum.Parse(typeof(KeyCode), val);
			} catch(ArgumentException) {
				Debug.Log("INVALID HARDWARE NAME DEFINED IN DEFAULT CONTROLS FILE: "+val);
			}
		}
			
	}
	
	public void Save(string name) {
		PlayerPrefs.SetInt("controls_"+name+"_key", (int)key);
		PlayerPrefs.SetString("controls_"+name+"_axis", axis);
		PlayerPrefs.SetInt("controls_"+name+"_pos", positive ? 1 : 0);
	}
	
	public void Load(string name) {
		key = (KeyCode)PlayerPrefs.GetInt("controls_"+name+"_key");
		axis = PlayerPrefs.GetString("controls_"+name+"_axis");
		if(axis=="") {
			axis = null;
		}
		positive = (PlayerPrefs.GetInt("controls_"+name+"_pos")==1);
	}
	
	public void Destroy(string name) {
		LoadDefault(name);
		Save(name);
	}
}