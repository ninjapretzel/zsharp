using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Console : MonoBehaviour {
	
	public string initialText = "";
	public GUISkin consoleSkin;
	public static string consoleText = "";
	public static Color color = Color.white;

	private static Rect consoleWindowRect = new Rect(Screen.width * 0.125f, Screen.height * 0.125f, Screen.width * 0.75f, Screen.height * 0.75f);
	private static bool _consoleUp = false;
	public static bool consoleUp { get { return _consoleUp; } }
	private static bool focusTheTextField = false;
	private static Vector2 consoleScrollPos;
	private static string consoleInput = "";
	private static float heightOfGUIContent = 0.0f;
	private static int cmdIndex = 0;
	private static List<string> previousCommands = new List<string>();
	private static Dictionary<string, string> aliases = new Dictionary<string, string>();
	private static Dictionary<KeyCode, string> binds = new Dictionary<KeyCode, string>();
	private static string configPath { get { return Application.persistentDataPath + "config.cfg"; } }
	//private static Message message = new Message();

	public void Start() {
		consoleText = initialText;

	}

	public void Update() {
		if(!_consoleUp) {
			foreach(KeyCode bind in binds.Keys) {
				if(Input.GetKeyDown(bind)) {
					Execute(binds[bind]);
				}
			}
		}
	}

	public void OnGUI() {
		GUI.skin = consoleSkin;
		GUI.skin.window.fontSize = 18;
		if(_consoleUp) {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC
			consoleWindowRect = GUI.Window(1, consoleWindowRect, ConsoleWindow, "Developer Console");
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
			consoleWindowRect = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.6667f);
			GUI.DrawTexture(consoleWindowRect, GUIF.pixel);
			ConsoleWindow(-1);
#endif
		} else {
			focusTheTextField = true;
		}

	}
		
	private static void ConsoleWindow(int id) {
		GUI.color = Color.white;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC
		GUI.DragWindow(new Rect(4, 4, consoleWindowRect.width - 36, 16));
#endif
		float heightOfFont = GUI.skin.button.LineSize();
		if(GUI.Button(new Rect(consoleWindowRect.width - 34, 2, 32, 16), "X")) {
			_consoleUp = false;
			consoleInput = "";
		}

		if(((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) || (GUI.Button(new Rect(consoleWindowRect.width * 0.9f + 5.0f, consoleWindowRect.height - heightOfFont - 5.0f, consoleWindowRect.width * 0.1f - 10.0f, heightOfFont), "Send"))) && consoleInput.Length > 0) {
			Echo("> "+consoleInput);
			try {
				Execute(consoleInput);
			} catch(System.Exception e) {
				Debug.LogError("Internal error executing console command:\n"+e);
			}
			previousCommands.Add(consoleInput);
			cmdIndex = previousCommands.Count;
			consoleInput = "";
			focusTheTextField = true;
		} else if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow && cmdIndex > 0) {
			cmdIndex--;
			consoleInput = previousCommands[cmdIndex];
		} else if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow && cmdIndex < previousCommands.Count - 1) {
			cmdIndex++;
			consoleInput = previousCommands[cmdIndex];
		} else if(Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || (Event.current.keyCode == KeyCode.Menu && Application.platform == RuntimePlatform.Android))) {
			_consoleUp = false;
			consoleInput = "";
		}
		GUI.SetNextControlName("ConsoleInput");
		consoleInput = GUI.TextField(new Rect(5.0f, consoleWindowRect.height - heightOfFont - 5.0f, consoleWindowRect.width * 0.9f - 10.0f, heightOfFont), consoleInput);
		if(focusTheTextField) {
			GUI.FocusControl("ConsoleInput");
			focusTheTextField = false;
		}

		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.wordWrap = true;
		GUI.skin.FontSizeFull(20.0f);
		heightOfGUIContent = GUI.skin.label.CalcHeight(new GUIContent(consoleText), consoleWindowRect.width - 26.0f);
		Rect sizeOfLabel = new Rect(0.0f, 0.0f, consoleWindowRect.width - 26.0f, Mathf.Max(heightOfGUIContent, consoleWindowRect.height - heightOfFont - 30.0f));
		consoleScrollPos = GUI.BeginScrollView(new Rect(5.0f, 20.0f, consoleWindowRect.width - 10.0f, consoleWindowRect.height - heightOfFont - 30.0f), consoleScrollPos, sizeOfLabel, false, true); {
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.6667f);
			GUI.DrawTexture(sizeOfLabel, GUIF.pixel);
			GUI.color = color;
			GUI.Label(sizeOfLabel, consoleText);
			//message.str = consoleText;
			//message.Draw(sizeOfLabel);
		} GUI.EndScrollView();

	}

	// Execute takes a line and attempts to turn it into commands which will be executed, through reflection.
	// Order: Fields (Variables), Methods (Functions), Properties, Aliases
	public static void Execute(string line) {
		line = line.Trim();
		if(line.Length < 1) { return; }
		List<string> substrings = line.SplitUnlessInContainer(';', '\"');
		if(substrings.Count > 1) {
			foreach(string st in substrings) {
				Execute(st);
			}
		} else {
			int indexOfSpace = line.IndexOf(' ');
			string command = "";
			string parameters = "";
			if(indexOfSpace > 0) {
				command = line.Substring(0, indexOfSpace);
				parameters = line.Substring(indexOfSpace+1).Trim();
			} else {
				command = line;
				parameters = null;
			}
			string targetClassName = null;
			string targetMemberName = null;
			int indexOfDot = command.LastIndexOf('.');
			System.Type targetClass;
			if(indexOfDot > 0) {
				targetClassName = command.Substring(0, indexOfDot);
				targetMemberName = command.Substring(indexOfDot+1);
				targetClass = System.Type.GetType(targetClassName);
			} else {
				targetClass = typeof(Console);
				targetMemberName = command;
			}
			if(targetClass != null) {
				if(!CallField(targetClass, targetMemberName, parameters)) {
					if(!CallMethod(targetClass, targetMemberName, parameters)) {
						if(!CallProperty(targetClass, targetMemberName, parameters)) {
							if(!aliases.ContainsKey(command)) {
								Echo("Unknown command: "+command);
							} else {
								Execute(aliases[command] + " " + parameters);
							}
						}
					}
				}
			} else {
				Echo("Unknown command: "+command);
			}
		}

	}

	public static bool CallField(System.Type targetClass, string varName, string parameters) {
		if(parameters == null || parameters.Length < 1) { return CallField(targetClass, varName); }
		FieldInfo targetVar = targetClass.GetField(varName, BindingFlags.Public | BindingFlags.Static);
		if(targetVar != null) {
			return SetFieldValue(null, targetVar, parameters.SplitUnlessInContainer(' ', '\"'));
		} else {
			object main = GetMainOfClass(targetClass);
			if(main != null) {
				return SetFieldValue(main, targetClass.GetField(varName), parameters.SplitUnlessInContainer(' ', '\"'));
			}
		}
		return false;
	}

	public static bool CallField(System.Type targetClass, string varName) {
		string output = GetFieldValue(null, targetClass.GetField(varName, BindingFlags.Public | BindingFlags.Static));
		if(output != null) {
			Echo(varName + " is " + output);
			return true;
		} else {
			object main = GetMainOfClass(targetClass);
			if(main != null) {
				output = GetFieldValue(main, targetClass.GetField(varName));
				if(output != null) {
					Echo(varName + " is " + output);
					return true;
				}
			}
		}
		return false;
	}

	public static string GetFieldValue(object instance, FieldInfo fieldInfo) {
		if(fieldInfo == null) { return null; }
		switch(fieldInfo.FieldType.Name) {
			case "Vector2":
			case "Vector3":
			case "Color":
			case "String":
			case "Char":
			case "Byte":
			case "SByte":
			case "Int16":
			case "Int32":
			case "Int64":
			case "UInt16":
			case "UInt32":
			case "UInt64":
			case "Single":
			case "Double":
			case "Boolean":
				return fieldInfo.GetValue(instance).ToString();
			default:
				return null;
		}
	}

	public static bool SetFieldValue(object instance, FieldInfo fieldInfo, List<string> parameters) {
		if(fieldInfo == null) { return false; }
		object result = ParseParameterListIntoType(fieldInfo.FieldType.Name, parameters);
		if(result != null) {
			fieldInfo.SetValue(instance, result);
			return true;
		} else {
			return CallField(fieldInfo.DeclaringType, fieldInfo.Name);
		}
	}

	public static bool CallProperty(System.Type targetClass, string propName, string parameters) {
		if(parameters == null || parameters.Length < 1) { return CallProperty(targetClass, propName); }
		PropertyInfo targetProp = targetClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Static);
		if(targetProp != null) {
			if(targetProp.GetSetMethod() != null) {
				return SetPropertyValue(null, targetProp, parameters.SplitUnlessInContainer(' ', '\"'));
			} else {
				Echo(propName+" is read-only!");
				return CallProperty(targetClass, propName);
			}
		} else {
			object main = GetMainOfClass(targetClass);
			if(main != null) {
				targetProp = targetClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
				if(targetProp != null) {
					if(targetProp.GetSetMethod() != null) {
						return SetPropertyValue(main, targetProp, parameters.SplitUnlessInContainer(' ', '\"'));
					} else {
						Echo(propName+" is read-only!");
						return CallProperty(targetClass, propName);
					}
				}
			}
		}
		return false;
	}

	public static bool CallProperty(System.Type targetClass, string propName) {
		string output = GetPropertyValue(null, targetClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Static));
		if(output != null) {
			Echo(propName + " is " + output);
			return true;
		} else {
			object main = GetMainOfClass(targetClass);
			if(main != null) {
				output = GetPropertyValue(main, targetClass.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance));
				if(output != null) {
					Echo(propName + " is " + output);
					return true;
				}
			}
		}
		return false;
	}

	public static string GetPropertyValue(object instance, PropertyInfo propertyInfo) {
		if(propertyInfo == null) { return null; }
		if(propertyInfo.GetGetMethod() == null) { return "write-only!"; }
		switch(propertyInfo.PropertyType.Name) {
			case "Vector2":
			case "Vector3":
			case "Color":
			case "String":
			case "Char":
			case "Byte":
			case "SByte":
			case "Int16":
			case "Int32":
			case "Int64":
			case "UInt16":
			case "UInt32":
			case "UInt64":
			case "Single":
			case "Double":
			case "Boolean":
				return propertyInfo.GetValue(instance, null).ToString();
			default:
				return null;
		}
	}

	public static bool SetPropertyValue(object instance, PropertyInfo propertyInfo, List<string> parameters) {
		object result = ParseParameterListIntoType(propertyInfo.PropertyType.Name, parameters);
		if(result != null) {
			propertyInfo.SetValue(instance, result, null);
			return true;
		} else {
			return CallProperty(propertyInfo.DeclaringType, propertyInfo.Name);
		}
	}

	public static bool CallMethod(System.Type targetClass, string methodName, string parameters) {
		MethodInfo[] targetMethods = targetClass.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);
		MethodInfo[] targetInstancedMethods = new MethodInfo[0];
		object main = GetMainOfClass(targetClass);
		if(main != null) {
			targetInstancedMethods = targetClass.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
		}
		if(parameters != null && parameters.Length != 0) {
			// Try to find a static method matching name and parameters
			if(CallMethodMatchingParameters(null, methodName, targetMethods, parameters.SplitUnlessInContainer(' ', '\"'))) {
				return true;
			}
			// Try to find an instanced method matching name and parameters if a main object to invoke on exists
			if(main != null) {
				if(CallMethodMatchingParameters(main, methodName, targetInstancedMethods, parameters.SplitUnlessInContainer(' ', '\"'))) {
					return true;
				}
			}
			// Try to find a static method matching name with one string parameter
			MethodInfo targetMethod = targetClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, new System.Type[] { typeof(string) }, null);
			if(targetMethod != null) {
				InvokeAndEchoResult(targetMethod, null, new string[] { ParseParameterListIntoType("String", parameters.SplitUnlessInContainer(' ', '\"')).ToString() });
				return true;
			}
			// Try to find a method matching name with one string parameter if a main object to invoke on exists
			if(main != null) {
				MethodInfo targetInstancedMethod = targetClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, new System.Type[] { typeof(string) }, null);
				if(targetMethod != null) {
					InvokeAndEchoResult(targetInstancedMethod, main, new string[] { ParseParameterListIntoType("String", parameters.SplitUnlessInContainer(' ', '\"')).ToString() });
					return true;
				}
			}
		}
		// Try to find a static parameterless method matching name
		MethodInfo targetParameterlessMethod = targetClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, new System.Type[] { }, null);
		if(targetParameterlessMethod != null) {
			InvokeAndEchoResult(targetParameterlessMethod, null, new object[] { });
			return true;
		}
		// Try to find a parameterless method matching name if a main object to invoke on exists
		if(main != null) {
			MethodInfo targetInstancedParameterlessMethod = targetClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, new System.Type[] { }, null);
			if(targetInstancedParameterlessMethod != null) {
				InvokeAndEchoResult(targetInstancedParameterlessMethod, main, new object[] { });
				return true;
			}
		}
		// At this point no method will be invoked. Print an error message based on what has happened.
		if(targetMethods.Length > 0 || targetInstancedMethods.Length > 0) {
			bool methodWithRightNameFound = false;
			foreach(MethodInfo methodInfo in targetMethods) {
				if(methodInfo.Name == methodName) { methodWithRightNameFound = true; break; }
			}
			if(!methodWithRightNameFound) {
				foreach(MethodInfo methodInfo in targetInstancedMethods) {
					if(methodInfo.Name == methodName) { methodWithRightNameFound = true; break; }
				}
			}
			if(methodWithRightNameFound) {
				if(parameters != null && parameters.Length != 0) {
					Echo("No method "+methodName+" matching the parameters provided could be found.");
				} else {
					Echo("No method "+methodName+" taking no parameters could be found. Provide some parameters!");
				}
				// In either case, the error message is handled here, so return true;
				return true;
			}
		}
		// No method matched this command, therefore indicate a failure
		return false;
	}

	public static bool CallMethodMatchingParameters(object targetObject, string methodName, MethodInfo[] targetMethods, List<string> parameterList) {
		foreach(MethodInfo targetMethod in targetMethods) {
			if(targetMethod.Name != methodName) { continue; }
			ParameterInfo[] parameterInfos = targetMethod.GetParameters();
			if(parameterInfos.Length != parameterList.Count) { continue; }
			object[] parsedParameters = new object[parameterInfos.Length];
			bool failed = false;
			for(int i = 0; i < parsedParameters.Length; i++) {
				// Need to split the given parameters AGAIN here if not in container, since ParseParameterListIntoType expects its parameters separately.
				// For example, if a method takes an int and a Color as an attribute, the user could type
				// Class.MethodName "7" "1 0.4 0.2 1"
				// which would get split into "7" and "1 0.4 0.2 1", and this method would try to find a method matching two parameters.
				// If such a method is found, it would further split "1 0.4 0.2 1" into four separate strings and pass them to ParseParameterListIntoType
				parsedParameters[i] = ParseParameterListIntoType(parameterInfos[i].ParameterType.Name, parameterList[i].SplitUnlessInContainer(' ', '\"'));
				if(parsedParameters[i] == null) { failed = true; break; }
			}
			if(failed) { continue; }
			InvokeAndEchoResult(targetMethod, targetObject, parsedParameters);
			return true;
		}
		return false;
	}

	public static void InvokeAndEchoResult(MethodInfo targetMethod, object targetObject, object[] parameters) {
		if(targetMethod.ReturnType == typeof(void)) {
			targetMethod.Invoke(targetObject, parameters);
		} else {
			Echo(targetMethod.Invoke(targetObject, parameters).ToString());
		}

	}

	public static object ParseParameterListIntoType(string typeName, List<string> parameters) {
		switch(typeName) {
			case "Vector2":
				Vector2 targetV2;
				PropertyInfo vector2ByName = typeof(Vector2).GetProperty(parameters[0], BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);
				if(vector2ByName != null) {
					if(parameters.Count != 1) { return null; }
					targetV2 = (Vector2)vector2ByName.GetValue(null, null);
				} else {
					if(parameters.Count != 2) { return null; }
					float x = 0.0f;
					try {
						x = System.Single.Parse(parameters[0]);
					} catch(System.FormatException) { return null; }
					float y = 0.0f;
					try {
						y = System.Single.Parse(parameters[1]);
					} catch(System.FormatException) { return null; }
					targetV2 = new Vector3(x, y);
				}
				return targetV2;
			case "Vector3":
				Vector3 targetV3;
				PropertyInfo vector3ByName = typeof(Vector3).GetProperty(parameters[0], BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);
				if(vector3ByName != null) {
					if(parameters.Count != 1) { return null; }
					targetV3 = (Vector3)vector3ByName.GetValue(null, null);
				} else {
					if(parameters.Count != 3) { return null; }
					float x = 0.0f;
					try {
						x = System.Single.Parse(parameters[0]);
					} catch(System.FormatException) { return null; }
					float y = 0.0f;
					try {
						y = System.Single.Parse(parameters[1]);
					} catch(System.FormatException) { return null; }
					float z = 0.0f;
					try {
						z = System.Single.Parse(parameters[2]);
					} catch(System.FormatException) { return null; }
					targetV3 = new Vector3(x, y, z);
				}
				return targetV3;
			case "Color":
				Color targetColor;
				PropertyInfo colorByName = typeof(Color).GetProperty(parameters[0], BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);
				if(colorByName != null) {
					if(parameters.Count != 1) { return null; }
					targetColor = (Color)colorByName.GetValue(null, null);
				} else {
					if(parameters.Count != 4) { return null; }
					float r = 0.0f;
					try {
						r = System.Single.Parse(parameters[0]);
					} catch(System.FormatException) { return null; }
					float g = 0.0f;
					try {
						g = System.Single.Parse(parameters[1]);
					} catch(System.FormatException) { return null; }
					float b = 0.0f;
					try {
						b = System.Single.Parse(parameters[2]);
					} catch(System.FormatException) { return null; }
					float a = 1.0f;
					try {
						a = System.Single.Parse(parameters[3]);
					} catch(System.FormatException) { return null; }
					targetColor = new Color(r, g, b, a);
				}
				return targetColor;
			case "Rect":
				if(parameters.Count != 4) { return null; }
				Rect targetRect;
				float l = 0.0f;
				try {
					l = System.Single.Parse(parameters[0]);
				} catch(System.FormatException) { return null; }
				float t = 0.0f;
				try {
					t = System.Single.Parse(parameters[1]);
				} catch(System.FormatException) { return null; }
				float w = 0.0f;
				try {
					w = System.Single.Parse(parameters[2]);
				} catch(System.FormatException) { return null; }
				float h = 1.0f;
				try {
					h = System.Single.Parse(parameters[3]);
				} catch(System.FormatException) { return null; }
				targetRect = new Rect(l, t, w, h);
				return targetRect;
			case "String":
				System.Text.StringBuilder bob = new System.Text.StringBuilder();
				foreach(string st in parameters) {
					bob.Append(st + " ");
				}
				string allparams = bob.ToString();
				return allparams.Substring(0, allparams.Length - 1);
			case "Char":
			case "SByte":
			case "Int16":
			case "Int32":
			case "Int64":
			case "Byte":
			case "UInt16":
			case "UInt32":
			case "UInt64":
			case "Single":
			case "Double":
				if(parameters.Count != 1) { return null; }
				try {
					return System.Type.GetType("System."+typeName).GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new System.Type[] { typeof(string) }, null).Invoke(null, new string[] { parameters[0] });
				} catch(System.Reflection.TargetInvocationException) { // Is thrown in place of the Parse method's exceptions
					return null;
				}
			case "Boolean":
				if(parameters.Count != 1) { return null; }
				if(parameters[0] == "1" || parameters[0].Equals("on", System.StringComparison.InvariantCultureIgnoreCase)) {
					return true;
				} else if(parameters[0] == "0" || parameters[0].Equals("off", System.StringComparison.InvariantCultureIgnoreCase)) {
					return false;
				} else {
					try {
						return System.Boolean.Parse(parameters[0]);
					} catch(System.FormatException) {
						return null;
					}
				}
			default:
				return null;
		}

	}

	public static object GetMainOfClass(System.Type targetClass) {
		FieldInfo mainField = targetClass.GetField("main", BindingFlags.Public | BindingFlags.Static);
		if(mainField != null && mainField.FieldType == targetClass) {
			return mainField.GetValue(null);
		}
		return null;
	}

	public static void ToggleConsole() {
		_consoleUp = !_consoleUp;
		consoleInput = "";

	}

	public static void Echo() {
		Echo("");

	}

	public static void Echo(string st) {
		consoleText += "\n"+st.ParseNewlines();
		consoleScrollPos = new Vector2(0, heightOfGUIContent);

	}

	public static void Clear() {
		consoleText = "";
		heightOfGUIContent = 0.0f;
		consoleScrollPos = Vector2.zero;

	}

	public static void Alias(string st) {
		List<string> parameters = st.SplitUnlessInContainer(' ', '\"');
		switch(parameters.Count) {
			case 0:
				Alias();
				break;
			case 1:
				if(aliases.ContainsKey(parameters[0])) {
					Echo(parameters[0]+" is "+aliases[parameters[0]]);
				} else {
					Echo(parameters[0]+" does not exist!");
				}
				break;
			default:
				bool containsQuote = (st.IndexOf('\"') >= 0);
				if(!containsQuote) {
					System.Text.StringBuilder rest = new System.Text.StringBuilder();
					for(int i=1; i<parameters.Count; i++) {
						rest.Append(' '+parameters[i]);
					}
					Alias(parameters[0], rest.ToString().Substring(1));
				} else {
					Alias(parameters[0], parameters[1]);
				}
				break;
		}

	}

	public static void Alias() {
		Echo("Alias: Allows multiple commands to be executed using one command.\nUsage: Alias <name> \"command1 \'[param1\' \'[params...]\'[;][commands...]\"");

	}

	public static void Alias(string name, string cmds) {
		if(!aliases.ContainsKey(name)) {
			aliases.Add(name, "");
		}
		aliases[name] = "";
		List<string> cmdList = cmds.SplitUnlessInContainer(';', '\"');
		foreach(string cmd in cmdList) {
			aliases[name] += ';' + cmd.ReplaceFirstAndLast('\'', '\"');
		}
		aliases[name] = aliases[name].Substring(1);

	}

	public static void Unalias() {
		Echo("Unalias: Deletes an alias.\nUsage: Unalias <name>");

	}

	public static void Unalias(string st) {
		string unaliasMe = st.SplitUnlessInContainer(' ', '\"')[0];
		if(aliases.ContainsKey(unaliasMe)) {
			aliases.Remove(unaliasMe);
		}

	}

	public static void Bind(string st) {
		List<string> parameters = st.SplitUnlessInContainer(' ', '\"');
		switch(parameters.Count) {
			case 0:
				Bind();
				break;
			case 1:
				KeyCode targetKeyCode;
				try {
					 targetKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), parameters[0]);
				} catch(System.ArgumentException) {
					Echo(parameters[0] + " is not a valid KeyCode");
					break;
				}
				if(binds.ContainsKey(targetKeyCode)) {
					Echo(parameters[0]+" is "+binds[targetKeyCode]);
				} else {
					Echo(parameters[0]+" is unbound");
				}
				break;
			default:
				bool containsQuote = (st.IndexOf('\"') >= 0);
				if(!containsQuote) {
					System.Text.StringBuilder rest = new System.Text.StringBuilder();
					for(int i=1; i<parameters.Count; i++) {
						rest.Append(' '+parameters[i]);
					}
					Bind(parameters[0], rest.ToString().Substring(1));
				} else {
					Bind(parameters[0], parameters[1]);
				}
				break;
		}

	}

	public static void Bind() {
		Echo("Bind: Allows binding of commands to keypresses.\nUsage: Bind <KeyCode> \"command1 \'[param1\' \'[params...]\'[;][commands...]\"");

	}

	public static void Bind(string name, string cmds) {
		KeyCode targetKeyCode;
		try {
				targetKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), name);
		} catch(System.ArgumentException) {
			Echo(name + " is not a valid KeyCode!");
			return;
		}
		Bind(targetKeyCode, cmds);

	}

	public static void Bind(KeyCode name, string cmds) {
		if(!binds.ContainsKey(name)) {
			binds.Add(name, "");
		}
		binds[name] = "";
		List<string> cmdList = cmds.SplitUnlessInContainer(';', '\"');
		foreach(string cmd in cmdList) {
			binds[name] += ';' + cmd.ReplaceFirstAndLast('\'', '\"');
		}
		binds[name] = binds[name].Substring(1);

	}

	public static void Unbind() {
		Echo("Unbind: Unbinds all commands from a key.\nUsage: Unbind <KeyCode>");

	}

	public static void Unbind(string st) {
		KeyCode unbindMe;
		string name = st.SplitUnlessInContainer(' ', '\"')[0];
		try {
			unbindMe = (KeyCode)System.Enum.Parse(typeof(KeyCode), name);
		} catch(System.ArgumentException) {
			Echo(name + " is not a valid KeyCode!");
			return;
		}
		if(binds.ContainsKey(unbindMe)) {
			binds.Remove(unbindMe);
		}

	}

	public static void Print(string st) {
		Debug.Log(st.ParseNewlines());

	}

	public static void Print() {
		Debug.Log("");

	}

	public static void Quit() {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif

	}
}