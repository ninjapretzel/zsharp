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
		colorMap.Add('n', C(.52549, 0.7098, .85098) );
		
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
		
		//Super bad hack to make the shit not break on handling the last line.
		//Re-do this function to fix this process.
		//Shouldn't have to rely on a hack like this.
		string msg = str + " \\w \n";
		
		
		
		while (i < msg.Length && safety++ < 100) {
			
			
			int nextLine;
			int nextEscape; 
			nextLine = msg.IndexOf('\n', i);
			nextEscape = msg.IndexOf('\\', i);
			
			
			if (nextLine == -1) { nextLine = msg.Length + 100; }
			if (nextEscape == -1) { nextEscape = msg.Length + 100; }
			
			int pos = msg.IndexOf(' ', i);
			
			int len = pos - i;
			string s;
			if (pos == -1) {
				len = msg.Length - i;
			}
			
			//Handle this stupid hack//
			if (pos == msg.Length - 1) {
				return;
			}
			
			if (nextEscape < nextLine) {
				
				if (nextEscape < pos) {
					
					if (nextEscape+1 < msg.Length) {
						char c = msg[nextEscape+1];
						s = msg.Substring(i, nextEscape - i);
						
						Label(s);
						
						
						ChangeColor(c);
						
					} else {
						s = msg.Substring(i, nextEscape - i);
						Label(s);
						
					}
					
					i = nextEscape + 2;
					
					continue;
				}
				
			} else if (nextLine < nextEscape) {
			
				if (nextLine < pos) {
					s = msg.Substring(i, nextLine - i);
					
					Label(s);
					Newline();
					i = nextLine+1;
					
					continue;
				}
				
			}
			
			if (len == 0) {
				i = pos + 1; 
				continue;
			}
			
			s = msg.Substring(i, len);
			
			if (s[0] == '\\') {
				char c = s[1];
				ChangeColor(c);
				//i = pos + 1;
				//continue;
				//s = s.Substring(2);
				
			}
			
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
