﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;



public class ItemDatabase : ZEditorWindow {
	
	public static string path { get { return Application.dataPath + "/Data/Resources/"; } } 
	public static string target { get { return path + "Items.csv"; } }
	
	List<Item> items;
	
	List<OptionEntry> stats;
	int numOptions;
	int removeAt;
	
	Vector4 selectScroll;
	Vector2 editScroll;
	
	int selection;
	Item editing;
	
	bool listChanged = false;
	public override float fieldWidth { get { return .6f * position.width; } }
	
	[MenuItem ("Window/Item Database")]
	static void ShowWindow() {
		ItemDatabase main = (ItemDatabase)EditorWindow.GetWindow(typeof(ItemDatabase));
		
		main.Init();
		//main.items.Add(new Item());
		main.LoadDatabase();
		
		if ( main.items.Count == 0) { main.items.Add(new Item()); }
		main.LoadSelection();
		
	}
	
	void Init() {
		items = new List<Item>();
		stats = new List<OptionEntry>();
		numOptions = 0;
		removeAt = -1;
		editing = new Item();
		listChanged = false;
	}
	
	
	void OnGUI() {
		//GUISkin blankSkin = Resources.Load("blank", typeof(GUISkin)) as GUISkin;
		//Color lastColor = GUI.color;
		//GUISkin lastSkin = GUI.skin;
		checkChanges = false;
		string[] ss = new string[items.Count];
		for (int i = 0; i < ss.Length; i++) { ss[i] = items[i].name; }
		
		Rect selectArea = new Rect(5, 5, 200, position.height-100);
		Rect single = new Rect(0, 0, 180, 20);
		int lastSelection = selection;
		selectScroll = GUIF.EditorSelectionArea(selectArea, selectScroll, single, ss, true);
		selection = (int)selectScroll.z;
		int selectAction = (int)selectScroll.w;
		
		if (selectAction == 1) { items.Insert(selection, items[selection].Clone()); }
		if (selectAction == -1) { items.RemoveAt(selection); selection = (int)selection.Clamp(0, items.Count); }
		
		if (selection != lastSelection) {
			LoadSelection();
		}
		
		
		Rect infoArea = new Rect(0, selectArea.yMax, selectArea.width + 10, position.height - selectArea.yMax);
		GUI.Box(infoArea, "");
		
		Rect line = infoArea.Top(.25f);
		GUI.Label(line, "Number of items: " + items.Count);
		
		line = line.MoveDown();
		if (GUI.Button(line, "Extra button")) {
			
		}
		
		line = line.MoveDown();
		GUI.color = listChanged ? Color.red : Color.white;
		if (GUI.Button(line, "Apply To Database")) {
			WriteDatabase();
		}
		
		line = line.MoveDown();
		GUI.color = Color.white;
		if (GUI.Button(line.Left(.5f), "Add Item")) {
			if (items.Count > 0) { items.Add(items.LastElement().Clone()); }
			else { items.Add(new Item()); }
		}
		
		if (GUI.Button(line.Right(.5f), "Remove Item")) {
			if (items.Count >= 1) { items.RemoveAt(items.Count-1); }
		}
		
		
		//Rect editArea = new Rect(210, 0, position.width - 210, position.height);
		//Rect editView = new Rect(0, 0, editArea.width-10, position.height * 2);
		checkChanges = true;
		GUILayout.BeginHorizontal();
			GUILayout.Space(210);
			
			GUILayout.BeginVertical("box");
			
				editScroll = GUILayout.BeginScrollView(editScroll);
				
					GUILayout.BeginVertical("box");
					
						BasicSettingsBox();
							
						PropertiesBox();
						
						
						
						StatsBox();
						
					GUILayout.EndVertical();
					
				GUILayout.EndScrollView();
				
				GUILayout.Space(10);
				GUI.color = changed ? Color.red : Color.white;
				if (GUILayout.Button("Apply Item")) {
					listChanged = changed || listChanged;
					ApplySelection();
				}
				
				GUI.color = Color.white;
				
			GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		
	}
	
	
	void BasicSettingsBox() {
		GUISkin blankSkin = GUIF.blankSkin;
		Color lastColor = GUI.color;
		
		GUILayout.BeginVertical("box");
		
			GUILayout.Label("Basic Settings");
			editing.name = TextField("Name", editing.name);
			editing.baseName = TextField("Base Name", editing.baseName);
			editing.type = TextField("Type", editing.type);
			editing.desc = TextArea("Description", editing.desc);
			
			
			
			
			
			GUILayout.BeginHorizontal("box");
				GUILayout.Label("Icon", GUILayout.Width(50));
				GUILayout.BeginVertical(GUILayout.Width(400));
					string lastIconName = editing.iconName;
					editing.iconName = TextField("Icon Name", editing.iconName, .3f);
					if (lastIconName != editing.iconName) { editing.ReloadIcon(); }
					
					if (GUILayout.Button("Use Name")) {
						editing.iconName = editing.name;
						editing.ReloadIcon();
					}
					editing.color = ColorField("Color", editing.color);
					
				GUILayout.EndVertical();
				
				Texture2D icon = editing.icon;
				GUI.color = editing.color;
				if (icon != null) {
					GUIStyle iconStyle = blankSkin.label.Aligned(TextAnchor.MiddleCenter);
					iconStyle.fixedHeight = 64;
					iconStyle.fixedWidth = 64;
					
					GUILayout.Label(editing.icon, iconStyle);
				} else {
					GUILayout.Label("Icon\nNot\nFound", GUILayout.Width(64));
				}
				
				
			GUILayout.EndHorizontal();
			
			GUI.color = lastColor;
		
		GUILayout.EndVertical();
	}
	
	
	void PropertiesBox() {
		Color lastColor = GUI.color;
		
		GUILayout.BeginVertical("box");
			GUILayout.Label("Properties");
			
			
			
			GUILayout.BeginHorizontal("box");
				GUILayout.BeginHorizontal("box");
					editing.stacks = GUILayout.Toggle(editing.stacks, "Stacks");
				GUILayout.EndHorizontal();
				editing.maxStack = IntField("Max Stack", editing.maxStack);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal("box");
				GUILayout.BeginHorizontal("box");
					editing.equip = GUILayout.Toggle(editing.equip, "Equippable");
				GUILayout.EndHorizontal();
				editing.equipSlot = IntField("Equip Slot", editing.equipSlot);
			GUILayout.EndHorizontal();
			
			//GUILayout.Space(fieldWidth*.3f);
			
			
			
			GUILayout.BeginHorizontal();
			editing.value = FloatField("Value", editing.value, .2f);
			GUI.color = editing.rarityColor;
			editing.rarity = FloatField("Rarity", editing.rarity, .2f);
			GUI.color = lastColor;
			editing.quality = FloatField("Quality", editing.quality, .2f);
			GUILayout.EndHorizontal();
			
			
		GUILayout.EndVertical();
	}
	
	void StatsBox() {
		GUILayout.BeginVertical("box");
			GUILayout.Label("Item Stats Options");
			numOptions = IntField("Number", numOptions);
			
			OptionsButtons();
			numOptions = (int)numOptions.Clamp(0, 100);
			
			while (numOptions > stats.Count) { stats.Add(new OptionEntry("blank", 0)); }
			while (numOptions < stats.Count) { stats.RemoveAt(stats.Count-1); }
			for (int i = 0; i < stats.Count; i++) { DrawOption(i); }
			
			if (removeAt >= 0) { stats.RemoveAt(removeAt); removeAt = -1; }
			numOptions = stats.Count;
			
			OptionsButtons();
			
		GUILayout.EndVertical();
	}
	
	
	
	
	
	
	
	void WriteDatabase() {
		string str = "";
		
		for (int i = 0; i < items.Count; i++) {
			str += items[i].ToString();
			if (i < items.Count-1) { str += "\n"; }
		}
		
		StreamWriter sr = File.CreateText(target);
		sr.WriteLine(str);
		sr.Close();
		
		listChanged = false;
		
	}
	
	void LoadDatabase() {
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
		if (File.Exists(target)) {
			string[] lines = File.ReadAllLines(target);
			items = Item.LoadLinesAsList(lines);
			
		} else {
			WriteDatabase();
		}
		listChanged = false;
	}
	
	void ApplySelection() {
		editing.stats = stats.ToTable();
		
		//Debug.Log(editing.stats);
		items[selection] = editing;
		editing = items[selection].Clone();
		changed = false;
	}
	
	void LoadSelection() {
		editing = items[selection].Clone();
		stats = editing.stats.ToListOfOptions();
		numOptions = stats.Count;
		changed = false;
	}
	
	
	void OptionsButtons() {
		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		if (GUILayout.Button("+", GUILayout.Width(20))) { numOptions++; }
		GUILayout.EndHorizontal();
	}
	
	void DrawOption(int i) {
		OptionEntry o = stats[i];
		GUILayout.BeginHorizontal("box");
			if (GUILayout.Button("-", GUILayout.Width(20))) { removeAt = i; }
			o.name = GUILayout.TextField(o.name);
			o.value = EditorGUILayout.FloatField(o.value);
		GUILayout.EndHorizontal();
	}
	
	
}



public class OptionEntry {
	public string name;
	public float value;
	
	public OptionEntry(string s, float f) { name = s; value = f; }
}


public static class ItemDatabaseUtils {
	public static Table ToTable(this List<OptionEntry> list) {
		Table t = new Table();
		foreach (OptionEntry o in list) { t[o.name] = o.value; }
		return t;
	}
	
	public static List<OptionEntry> ToListOfOptions(this Table t) {
		List<OptionEntry> list = new List<OptionEntry>();
		foreach (string s in t.Keys) { list.Add(new OptionEntry(s, t[s])); }
		return list;
	}
}



#endif








