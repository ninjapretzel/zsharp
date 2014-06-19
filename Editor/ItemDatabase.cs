#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : ZEditorWindow {
	
	public static string path { get { return Application.dataPath + "/Data/Resources/"; } } 
	public static string target { get { return path + "Items.csv"; } }
	
	[System.NonSerialized] private Table equipSlots;
	
	[System.NonSerialized] private List<Item> items;
	
	[System.NonSerialized] private List<OptionEntry> stats;
	[System.NonSerialized] private StringMap strings;
	[System.NonSerialized] private int numOptions;
	
	[System.NonSerialized] private int removeAt = -1;
	[System.NonSerialized] private int moveUp = -1;
	[System.NonSerialized] private int moveDown = -1;
	
	[System.NonSerialized] private Vector4 selectScroll;
	[System.NonSerialized] private Vector2 editScroll;
	
	[System.NonSerialized] private int selection;
	[System.NonSerialized] private Item editing;
	
	[System.NonSerialized] private bool listChanged = false;
	[System.NonSerialized] private Vector2 listScroll = Vector2.zero;
	
	
	[System.NonSerialized] private bool searchMode = false;
	[System.NonSerialized] private string searchString = "";
	
	public override float fieldWidth { get { return .33f * position.width; } }
	
	public Color selectedColor { get { return new Color(.5f, .5f, 1); } }
	
	//private int numStringOptions;
	
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
		moveUp = -1;
		moveDown = -1;
		
		editing = new Item();
		listChanged = false;
		changed = false;
		selection = 0;
		
		items.Add(editing);
		
		TextAsset equipCSV = Resources.Load<TextAsset>("EquipSlots");
		if (equipCSV != null) { 
			equipSlots = new Table(equipCSV);
		} else {
			equipSlots = new Table();
		}
		
		
		
	}
	
	
	void OnGUI() {
		checkChanges = false;
		BeginHorizontal("box"); {
			ListScrollArea();
			
			EditSelection();
			
			
		} EndHorizontal();
		
		
	}
	
	
	void Sort(Comparison<Item> sortFunc) {
		Item item = items[selection];
		listChanged = true;
		
		items.Sort(sortFunc);
		
		selection = items.IndexOf(item);
		
	}
	
	void ListScrollArea() {
	
		
		BeginVertical("box"); {
		
			BeginHorizontal("box"); {
				FixedLabel("Sort:");
				
				FixedLabel("Name");
				if (FixedButton("/\\")) { Sort(Item.Sorts.NameRarityA); }
				if (FixedButton("\\/")) { Sort(Item.Sorts.NameRarityD); }
				
				FixedLabel("Type");
				if (FixedButton("/\\")) { Sort(Item.Sorts.TypeRarityA); }
				if (FixedButton("\\/")) { Sort(Item.Sorts.TypeRarityD); }
				
				FixedLabel("Rarity");
				if (FixedButton("/\\")) { Sort(Item.Sorts.RarityTypeA); }
				if (FixedButton("\\/")) { Sort(Item.Sorts.RarityTypeD); }
				
				
			} EndHorizontal();
			
			/* NOT IMPLEMENTED YET
			BeginHorizontal("box"); {
				FixedLabel("Search:");
				searchMode = ToggleButton(searchMode, "Name", "Type", Width(65));
				
				
			} EndHorizontal();
			//*/
			
			listScroll = BeginScrollView(listScroll, false, true, Width(350), Height(height - 100) ); {
				
				int lastSelection = selection;
				
				BeginVertical("box"); {
					
					for (int i = 0; i < items.Count; i++) {
						Item item = items[i];
						
						if (selection == i) {
							GUI.color = selectedColor;
						} else {
							GUI.color = Color.white;
						}
						
						Rect area = BeginVertical("button"); {
							
							BeginHorizontal("box"); {
								Box(""+i, Width(35));
								Box("[" + item.name.MinSubstring(10, '-') + "]", Width(100));
								
								FlexibleSpace();
								Box("[" + item.type + "]", Width(100));
								FlexibleSpace();
								
								GUI.color = item.rarityColor;
								Box("" + item.rarity, Width(40));
								GUI.color = Color.white;
								
							} EndHorizontal();
							
							BeginHorizontal(); {
								if (FixedButton("-")) { listChanged = true; items.RemoveAt(i); }
								if (FixedButton("D")) { listChanged = true; items.Insert(i, items[i].Clone()); }
								
								if (FixedButton("\\/")) { listChanged = true; items.Swap(i, i+1); }
								if (FixedButton("/\\")) { listChanged = true; items.Swap(i, i-1); }
							} EndHorizontal();
							
							
							
							if (BlankButton(area)) { selection = i; }
							
						} EndHorizontal();
						
					}
					
				} EndVertical();
				
				if (selection != lastSelection) {
					Unfocus();
					LoadSelection();
				}
				
				lastSelection = selection;
				
				//Draw controls at the bottom
				
				
				
			} EndScrollView();
			
			if (Button("Revert")) {
				Init();
				Unfocus();
			}
			
			SetChangedColor(listChanged);
			if (Button("Save")) {
				WriteDatabase();
				Unfocus();
			}
			
			SetChangedColor();
			if (Button("Apply and Save")) {
				ApplySelection();
				WriteDatabase();
				Unfocus();
			}
			
			
			GUI.color = Color.white;
			
		} EndVertical();
		
	}
	
	
	
	
	
	void EditSelection() {
		checkChanges = true;
		BeginVertical("box"); {
	
			editScroll = BeginScrollView(editScroll); {
			
				BeginVertical("box"); {
					
					BasicSettingsBox();
						
					PropertiesBox();
					
					
					
					StatsBox();
					
				} EndVertical();
				
			} EndScrollView();
			
			Space(10);
			GUI.color = changed ? Color.red : Color.white;

			if (Button("Apply And Save")) {
				ApplySelection();
				WriteDatabase();
				Unfocus();
			}
			
			if (Button("Apply Item")) {
				listChanged = changed || listChanged;
				ApplySelection();
				Unfocus();
			}
			
			
			GUI.color = Color.white;
			
		} EndVertical();
	}
	
	
	void BasicSettingsBox() {
		GUISkin blankSkin = GUIF.blankSkin;
		Color lastColor = GUI.color;
		
		BeginVertical("box"); {
		
			Label("Basic Settings");
			strings["name"] = TextField("Name", strings["name"], .5f);
			BeginHorizontal(); {
				Space(150);
				if (Button("\\/ Set \\/", Width(150))) {
					Unfocus();
					strings["baseName"] = strings["name"];
				}
			} EndHorizontal();
			
			strings["baseName"] = TextField("Base Name", strings["baseName"], .5f);
			strings["type"] = TextField("Type", strings["type"], .5f);
			strings["desc"] = TextArea("Description", strings["desc"]);
			
			BeginHorizontal("box", ExpandWidth(false)); {
				Label("Icon", Width(50));
				BeginVertical(Width(200)); {
					//string lastIconName = strings["iconName"];
					strings["iconName"] = TextField("Icon Name", strings["iconName"], .3f);
					//if (lastIconName != strings["iconName"]) { editing.ReloadIcon(); }
					
					BeginHorizontal(); {
						if (Button("Use Name")) {
							Unfocus();
							
							if (strings["name"] != strings["iconName"]) { changed = true; }
							strings["iconName"] = strings["name"] ;
							//editing.ReloadIcon();
						}
						FlexibleSpace();
					} EndHorizontal();
					
					editing.color = ColorField("Color", editing.color);
					
					editing.blendAmount = FloatField("Blend Amount", editing.blendAmount, .3f); 
				} EndVertical();
				
				
				
				Texture2D icon = Resources.Load<Texture2D>(strings["iconName"]);
				GUI.color = editing.color;
				if (icon != null) {
					GUIStyle iconStyle = blankSkin.label.Aligned(TextAnchor.MiddleCenter);
					iconStyle.fixedHeight = 64;
					iconStyle.fixedWidth = 64;
					
					GUILayout.Label(icon, iconStyle, ExpandWidth(false));
				} else {
					Label("Icon\nNot\nFound", Width(64));
				}
				
			} EndHorizontal();
			
			GUI.color = lastColor;
		
		} EndVertical();
		
	}
	
	
	void PropertiesBox() {
		Color lastColor = GUI.color;
		
		BeginVertical("box", ExpandWidth(false)); {
			FixedLabel("Properties");
			
			//Stack Settings
			BeginHorizontal("box"); {
				BeginHorizontal("box", ExpandWidth(false)); {
					editing.stacks = ToggleField(editing.stacks, "Stacks");
					
				} EndHorizontal();
				editing.maxStack = IntField("Max Stack", editing.maxStack);
				
			} EndHorizontal();
			
			//Equipment Settings
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
			
			if (equipSlots != null) {
				BeginHorizontal(ExpandWidth(false)); {
					Space(250);
					FixedLabel("Slot: ");
					Space(5);
					
					FixedLabel(equipSlots.GetKey(editing.minSlot));
					if (editing.equipInRange) {
						FixedLabel(" - " + equipSlots.GetKey(editing.maxSlot));
					}
				} EndHorizontal();
			}
			
			
			
			//Space(fieldWidth*.3f);
			
			
			
			BeginHorizontal(); {
				editing.value = FloatField("Value", editing.value, .3f);
				GUI.color = editing.rarityColor;
				editing.rarity = FloatField("Rarity", editing.rarity, .3f);
				GUI.color = lastColor;
				editing.quality = FloatField("Quality", editing.quality, .3f);
			} EndHorizontal();
			
			
		} EndVertical();
		
	}
	
	void StatsBox() {
		BeginVertical("box"); {
			Label("Item Stats Options");
			numOptions = IntField("Number", numOptions);
			
			OptionsButtons();
			numOptions = (int)numOptions.Clamp(0, 100);
			
			removeAt = -1;
			moveDown = -1;
			moveUp = -1;
			
			while (numOptions > stats.Count) { stats.Add(new OptionEntry("blank", 0)); }
			while (numOptions < stats.Count) { stats.RemoveAt(stats.Count-1); }
			for (int i = 0; i < stats.Count; i++) { DrawOption(i); }
			
			if (removeAt >= 0) { 
				changed = true; 
				stats.RemoveAt(removeAt); 
			}
			if (moveDown >= 0 && moveDown < stats.Count-1) { 
				changed = true; 
				stats.Swap(moveDown, moveDown+1);
			}
			if (moveUp >= 1) { 
				changed = true; 
				stats.Swap(moveUp, moveUp-1); 
			}
			
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
		//numStringOptions = strings.Count;
		
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
			if (Button("/\\", Width(20))) { moveUp = i; }
			if (Button("\\/", Width(20))) { moveDown = i; }
			
			
			o.name = TextField(o.name, Width(200));
			o.value = EditorGUILayout.FloatField(o.value, Width(200));
			
			changed = changed || (!o.Equals(temp));
		EndHorizontal();
	}
	
	[System.Serializable]
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









