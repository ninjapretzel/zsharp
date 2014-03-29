using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Message {
	
	const int SPACING = 4;
	
	public string str;
	public string style = "box";
	
	public static Dictionary<char, Color> colorMap;
	
	float width = 0;
	float usedThisLine = 0;
	
	static Color C(double r, double g, double b) { return new Color((float)r, (float)g, (float)b); }
	static Color C(double d) { float f = (float)d; return new Color(f, f, f); }
	Color pColor;
	Color pcColor;
		
	static Message() {
		colorMap = new Dictionary<char, Color>();
		colorMap.Add('r', Color.red );
		colorMap.Add('o', C(1, .75, 0) );
		colorMap.Add('y', Color.yellow );
		colorMap.Add('g', Color.green );
		colorMap.Add('b', Color.blue );
		colorMap.Add('i', C(.8, .8, 1) );
		colorMap.Add('v', C(.8, 0, 1) );
		
		colorMap.Add('c', Color.cyan );
		
		colorMap.Add('h', C(.5, .5, .5) );
		colorMap.Add('q', C(.8, .8, .8) );
		colorMap.Add('e', C(1, .6, 0) );
		colorMap.Add('t', C(.8, 1, .8) );
		colorMap.Add('p', C(.8, 1, .8) );
		
		//colorMap.Add('e', new Color(1, .6f, .0f) );
	}
	
	public Message() {
		str = "";
	}
	
	public Message(string s) {
		str = s;
	}
	
	public void Draw(Rect area) {
		
		width = area.Denormalized().width - GUI.skin.box.padding.left - GUI.skin.box.padding.right;
		GUILayout.BeginArea(area.Denormalized(), GUI.skin.GetStyle(style)); 
		GUILayout.BeginVertical("", GUILayout.ExpandHeight(false)); 
		GUILayout.BeginHorizontal("label", GUILayout.ExpandHeight(false));
		pColor = GUI.color;
		pcColor = GUI.contentColor;
		
		int i = 0;
		int safety = 0;
		usedThisLine = 0;
		
		while (i < str.Length && safety++ < 100) {
			
			int nextEscape = str.IndexOf('\\', i);
			int nextLine = str.IndexOf('\n', i);
			if (nextLine == -1) { nextLine = str.Length + 10; }
			if (nextEscape == -1) { nextEscape = str.Length + 10; }
			
			int pos = str.IndexOf(' ', i);
			
			int len = pos - i;
			string s;
			if (pos == -1) {
				len = str.Length - i;
			}
			
			if (nextEscape < nextLine) {
				
				if (nextEscape < pos) {
					
					if (nextEscape+1 < str.Length) {
						char c = str[nextEscape+1];
						s = str.Substring(i, nextEscape - i);
						
						Label(s);
						
						
						ChangeColor(c);
						
					} else {
						s = str.Substring(i, nextEscape - i);
						Label(s);
						
					}
					
					i = nextEscape + 2;
					
					continue;
				}
				
			} else if (nextLine < nextEscape) {
			
				if (nextLine < pos) {
					s = str.Substring(i, nextLine - i);
					Label(s);
					Newline();
					i = nextLine+1;
					
					
					continue;
				}
				
			}
			
			s = str.Substring(i, len);
			Label(s);
			
			
			if (pos != -1 && pos < str.Length) {
				if (str[pos] == ' ') {
					GUILayout.Space(SPACING);
					usedThisLine += SPACING;
				}
			}
			
			i = pos+1;
			if (pos == -1) { i = str.Length; }
			
		}
					
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		
	}
	
	
	
	void Newline() {
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("label", GUILayout.ExpandHeight(false));
		usedThisLine = 0;
	}
	
	void ChangeColor(char c){
		Color color = Color.white;
		if (colorMap.ContainsKey(c)) {
			color = colorMap[c];
		}
		
		color.a *= pColor.a;
		color.a *= pcColor.a;
		
		GUI.color = color;
	}
	
	void Label(string s) {
		Vector2 size = GUI.skin.label.CalcSize(new GUIContent(s));
		if (usedThisLine + size.x >= width) { Newline(); }
		
		GUILayout.Label(s, GUILayout.ExpandWidth(false));
		usedThisLine += size.x;
	}
	
	
}
