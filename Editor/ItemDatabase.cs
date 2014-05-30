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
	
	private List<Item> items;
	
	private List<OptionEntry> stats;
	private StringMap strings;
	private int numOptions;
	private int numStringOptions;
	private int removeAt;
	
	private Vector4 selectScroll;
	private Vector2 editScroll;
	
	private int selection;
	private Item editing;
	
	private bool listChanged = false;
	public new float fieldWidth { get { return .6f * position.width; } }
	
	[MenuItem ("Window/Item Database")]
	static void ShowWindow() {
		EditorWindow.GetWindow(typeof(ItemDatabase));
		
		
		
	}
	
	//
	public ItemDatabase() : base() {
		Init();
		//main.items.Add(new Item());
		LoadDatabase();
		
		if (items.Count == 0) { items.Add(new Item()); }
		LoadSelection();
		
	}
	
	void Init() {
		items = new List<Item>();
		stats = new List<OptionEntry>();
		numOptions = 0;
		removeAt = -1;
		editing = new Item();
		listChanged = false;
		changed = false;
		selection = 0;
		
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
		BeginHorizontal();
			Space(210);
			
			BeginVertical("box");
			
				editScroll = BeginScrollView(editScroll);
				
					BeginVertical("box");
					
						BasicSettingsBox();
							
						PropertiesBox();
						
						
						
						StatsBox();
						
					EndVertical();
					
				EndScrollView();
				
				Space(10);
				GUI.color = changed ? Color.red : Color.white;

				if (Button("Apply And Save")) {
					ApplySelection();
					WriteDatabase();
				}
				
				if (Button("Apply Item")) {
					listChanged = changed || listChanged;
					ApplySelection();
				}
				
				
				GUI.color = Color.white;
				
			EndVertical();
		EndHorizontal();
		
	}
	
	
	void BasicSettingsBox() {
		GUISkin blankSkin = GUIF.blankSkin;
		Color lastColor = GUI.color;
		
		BeginVertical("box"); {
		
			Label("Basic Settings");
			editing.name = TextField("Name", editing.name);
			editing.baseName = TextField("Base Name", editing.baseName);
			editing.type = TextField("Type", editing.type);
			editing.desc = TextArea("Description", editing.desc);
			
			BeginHorizontal("box"); {
				Label("Icon", Width(50));
				BeginVertical(Width(400)); {
					string lastIconName = editing.iconName;
					editing.iconName = TextField("Icon Name", editing.iconName, .3f);
					if (lastIconName != editing.iconName) { editing.ReloadIcon(); }
					
					if (Button("Use Name")) {
						if (editing.name != editing.iconName) { changed = true; }
						editing.iconName = editing.name;
						editing.ReloadIcon();
					}
					
					editing.color = ColorField("Color", editing.color);
					
				} EndVertical();
				
				Texture2D icon = editing.icon;
				GUI.color = editing.color;
				if (icon != null) {
					GUIStyle iconStyle = blankSkin.label.Aligned(TextAnchor.MiddleCenter);
					iconStyle.fixedHeight = 64;
					iconStyle.fixedWidth = 64;
					
					GUILayout.Label(editing.icon, iconStyle);
				} else {
					Label("Icon\nNot\nFound", Width(64));
				}
				
			} EndHorizontal();
			
			GUI.color = lastColor;
		
		} EndVertical();
		
	}
	
	
	void PropertiesBox() {
		Color lastColor = GUI.color;
		
		BeginVertical("box"); {
			Label("Properties");
			
			BeginHorizontal("box"); {
				BeginHorizontal("box", ExpandWidth(false)); {
					editing.stacks = ToggleField(editing.stacks, "Stacks");
					
				} EndHorizontal();
				editing.maxStack = IntField("Max Stack", editing.maxStack);
				
			} EndHorizontal();
			
			BeginHorizontal("box"); {
				BeginHorizontal("box"); { 
					
					editing.equip = ToggleField(editing.equip, "Equippable");
					
				} EndHorizontal();
				
				BeginHorizontal("box"); {
					editing.equipInRange = ToggleField(editing.equipInRange, "Equips To Range");
				} EndHorizontal();
				
				if (editing.equipInRange) {
					BeginHorizontal("box"); {
						editing.equipInWholeRange = ToggleField(editing.equipInWholeRange, "Exclusive");
						FixedLabel("Min Slot: ");
						editing.minSlot = EditorGUILayout.IntField(editing.minSlot, ExpandWidth(false));
						FixedLabel("Max Slot: ");
						editing.maxSlot = EditorGUILayout.IntField(editing.maxSlot, ExpandWidth(false));
					} EndHorizontal();
				} else {
					editing.equipSlot = IntField("Equip Slot", editing.equipSlot, .5f);
				}
				
				
				
			} EndHorizontal();
			
			//Space(fieldWidth*.3f);
			
			
			
			BeginHorizontal(); {
				editing.value = FloatField("Value", editing.value, .2f);
				GUI.color = editing.rarityColor;
				editing.rarity = FloatField("Rarity", editing.rarity, .2f);
				GUI.color = lastColor;
				editing.quality = FloatField("Quality", editing.quality, .2f);
			} EndHorizontal();
			
			
		} EndVertical();
		
	}
	
	void StatsBox() {
		BeginVertical("box"); {
			Label("Item Stats Options");
			numOptions = IntField("Number", numOptions);
			
			OptionsButtons();
			numOptions = (int)numOptions.Clamp(0, 100);
			
			while (numOptions > stats.Count) { stats.Add(new OptionEntry("blank", 0)); }
			while (numOptions < stats.Count) { stats.RemoveAt(stats.Count-1); }
			for (int i = 0; i < stats.Count; i++) { DrawOption(i); }
			
			if (removeAt >= 0) { stats.RemoveAt(removeAt); removeAt = -1; }
			numOptions = stats.Count;
			
			OptionsButtons();
			
		} EndVertical();
		
		
		
		BeginVertical("box"); {
			Label("Item String Options");
			
			strings = StringMapField(strings, Item.DEFAULT_STRINGS);
			
			
		} EndVertical();
		
		
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
		editing.strings = strings.Clone();
		
		items[selection] = editing;
		
		editing = items[selection].Clone();
		changed = false;
	}
	
	void LoadSelection() {
		editing = items[selection].Clone();
		stats = editing.stats.ToListOfOptions();
		strings = editing.strings.Clone();
		
		numOptions = stats.Count;
		numStringOptions = strings.Count;
		
		changed = false;
	}
	
	
	void OptionsButtons() {
		BeginHorizontal();
		Space(20);
		if (Button("+", Width(20))) { 
			numOptions++; 
			changed = changed || checkChanges;
		}
		EndHorizontal();
	}
	
	void DrawOption(int i) {
		OptionEntry o = stats[i];
		BeginHorizontal("box");
			OptionEntry temp = new OptionEntry(o);
			
			if (Button("-", Width(20))) { removeAt = i; }
			
			o.name = TextField(o.name);
			o.value = EditorGUILayout.FloatField(o.value);
			
			changed = changed || (!o.Equals(temp));
		EndHorizontal();
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









