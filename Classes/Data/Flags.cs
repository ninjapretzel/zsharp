using UnityEngine;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class Flags : Dictionary<int, bool> {
	
	public int maxIndex { get { 
		int highest = -1;
		foreach (int i in Keys) {
			if (i > highest) { highest = i; }
		}
		return highest;
	}}
	
	public new bool this[int i] {
		get { 
			if (ContainsKey(i)) {
				Dictionary<int, bool> dict = (Dictionary<int, bool>)this;
				return dict[i];
			}
			return false;
		}
		
		set {
			if (ContainsKey(i)) {
				Dictionary<int, bool> dict = (Dictionary<int, bool>)this;
				dict[i] = value;
			} else {
				Add(i, value);
			}
		}
	}
	
	public byte[] ToBytes() {
		int max = maxIndex;
		if (max == -1) { return new byte[0]; }
		int size = (max / 8) + ((max % 8 == 0) ? 0 : 1);
		byte[] array = new byte[size];
		
		for (int i = 0; i < (max+8); i += 8) {
			int index = i / 8;
			
			byte b = 0;
			for (int j = 0; j < 8; j++) {
				if (this[i+j]) { b += (byte)(1 << j); }
			}
			
			array[index] = b;
		}
		
		return array;
	}
	
	public void LoadBytes(byte[] bytes){ 
		Clear();
		
		int index = 0;
		for (int i = 0; i < bytes.Length; i++) {
			byte b = bytes[i];
			
			for (int j = 7; j >= 0; j--) {
				if (b > (1 << j)) {
					b -= (byte)(1 << j);
					this[index] = true;
				}
				
				
				index++;
			}
			
			
			
		}
		
		
	}
	
	public void Save(string key) {
		
	}
	
	public void Load(string key) {
		
	}
	
}