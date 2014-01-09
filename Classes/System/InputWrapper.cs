using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// InputWrapper class
// Provides lower-level access to input, using Unity's built-in Input Manager
// by default, or allowing people to create their own configurations.
public static class InputWrapper {

	public const float axisDownMagnitude = 0.5f;
	public static Dictionary<string, PreviousAxis> prevAxes = new Dictionary<string, PreviousAxis>();
	public static ControlBindings bindings = new ControlBindings();
	public static Dictionary<string, Set<string>> defaults;
	
	static InputWrapper() {
		defaults = new Dictionary<string, Set<string>>();
		
		TextAsset file = Resources.Load("DefaultControls", typeof(TextAsset)) as TextAsset;
		LoadFromTextasset(file);
		LoadCustomPlayerprefs();
	}
	
	public static void LoadFromTextasset(TextAsset ta) {
		if (ta != null) { LoadFromString(ta.text); }
		else { LogWarning("CONTROLS FILE NOT FOUND!"); return; }
		
	}
	
	public static void LoadFromString(string st) {
		defaults.LoadCSV(st.ConvertNewlines());
		
		foreach (string key in defaults.Keys) {
			bindings.Load(key);
			prevAxes = new Dictionary<string, PreviousAxis>();
			if(key[key.Length-1] == '+' || key[key.Length-1] == '-') {
				prevAxes[key.Substring(0, key.Length-1)] = new PreviousAxis(key.Substring(0, key.Length-1));
			} else {
				prevAxes[key] = new PreviousAxis(key);
			}
		}
		
	}
	
	public static void LoadCustomPlayerprefs() {
		foreach(string key in defaults.Keys) {
			bindings.Load(key);
		}
	}
	
	static void Log(string s) { Debug.Log("InputWrapper: " + s); }
	static void LogWarning(string s) { Debug.LogWarning("InputWrapper: " + s); }

	public static void LateUpdate() {
		foreach (string key in prevAxes.Keys) {
			prevAxes[key].val = GetAxis(key);
		}
	}

	public static float GetAxis(string axis) {
		float val = 0.0f;
		Set<ControlBinding> positiveBinding = null;
		Set<ControlBinding> negativeBinding = null;
		
		if(!bindings.ContainsKey(axis+"+") && !bindings.ContainsKey(axis+"-")) {
			positiveBinding = bindings[axis]; // A button, in this case
		} else {
			positiveBinding = bindings[axis+"+"];
			negativeBinding = bindings[axis+"-"];
		}
		
		for(int i = 0;i < positiveBinding.Count; i++) {
			if (positiveBinding[i].key != KeyCode.None) {
				if (Input.GetKey(positiveBinding[i].key)) { val += 1.0f; }
			} else {
				string boundPosAxis = positiveBinding[i].axis;
				if(boundPosAxis != null) {
					float postemp = Input.GetAxisRaw(boundPosAxis);
					bool posgtZero = positiveBinding[i].positive;
					if(posgtZero && postemp > 0) {
						val += postemp;
					} else if(!posgtZero && postemp < 0) {
						val -= postemp;
					}
				} else {
					LogWarning("Axis " + axis + "+ is unbound!");
				}
			}
		}
		
		if (negativeBinding != null) {
			for(int i = 0;i < negativeBinding.Count; i++) {
				if (negativeBinding[i].key != KeyCode.None) {
					if (Input.GetKey(negativeBinding[i].key)) { val -= 1.0f; }
				} else {
					string negboundAxis = negativeBinding[i].axis;
					if(negboundAxis != null) {
						float negtemp = Input.GetAxisRaw(negboundAxis);
						bool neggtZero = negativeBinding[i].positive;
						if(neggtZero && negtemp > 0) {
							val -= negtemp;
						} else if(!neggtZero && negtemp < 0) {
							val += negtemp;
						}
					} else {
						LogWarning("Axis "+axis+"- is unbound!");
					}
				}
			}
		}
		return val;
	}

	public static bool GetButton(string key) {
		try {
			return (Mathf.Abs(prevAxes[key].val) > axisDownMagnitude);
		} catch(KeyNotFoundException) {
			LogWarning("The control "+key+" is undefined!");
			return false;
		}
	}

	public static bool GetButtonDown(string key) {
		try {
			return (Mathf.Abs(prevAxes[key].val) < axisDownMagnitude && Mathf.Abs(GetAxis(key)) >= axisDownMagnitude);
		} catch(KeyNotFoundException) {
			LogWarning("The control "+key+" is undefined!");
			return false;
		}
	}

	public static bool GetButtonUp(string key) {
		try {
			return (Mathf.Abs(prevAxes[key].val) >= axisDownMagnitude && Mathf.Abs(GetAxis(key)) < axisDownMagnitude);
		} catch(KeyNotFoundException) {
			LogWarning("The control "+key+" is undefined!");
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
			try {
				if(Input.GetAxisRaw("MouseAxisX") > axisDownMagnitude) {
					return "MouseAxisX+";
				}
				if(Input.GetAxisRaw("MouseAxisX") < -axisDownMagnitude) {
					return "MouseAxisX-";
				}
			} catch(UnityException) { ; }
			try {
				if(Input.GetAxisRaw("MouseAxisY") > axisDownMagnitude) {
					return "MouseAxisY+";
				}
				if(Input.GetAxisRaw("MouseAxisY") < -axisDownMagnitude) {
					return "MouseAxisY-";
				}
			} catch(UnityException) { ; }
			try {
				if(Input.GetAxisRaw("MouseWheel") > axisDownMagnitude) {
					return "MouseWheel+";
				}
				if(Input.GetAxisRaw("MouseWheel") < -axisDownMagnitude) {
					return "MouseWheel-";
				}
			} catch(UnityException) { ; }
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
			bindings[inputFor].Add(new ControlBinding(axis.Substring(0, axis.Length-1), positive));
			bindings.Save(inputFor);
			return true;
		} else {
			KeyCode kcode = InputWrapper.GetPressedKey();
			if(kcode != KeyCode.None) {
				if(kcode != KeyCode.Escape) {
					bindings[inputFor].Add(new ControlBinding(kcode));
					bindings.Save(inputFor);
				}
				return true;
			}
		}
		return false;
	}

	// Attempts to detect input on any axis or button.
	// Returns true if input was captured. False otherwise.
	// Only creates the binding if captured input wasn't escape key/back button.
	// Binds it to the specified binding slot for the specified control
	public static bool CaptureInput(string inputFor, int slot) {
		string axis = GetPressedAxis();
		if(axis != null) {
			bool positive = (axis[axis.Length-1] == '+');
			if(bindings[inputFor].Count - 1 < slot) {
				bindings[inputFor].Add(new ControlBinding(axis.Substring(0, axis.Length-1), positive));
			} else {
				bindings[inputFor][slot] = new ControlBinding(axis.Substring(0, axis.Length-1), positive);
			}
			bindings.Save(inputFor);
			return true;
		} else {
			KeyCode kcode = InputWrapper.GetPressedKey();
			if(kcode != KeyCode.None) {
				if(kcode != KeyCode.Escape) {
					if(bindings[inputFor].Count - 1 < slot) {
						bindings[inputFor].Add(new ControlBinding(kcode));
					} else {
						bindings[inputFor][slot] = new ControlBinding(kcode);
					}
					bindings.Save(inputFor);
				} else {
					if(bindings[inputFor].Count > slot) {
						bindings[inputFor].RemoveAt(slot);
					}
				}
				return true;
			}
		}
		return false;
	}
	
	public static void ClearBind(string inputFor) {
		bindings[inputFor] = new Set<ControlBinding>();
		bindings.Save(inputFor);
	}
	
	public static void ClearBind(string inputFor, int index) {
		bindings[inputFor].RemoveAt(index);
	}
	
	public static void ClearBind(string inputFor, string name) {
		bindings[inputFor].Remove(new ControlBinding(name));
	}
	
	public static string GetHardwareName(string nameOf) {
		string res = "";
		foreach(ControlBinding bind in bindings[nameOf]) {
			res += bind.ToString() + ",";
		}
		if(res.Length == 0) { return ""; }
		return res.Substring(0,res.Length-1); // Trim the last unnecessary comma
	}
	
	public static string[] GetHardwareNames(string nameOf) {
		string[] stringArray = new string[bindings[nameOf].Count];
		for(int i=0;i<stringArray.Length;i++) {
			stringArray[i] = bindings[nameOf][i].ToString();
		}
		return stringArray;
	}
	
	// Check if any control already has a certain hardware input bound to it
	public static bool CheckDuplicate(string hardwareName) {
		foreach(string key in bindings.Keys) {
			foreach(ControlBinding binding in bindings[key]) {
				if(binding.ToString() == hardwareName) { return true; }
			}
		}
		return false;
	}
	
	// Check if any control has a certain hardware input bound to it, and remove it
	public static void RemoveAllHardwareBind(string hardwareName) {
		ControlBinding removeMe = new ControlBinding(hardwareName);
		foreach(string key in bindings.Keys) {
			if(bindings[key].Contains(removeMe)) {
				bindings[key].Remove(removeMe);
			}
		}
	}
	
	// Check if any control has a certain hardware input bound to it, and remove it, unless the control name is the passed exception
	public static void RemoveAllHardwareBind(string hardwareName, string exception) {
		ControlBinding removeMe = new ControlBinding(hardwareName);
		foreach(string key in bindings.Keys) {
			if(key == exception) { continue; }
			if(bindings[key].Contains(removeMe)) {
				bindings[key].Remove(removeMe);
			}
		}
	}
	
	// Stores info of previous values to be able to treat sticks as buttons.
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

public class ControlBindings:Dictionary<string, Set<ControlBinding>> {
	
	public void Load(string name) {
		if(this.ContainsKey(name)) { // If the binding exists in the dictionary, it shall be reloaded from playerprefs
			this[name] = LoadFromPlayerprefs(name);
		} else {
			if(PlayerPrefs.HasKey("controls_"+name+"_binds")) { // If it doesn't, check if playerprefs has a definition for it
				this[name] = LoadFromPlayerprefs(name);
			} else {
				this.Add(name, LoadFromStringSet(InputWrapper.defaults[name]));
			}
		}
		
	}
	
	public Set<ControlBinding> LoadFromPlayerprefs(string name) {
		return LoadFromStringSet(new Set<string>(PlayerPrefs.GetString("controls_"+name+"_binds").Split(',')));
	}
	
	public Set<ControlBinding> LoadFromStringSet(Set<string> sts) {
		Set<ControlBinding> loaded = new Set<ControlBinding>();
		foreach(string keyString in sts) {
			if(keyString == "") { continue; }
			ControlBinding newBinding;
			if (keyString[keyString.Length-1]=='+') {
				newBinding = new ControlBinding(keyString.Substring(0, keyString.Length - 1), true);
				loaded.Add(newBinding);
			} else if (keyString[keyString.Length-1]=='-') {
				newBinding = new ControlBinding(keyString.Substring(0, keyString.Length - 1), false);
				loaded.Add(newBinding);
			} else {
				try {
					newBinding = new ControlBinding((KeyCode)Enum.Parse(typeof(KeyCode), keyString));
					loaded.Add(newBinding);
				} catch(ArgumentException) {
					Debug.LogWarning("ControlBinding: INVALID HARDWARE NAME: " + keyString);
				}
			}
		}
		return loaded;
	}
	
	public bool Delete(string name) {
		if(!this.ContainsKey(name)) {
			return false;
		}
		this[name] = LoadFromStringSet(InputWrapper.defaults[name]);
		Save(name);
		return true;
	}
	
	public void DeleteAll() {
		foreach(string key in this.Keys) {
			Delete(key);
		}
	}
	
	public void Save() {
		foreach (string name in this.Keys) {
			Save(name);
		}
	}
	
	public void Save(string name) {
		if(ContainsKey(name)) {
			PlayerPrefs.SetString("controls_"+name+"_binds", InputWrapper.GetHardwareName(name));
		}
	}

}

public class ControlBinding : IEquatable<ControlBinding> {
	public KeyCode key;
	public string axis;
	public bool positive = false;
	
	public ControlBinding() {
		this.key = KeyCode.None;
		this.axis = null;
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
	
	public ControlBinding(string HWName) {
		if (HWName[HWName.Length-1]=='+') {
			this.axis = HWName.Substring(0, HWName.Length - 1);
			this.positive = true;
			this.key = KeyCode.None;
		} else if (HWName[HWName.Length-1]=='-') {
			this.axis = HWName.Substring(0, HWName.Length - 1);
			this.positive = false;
			this.key = KeyCode.None;
		} else {
			try {
				this.key = (KeyCode)Enum.Parse(typeof(KeyCode), HWName);
				this.axis = null;
			} catch(ArgumentException) {
				Debug.LogWarning("ControlBinding: INVALID HARDWARE NAME: " + HWName);
			}
		}
	}
	
	public static bool operator ==(ControlBinding b1, ControlBinding b2) {
		if(System.Object.ReferenceEquals(b1, null) ^ System.Object.ReferenceEquals(b2, null)) { return false; }
		if(System.Object.ReferenceEquals(b1, null) && System.Object.ReferenceEquals(b2, null)) { return true; }
		return b1.Equals(b2);
	}
	
	public static bool operator !=(ControlBinding b1, ControlBinding b2) {
		if(System.Object.ReferenceEquals(b1, null) ^ System.Object.ReferenceEquals(b2, null)) { return true; }
		if(System.Object.ReferenceEquals(b1, null) && System.Object.ReferenceEquals(b2, null)) { return false; }
		return !b1.Equals(b2);
	}
	
	public override bool Equals(System.Object obj) {
		if(obj==null) { return false; }
		ControlBinding b = obj as ControlBinding;
		if((System.Object)b==null) { return false; }
		return Equals(b);
	}
	
	public bool Equals(ControlBinding b) {
		return (key == b.key) && (axis == b.axis) && (positive == b.positive);
	}
	
	public override int GetHashCode() {
		return ToString().GetHashCode();
	}
	
	public override string ToString() {
		if(axis == null) {
			return key.ToString();
		} else {
			return axis+(positive ? "+" : "-");
		}
	}
	
	public void Save(string name) {
		PlayerPrefs.SetInt("controls_"+name+"_key", (int)key);
		PlayerPrefs.SetString("controls_"+name+"_axis", axis);
		PlayerPrefs.SetInt("controls_"+name+"_pos", positive ? 1 : 0);
	}
}