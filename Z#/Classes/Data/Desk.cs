using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Desk : Dictionary<string, Table> {
	
	public Desk Clone() {
		Desk d = new Desk();
		foreach (string key in Keys) { d[key] = this[key]; }
		return d;
	}
	
	public new Table this[string key] {
		get {
			Dictionary<string, Table> goy = this;
			if (!goy.ContainsKey(key)) { return new Table(); }
			return goy[key];
		}
		
		set {
			Dictionary<string, Table> goy = this;
			if (goy.ContainsKey(key)) { goy[key] = value; }
			else { goy.Add(key, value); }
		}
	}
	
	public override string ToString() {
		StringBuilder str = new StringBuilder("#Formatted Desk as .csv:");
		foreach (string key in Keys) {
			str.Append("\n");
			str.Append(key);
			
			Table t = this[key];
			foreach (string k in t.Keys) {
				str.Append(",");
				str.Append(k);
				str.Append(",");
				str.Append(t[k]);
			}
		}
		return str.ToString();
		
	}
	
	
	public void LoadCSV(string csv) {
		Clear();
		string[] lines = csv.Split('\n');
		
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i][0] == '#') { continue; }
			string[] content = lines[i].Split(',');
			string key = content[0];
			this[key] = new Table();
			for (int j = 1; j < content.Length; j += 2) {
				this[key].Add(content[j], float.Parse(content[j+1]));
			}
			
		}
		
		
	}
	
}
