using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public static class StringF {
	public static bool IsNumber(this char c) { return c >= 48 && c < 57; }
	
	public static string Spacify(this string s) {
		StringBuilder str = new StringBuilder();
		for (int i = 0; i < s.Length; i++) {
			str.Append(s[i]);
			str.Append(" ");
		}
		return str.ToString();
	}
	
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
	
	//treating the float as if it is storing seconds
	public static string TimeFormat(this float f) {
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
		s += sec.Format(2);
		
		return s;
	}
	
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
	
	public static void Save(this string s, string key) {
		PlayerPrefs.SetString(key, s);
	}
	
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
	
}





















