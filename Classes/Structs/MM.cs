using UnityEngine;
using System.Collections;

[System.Serializable]
public class MM {
	public float min;
	public float max;
	
	public float value { get { return RandomF.Range(min, max); } }
	public float normal { get { return RandomF.Normal(min, max); } }

	
	public MM() {
		min = 0;
		max = 1;
	}
	
	public MM(float a, float b) {
		min = Mathf.Min(a, b);
		max = Mathf.Max(a, b);
	}
	
	public override string ToString() { return ToString(','); }
	public string ToString(char delim) { return "" + min + delim + max; }
	
	public void LoadFromString(string str) {
		string[] cells = str.Split(',');
		if (cells.Length != 2) { 
			Debug.LogWarning("Trying to load malformed string into range.");
			return;
		}
		
		min = cells[0].ParseFloat();
		max = cells[1].ParseFloat();
	}
	
}
