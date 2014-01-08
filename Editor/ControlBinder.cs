#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;


public class ControlBinder : EditorWindow {
	
	public static string path { get { return Application.dataPath + "/Data/Resources/"; } } 
	public static string target { get { return "Controls.csv"; } }
	public static string ouyaTarget { get { return "OuyaControls.csv"; } }
	
	private static Vector2 scrollPos = Vector2.zero;
	private static string clipboard = null;
	
	private int currentTab = 0;
	
	private static List<string> controls = new List<string>();
	private static List<string> keys = new List<string>();
	
	[MenuItem ("Window/Control Bindings")]
	public static void ShowWindow() {
		ControlBinder main = (ControlBinder)EditorWindow.GetWindow(typeof(ControlBinder));
		main.LoadDatabase(target);
		main.minSize = new Vector2(325,0);
	}
	
	public void OnGUI() {
		GUILayout.BeginVertical(); {
			GUILayout.BeginHorizontal(); {
				Color backup = GUI.color;
				if(currentTab == 0) {
					GUI.color = backup;
				} else {
					GUI.color = Color.gray;
				}
				if(GUILayout.Button("Normal", GUILayout.Width(100))) {
					SaveDatabase(ouyaTarget);
					currentTab = 0;
					LoadDatabase(target);
				}
				if(currentTab == 1) {
					GUI.color = backup;
				} else {
					GUI.color = Color.gray;
				}
				if(GUILayout.Button("OUYA", GUILayout.Width(100))) {
					SaveDatabase(target);
					currentTab = 1;
					LoadDatabase(ouyaTarget);
				}
				GUI.color = backup;
			} GUILayout.EndHorizontal();
			GUILayout.BeginVertical("box"); {
				BindTableGUI();
			} GUILayout.EndVertical();
			GUILayout.BeginHorizontal(); {
				if(GUILayout.Button("Apply & Save", GUILayout.Width(100))) {
					switch(currentTab) {
						case 0:
							SaveDatabase(target);
							break;
						case 1:
							SaveDatabase(ouyaTarget);
							break;
					}
				}
				if(GUILayout.Button("Load defaults", GUILayout.Width(100))) {
					LoadDefault();
				}
				if(GUILayout.Button("Copy sheet", GUILayout.Width(100))) {
					CopyAll();
				}
				if(GUILayout.Button("Paste sheet", GUILayout.Width(100))) {
					PasteAll();
				}
			} GUILayout.EndHorizontal();
		} GUILayout.EndVertical();
	}
	
	public void BindTableGUI() {
		GUILayout.BeginHorizontal(); {
			scrollPos = GUILayout.BeginScrollView(scrollPos); {
				GUILayout.BeginHorizontal("box"); {
					GUILayout.Box("#", GUILayout.Width(45));
					GUILayout.Box("Control", GUILayout.Width(200));
					GUILayout.Box("Keys/Axes", GUILayout.Width(position.width - 282));
				} GUILayout.EndHorizontal();
				for(int i=0;i<controls.Count;i++) {
					BindTableItem(i);
				}
				if(GUILayout.Button("+", GUILayout.Width(20))) {
					controls.Add("New");
					keys.Add("None");
				}
			} GUILayout.EndScrollView();
		} GUILayout.EndHorizontal();
	}
	
	public void BindTableItem(int index) {
		GUILayout.BeginHorizontal("box"); {
			GUILayout.Label(index.ToString(), GUILayout.Width(20));
			if(GUILayout.Button("-", GUILayout.Width(20))) {
				controls.RemoveAt(index);
				keys.RemoveAt(index);
			} else {
				controls[index] = GUILayout.TextField(controls[index], GUILayout.Width(200));
				if(keys[index].Length > 0) {
					string[] key = keys[index].Split(',');
					if(key.Length > 0) {
						if(GUILayout.Button("-", GUILayout.Width(20))) {
							string[] newList = new string[key.Length-1];
							for(int j=0;j<newList.Length;j++) {
								newList[j] = key[j+1];
							}
							key = newList;
						}
					}
					if(key.Length > 0) {
						keys[index] = GUILayout.TextField(key[0], GUILayout.Width(120));
					} else {
						keys[index] = "";
					}
					for(int i=1;i<key.Length;i++) {
						if(GUILayout.Button("-", GUILayout.Width(20))) {
							string[] newList = new string[key.Length-1];
							for(int j=0;j<newList.Length;j++) {
								if(j<i) {
									newList[j] = key[j];
								} else {
									newList[j] = key[j+1];
								}
							}
							key = newList;
						}
						if(key.Length > i) {
							keys[index] += ","+GUILayout.TextField(key[i], GUILayout.Width(120));
						}
					}
				}
				if(GUILayout.Button("+", GUILayout.Width(20))) {
					if(keys[index].Length > 0) {
						keys[index] += ",None";
					} else {
						keys[index] = "None";
					}
				}
			}
		} GUILayout.EndHorizontal();
	}
	
	void LoadDefault() {
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
		string[] bindings;
		bindings = (Resources.Load("DefaultControls", typeof(TextAsset)) as TextAsset).text.ConvertNewlines().Split('\n');
		controls = new List<string>(bindings.Length);
		keys = new List<string>(bindings.Length);
		for(int i=0;i<bindings.Length;i++) {
			if(bindings[i][0] != '#') {
				int ind = bindings[i].IndexOf(',');
				controls.Add(bindings[i].Substring(0, ind));
				keys.Add(bindings[i].Substring(ind+1));
			}
		}
	}
	
	void LoadDatabase(string file) {
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
		string[] bindings;
		if (File.Exists(path+file)) {
			bindings = File.ReadAllLines(path+file);
		} else {
			bindings = (Resources.Load("DefaultControls", typeof(TextAsset)) as TextAsset).text.ConvertNewlines().Split('\n');
		}
		controls = new List<string>(bindings.Length);
		keys = new List<string>(bindings.Length);
		for(int i=0;i<bindings.Length;i++) {
			if(bindings[i][0] != '#') {
				int ind = bindings[i].IndexOf(',');
				controls.Add(bindings[i].Substring(0, ind));
				keys.Add(bindings[i].Substring(ind+1));
			}
		}
	}
	
	void SaveDatabase(string file) {
		if (File.Exists(path+file)) {
			File.Delete(path+file);
		}
		string data = "#ControlName,HardwareName\n";
		for(int i=0;i<controls.Count;i++) {
			data += controls[i];
			data += ",";
			data += keys[i];
			if(i < controls.Count-1) {
				data += (char)0x0A;
			}
		}
		StreamWriter sw = File.CreateText(path+file);
		sw.Write(data);
		sw.Close();
	}
	
	public void CopyAll() {
		clipboard = "";
		for(int i=0;i<controls.Count;i++) {
			clipboard += controls[i];
			clipboard += ",";
			clipboard += keys[i];
			if(i < controls.Count-1) {
				clipboard += (char)0x0A;
			}
		}
	}
	
	public void PasteAll() {
		string[] bindings;
		bindings = clipboard.Split((char)0x0A);
		controls = new List<string>(bindings.Length);
		keys = new List<string>(bindings.Length);
		for(int i=0;i<bindings.Length;i++) {
			int ind = bindings[i].IndexOf(',');
			controls.Add(bindings[i].Substring(0, ind));
			keys.Add(bindings[i].Substring(ind+1));
		}
	}
}




#endif