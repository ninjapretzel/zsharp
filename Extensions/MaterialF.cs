using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MaterialF {
	
	static Dictionary<string, List<string>> colors;
	static List<string> basicList { get { List<string> list = new List<string>(); list.Add("_Color"); return list; } }
	
	static MaterialF() {
		colors = new Dictionary<string, List<string>>();
		
		string[] lines = Resources.Load<TextAsset>("PrimaryMaterialColors").text.Split('\n');
		
		foreach (string line in lines) {
			string[] crap = line.Split(':');
			
			string shader = crap[0];
			List<string> primaryColors = crap[1].ParseStringList();
			
			primaryColors.Append(basicList);
			colors[shader] = primaryColors;
		}
		
	}
	
	public static List<string> ColorNames(this Material material) {
		if (colors.ContainsKey(material.shader.name)) {
			
			return colors[material.shader.name];
		} else {
			
			return basicList;
		}
	}
	
}
