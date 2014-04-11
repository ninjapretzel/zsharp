using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public static class StringF {

	//Misc functions
	public static bool IsNumber(this char c) { return c >= 48 && c < 57; }
	public static bool Contains(this string str, string s) { return str.IndexOf(s) != -1; }
	
	//Newline constants
	public static string WINDOWS_NEWLINE { get { return "" + (char)0x0d + (char)0x0a; } }
	public static string UNIX_NEWLINE { get { return "" + (char)0x0a; } }
	public static string MAC_NEWLINE { get { return "" + (char)0x0d; } }
	
	//Misc function to generate a string that has a space inserted every other character
	public static string Spacify(this string s) {
		StringBuilder str = new StringBuilder();
		for (int i = 0; i < s.Length; i++) {
			str.Append(s[i]);
			str.Append(" ");
		}
		return str.ToString();
	}
	
	//Returns the given string with newline characters converted to the Unix standard
	public static string ConvertNewlines(this string s) {
		string ss = s.Replace(WINDOWS_NEWLINE, UNIX_NEWLINE);
		return ss.Replace(MAC_NEWLINE, UNIX_NEWLINE);
	}
	
	//Returns the substring of a string up to the last instance of a character.
	public static string UpToLast(this string s, char c) {
		int lastIndex = s.LastIndexOf(c);
		return s.Substring(0, lastIndex);
	}
	
	//Moves a string representing a path to point one directory above.
	public static string PreviousDirectory(this string path) {
		return path.Substring(0, path.LastIndexOf("/"));
	}
	
	//Returns the given string with all spaces and tabs removed
	public static string RemoveWhitespace(this string s) {
		return s.Replace(" ", "").Replace("\t", "");
	}
	
	//Returns a given string with all characters from another given string removed.
	public static string RemoveAllChars(this string str, string s) {
		string ss = str;
		foreach (char c in s) { ss = ss.RemoveAll(c); }
		return ss;
	}
	
	public static string RemoveAll(this string str, string s) { return str.Replace(s, ""); }
	public static string RemoveAll(this string str, char c) { return str.Replace(""+c, ""); }
	
	//Format a float to some number of decimal places
	public static string Format(this float f, int dec) {
		int ir = (int)f;
		float fr = f - ir;
		string s = "";
		if (dec > 0 && fr > 0) {
			s = "" + fr;
			if (s.Length <= dec+1) { s = s.Substring(1); }
			else { s = s.Substring(1, dec+1); }
		}
		return "" + ir + s;
	}
	
	//Formats a float as if it represents time in seconds
	public static string TimeFormat(this float f) { return f.TimeFormat(2); }
	public static string TimeFormat(this float f, int places) {
		float hr = Mathf.Floor((f / 3600.0f));
		float min = Mathf.Floor((f / 60.0f) % 60.0f);
		
		float sec = (f % 60.0f);
		
		string s = "";
		if (hr > 0) { 
			s += hr + ":"; 
			if (min < 10) { s += "0"; }
		}
		
		s += min + ":";
		if (sec < 10) { s += "0"; }
		s += sec.Format(places);
		
		return s;
	}
	
	//Formats an int value so commas are inserted every 3 places.
	public static string Commify(this int i) {
		string str = "" + i;
		int ind = str.Length;
		ind -= 3;
		while (ind > 0) {
			str = str.Insert(ind, ",");
			ind -= 3;
		}
		return str;
	}
	
	//Parsing functions
	public static float ParseFloat(this string s) { return float.Parse(s); }
	public static int ParseInt(this string s) { return int.Parse(s); }
	public static Color ParseColor(this string s) { return ColorF.FromString(s); }
	public static Color ParseColor(this string s, char delim) { return ColorF.FromString(s, delim); }
	public static Table ParseTable(this string s) { return Table.CreateFromLine(s); }
	public static Table ParseTable(this string s, char delim) { return Table.CreateFromLine(s, delim); }
	
	//Parse a date from a string
	public static System.DateTime ParseDate(this string s) {
		char[] splits = new char[3];
		splits[0] = '/';
		splits[1] = ' ';
		splits[2] = ':';
		
		
		string[] strs = s.Split(splits);
		if (strs.Length < 6) { return System.DateTime.Now.AddDays(-1); }
		int year = int.Parse(strs[2]);
		int month = int.Parse(strs[0]);
		int day = int.Parse(strs[1]);
		
		int hr = int.Parse(strs[3]);
		int min = int.Parse(strs[4]);
		int sec = int.Parse(strs[5]);
		
		if (strs[6] == "PM") { hr += 12; }
		
		System.DateTime dt = new System.DateTime(year, month, day, hr, min, sec);
		
		return dt;
	}
	
	//Convert a System.DateTime object to a string with a consistant representation.
	//Can be safely used for storing/loading dates across platforms and locales
	public static string DateToString(this System.DateTime dt) {
		string str = "";
		
		int hr = dt.Hour;
		int min = dt.Minute;
		int sec = dt.Second;
		bool am = true;
		if (hr > 12) { hr -= 12; am = false; }
		
		int year = dt.Year;
		int month = dt.Month;
		int day = dt.Day;
		
		str = "" + month + "/" + day + "/" + year + " ";
		if (hr < 10) { str += "0"; }
		str += "" + hr + ":";
		
		if (min < 10) { str += "0"; }
		str += "" + min + ":";
		
		if (sec < 10) { str += "0"; }
		str += "" + sec + " ";
		if (am) { str += "AM"; }
		else { str += "PM"; }
		
		return str;
	}
	
	//Wrap PlayerPrefs.SetString
	public static void Save(this string s, string key) {
		PlayerPrefs.SetString(key, s);
	}
	
	//Extracts a section between matching '{' and '}' characters
	public static string ExtractSection(this string s) { return s.ExtractSection(0); }
	public static string ExtractSection(this string s, int st) {
		int start = s.IndexOf('{', st);
		Stack<int> stack = new Stack<int>();
		
		stack.Push(start);
		
		int end = s.Length;
		int i = start;
		while (stack.Count > 0) {
			int open = s.IndexOf('{', i+1);
			int close = s.IndexOf('}', i+1);
			
			if (open == -1 || close == -1) {
				if (close == -1) {
					return s.Substring(start);
				} else {
					stack.Pop();
					if (stack.Count == 0) { end = close; }
					i = close;
				}
			
			} else {
				if (open < close) {
					stack.Push(open);
					i = open;
				} else {
					stack.Pop();
					if (stack.Count == 0) { end = close; }
					i = close;
				}
			}
			
		}
		
		
		return s.Substring(start+1, (end-start-1));
	}
	
	//Parse a string and replace all escape character newlines with the real thing
	//used on strings that are expected to be modified from the inspector so they can support newline characters.
	public static string ParseNewlines(this string input) {
		return input.Replace("\\n", "\n");
	}
	
	
	
}





















