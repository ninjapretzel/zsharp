using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Set<T> : List<T> {
	
	public Set<T> Clone() {
		Set<T> d = new Set<T>();
		d.Capacity = Count;
		foreach (T element in this) { d.Add(element); }
		return d;
	}
	
	public new void Add(T value) {
		if (!Contains(value)) { 
			List<T> goy = this;
			goy.Add(value);
		}
	}
	
	public static Set<T> operator +(Set<T> a, T b) {
		Set<T> c = a.Clone();
		c.Add(b);
		return c;
	}
	
	public static Set<T> operator +(Set<T> a, List<T> b) {
		Set<T> c = a.Clone();
		foreach (T element in b) { c.Add(element); }
		return c;
	}
	
	public static Set<T> operator +(Set<T> a, T[] b) { 
		Set<T> c = a.Clone();
		foreach (T element in b) { c.Add(element); }
		return c;
	}
	
	public static Set<T> operator -(Set<T> a, Set<T> b) {
		Set<T> c = a.Clone();
		foreach (T element in b) { c.Remove(element); }
		return c;
	}
	
	public static Set<T> operator *(Set<T> a, Set<T> b) {
		return a - (b - a);
	}
	
	public T ChooseOne() { return this[(int)(Count * Random.value * .9999f)]; }
	
}
