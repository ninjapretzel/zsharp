using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

public class Console : MonoBehaviour {
	
	public string initialText = "";
	public GUISkin consoleSkin;
	public static string consoleText = "";

	private static Rect consoleWindowRect = new Rect(Screen.width * 0.125f, Screen.height * 0.125f, Screen.width * 0.75f, Screen.height * 0.75f);
	private static bool _consoleUp = false;
	public static bool consoleUp { get { return _consoleUp; } }
	private static bool consoleWasClosed = false;
	private static Vector2 consoleScrollPos;
	private static string consoleInput = "";
	private static float heightOfGUIContent = 0.0f;
	private static int cmdIndex = 0;
	private static List<string> previousCommands = new List<string>();
	private static Dictionary<string, string> aliases = new Dictionary<string, string>();

	public void Start() {
		consoleText = initialText;

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
			consoleWasClosed = true;
		}

	}
		
	private static void ConsoleWindow(int id) {
		GUI.color = Color.white;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC
		GUI.DragWindow(new Rect(4, 4, consoleWindowRect.width - 36, 16));
#endif
		if(GUI.Button(new Rect(consoleWindowRect.width - 32, 2, 32, 16), "X")) {
			_consoleUp = false;
			consoleInput = "";
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.wordWrap = true;
		GUI.skin.FontSizeFull(20.0f);
		float heightOfFont = GUI.skin.button.LineSize();
		Rect sizeOfLabel = new Rect(0.0f, 0.0f, consoleWindowRect.width - 26.0f, Mathf.Max(heightOfGUIContent, consoleWindowRect.height - heightOfFont - 30.0f));
		consoleScrollPos = GUI.BeginScrollView(new Rect(5.0f, 20.0f, consoleWindowRect.width - 10.0f, consoleWindowRect.height - heightOfFont - 30.0f), consoleScrollPos, sizeOfLabel, false, true); {
			GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.6667f);
			GUI.DrawTexture(sizeOfLabel, GUIF.pixel);
			GUI.color = Color.white;
			GUI.Label(sizeOfLabel, consoleText);
		} GUI.EndScrollView();
		if(((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) || (GUI.Button(new Rect(consoleWindowRect.width * 0.9f + 5.0f, consoleWindowRect.height - heightOfFont - 5.0f, consoleWindowRect.width * 0.1f - 10.0f, heightOfFont), "Send"))) && consoleInput.Length > 0) {
			Echo(">"+consoleInput);
			try {
				Execute(consoleInput);
			} catch(System.Exception e) {
				Debug.LogError("Internal error executing console command: "+e);
			}
			previousCommands.Add(consoleInput);
			cmdIndex = previousCommands.Count;
			consoleInput = "";
		}
		else if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow && cmdIndex > 0) {
			cmdIndex--;
			consoleInput = previousCommands[cmdIndex];
		} else if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow && cmdIndex < previousCommands.Count - 1) {
			cmdIndex++;
			consoleInput = previousCommands[cmdIndex];
		} else if(Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.Menu) && cmdIndex < previousCommands.Count - 1) {
			_consoleUp = false;
			consoleInput = "";
		} else {
			GUI.SetNextControlName("ConsoleInput");
			consoleInput = GUI.TextField(new Rect(5.0f, consoleWindowRect.height - heightOfFont - 5.0f, consoleWindowRect.width * 0.9f - 10.0f, heightOfFont), consoleInput);
		}
		if(consoleWasClosed) {
			heightOfGUIContent = GUI.skin.label.CalcHeight(new GUIContent(consoleText), consoleWindowRect.width - 26.0f);
			GUI.FocusControl("ConsoleInput");
			consoleWasClosed = false;
		}

	}

	public static void Execute(string line) {
		line = line.Trim();
		if(line.Length < 1) { return; }
		List<string> substrings = SplitUnlessInQuotes(line, ';');
		if(substrings.Count > 1) {
			foreach(string st in substrings) {
				Execute(st);
			}
		} else {
			line = substrings[0];
			int indexOfSpace = line.IndexOf(' ');
			string command = "";
			string parameters = "";
			if(indexOfSpace > 0) {
				command = line.Substring(0, indexOfSpace);
				parameters = line.Substring(indexOfSpace+1);
			} else {
				command = line;
				parameters = null;
			}
			if(aliases.ContainsKey(command)) {
				Execute(aliases[command] + " " + parameters);
			} else {
				string targetClass = null;
				string targetMethod = null;
				int indexOfDot = command.LastIndexOf('.');
				MethodInfo method = null;
				if(indexOfDot > 0) {
					targetClass = command.Substring(0, indexOfDot);
					targetMethod = command.Substring(indexOfDot+1);
					System.Type t = System.Type.GetType(targetClass);
					if(t != null) {
						if(parameters == null) {
							method = t.GetMethod(targetMethod, BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { }, null);
						} else {
							method = t.GetMethod(targetMethod, BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { typeof(string) }, null);
						}
					} else {
						Echo("Unknown class: "+targetClass);
					}
				} else {
					if(parameters == null) {
						method = typeof(Console).GetMethod(command, BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { }, null);
					} else {
						method = typeof(Console).GetMethod(command, BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { typeof(string) }, null);
					}
				}
				if(method == null) {
					if(targetMethod != null) {
						Echo("Unknown method: "+targetMethod);
					} else {
						Echo("Unknown command: "+command);
					}
				} else {
					if(parameters == null) {
						method.Invoke(null, null);
					} else {
						method.Invoke(null, new string[] { parameters });
					}
				}
			}
		}
	}

	public static List<string> SplitUnlessInQuotes(string st, char split) {
		List<string> result = new List<string>();
		st = st.Trim(split);
		if(st.IndexOf(split) < 0) {
			result.Add(st);
		} else {
			bool inQuotes = false;
			int lastSplitChar = -1;
			for(int i = 0; i < st.Length; i++) {
				if(st[i] == '\"') { inQuotes = !inQuotes; }
				if(!inQuotes && st[i] == split) {
					string substring = st.Substring(lastSplitChar + 1, i - lastSplitChar);
					if(substring.Length > 0) {
						result.Add(substring.Trim(split));
					}
					lastSplitChar = i;
				}
			}
			result.Add(st.Substring(lastSplitChar + 1));
		}
		return result;
	}

	public static void ToggleConsole() {
		_consoleUp = !_consoleUp;

	}

	public static void Echo() {
		Echo("");

	}

	public static void Echo(string st = "") {
		consoleText += "\n"+st.ParseNewlines();
		heightOfGUIContent = GUI.skin.label.CalcHeight(new GUIContent(consoleText), consoleWindowRect.width - 26.0f);
		consoleScrollPos = new Vector2(0, heightOfGUIContent);

	}

	public static void Clear() {
		consoleText = "";
		heightOfGUIContent = 0.0f;
		consoleScrollPos = Vector2.zero;

	}

	public static void Alias() {
		Echo("Usage: alias <name> \"[commands]\"");

	}

	public static void Alias(string st) {
		List<string> parameters = SplitUnlessInQuotes(st, ' ');
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
				Alias(parameters[0], parameters[1]);
				break;
		}

	}

	public static void Alias(string name, string cmds) {
		if(aliases.ContainsKey(name)) {
			aliases[name] = cmds.Trim('\"');
		} else {
			aliases.Add(name, cmds.Trim('\"'));
		}

	}

	public static void Print(string st) {
		Debug.Log(st.ParseNewlines());

	}

	public static void Print() {
		Debug.Log("");

	}
}