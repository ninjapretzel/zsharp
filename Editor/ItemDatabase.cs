#if UNITY_EDITOR
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
	public new float fieldWidth { get { return .6f * position.width; } }
	
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
		
		items.Add(editing);
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
		int selectAction = (int)selectScroll.w;
		
		if (selectAction == 1) { listChanged = true; items.Insert(selection, items[selection].Clone()); }
		if (selectAction == -1) { listChanged = true; items.RemoveAt(selection); selection = (int)selection.Clamp(0, items.Count); }
		selection = (int)selectScroll.z.Clamp(0, items.Count-1);
		
		
		if (selection != lastSelection) {
			GUI.SetNextControlName("Dicks");
			GUI.TextField(new Rect(0, -300, 100, 30), "fuck dicks");
			GUI.FocusControl("Dicks");
			LoadSelection();
		}
		
		
		Rect infoArea = new Rect(0, selectArea.yMax, selectArea.width + 10, position.height - selectArea.yMax);
		GUI.Box(infoArea, "");
		
		Rect line = infoArea.Top(.25f);
		GUI.Label(line, "Number of items: " + items.Count);
		
		line = line.MoveDown();
		GUI.color = (changed || listChanged) ? Color.red : Color.white;
		
		if (GUI.Button(line, "Apply and Save")) {
			ApplySelection();
			WriteDatabase();
		}
		
		GUI.color = listChanged ? Color.red : Color.white;
		line = line.MoveDown();
		if (GUI.Button(line, "Save Database")) {
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
				
				if (GUILayout.Button("Apply And Save")) {
					ApplySelection();
					WriteDatabase();
				}
				
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
		
		GUILayout.BeginVertical("box"); {
		
			GUILayout.Label("Basic Settings");
			editing.name = TextField("Name", editing.name);
			editing.baseName = TextField("Base Name", editing.baseName);
			editing.type = TextField("Type", editing.type);
			editing.desc = TextArea("Description", editing.desc);
			
			GUILayout.BeginHorizontal("box"); {
				GUILayout.Label("Icon", GUILayout.Width(50));
				GUILayout.BeginVertical(GUILayout.Width(400)); {
					string lastIconName = editing.iconName;
					editing.iconName = TextField("Icon Name", editing.iconName, .3f);
					if (lastIconName != editing.iconName) { editing.ReloadIcon(); }
					
					if (GUILayout.Button("Use Name")) {
						if (editing.name != editing.iconName) { changed = true; }
						editing.iconName = editing.name;
						editing.ReloadIcon();
					}
					
					editing.color = ColorField("Color", editing.color);
					
				} GUILayout.EndVertical();
				
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
				
			} GUILayout.EndHorizontal();
			
			GUI.color = lastColor;
		
		} GUILayout.EndVertical();
		
	}
	
	
	void PropertiesBox() {
		Color lastColor = GUI.color;
		
		GUILayout.BeginVertical("box");
			GUILayout.Label("Properties");
			
			bool temp;
			int tempNum;
			
			GUILayout.BeginHorizontal("box");
				GUILayout.BeginHorizontal("box");
					temp = editing.stacks;
					editing.stacks = GUILayout.Toggle(editing.stacks, "Stacks");
					if (editing.stacks != temp) { changed = true; }
					
				GUILayout.EndHorizontal();
				tempNum = editing.maxStack;
				editing.maxStack = IntField("Max Stack", editing.maxStack);
				if (editing.maxStack != tempNum) { changed = true; }
				
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal("box");
				GUILayout.BeginHorizontal("box");
					temp = editing.equip;
					editing.equip = GUILayout.Toggle(editing.equip, "Equippable");
					if (editing.equip != temp) { changed = true; }
					
				GUILayout.EndHorizontal();
				tempNum = editing.equipSlot;
				editing.equipSlot = IntField("Equip Slot", editing.equipSlot);
				if (editing.equipSlot != tempNum) { changed = true; }
				
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
		AssetDatabase.Refresh();
		
	}
	
	void LoadDatabase() {
		if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
		if (File.Exists(target)) {
			items = (new Inventory(File.ReadAllText(target))) as List<Item>;
			
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
			OptionEntry temp = new OptionEntry(o);
			
			if (GUILayout.Button("-", GUILayout.Width(20))) { removeAt = i; }
			
			o.name = GUILayout.TextField(o.name);
			o.value = EditorGUILayout.FloatField(o.value);
			
			changed = changed || (!o.Equals(temp));
		GUILayout.EndHorizontal();
	}
	
	public class OptionEntry {
		public string name;
		public float value;
		
		public OptionEntry(string s, float f) { name = s; value = f; }
		public OptionEntry(OptionEntry o) { name = o.name; value = o.value; }
		
		public new bool Equals(System.Object other) {
			if (other == null) { return false; } 
			if (other.GetType() != GetType()) { return false; }
			OptionEntry o = other as OptionEntry;
			return o.name == name && o.value == value;
		}
		
	}
	
	
}






public static class ItemDatabaseUtils {
	public static Table ToTable(this List<ItemDatabase.OptionEntry> list) {
		Table t = new Table();
		foreach (ItemDatabase.OptionEntry o in list) { t[o.name] = o.value; }
		return t;
	}
	
	public static List<ItemDatabase.OptionEntry> ToListOfOptions(this Table t) {
		List<ItemDatabase.OptionEntry> list = new List<ItemDatabase.OptionEntry>();
		foreach (string s in t.Keys) { list.Add(new ItemDatabase.OptionEntry(s, t[s])); }
		return list;
	}
}



#endif









