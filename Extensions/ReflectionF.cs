using UnityEngine;
using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public static class ReflectionF {
	public static string ShortName(this Type t) {
		if (t == typeof(void)) { return "void"; }
		else if (t == typeof(string)) { return "string"; }
		else if (t == typeof(float)) { return "float"; }
		else if (t == typeof(bool)) { return "bool"; }
		else if (t == typeof(int)) { return "int"; }
		else if (t == typeof(double)) { return "double"; }
		else if (t == typeof(long)) { return "long"; }
		else if (t == typeof(System.Object)) { return "Object"; }
		else if (t == typeof(UnityEngine.Object)) { return "UnityEngine.Object"; }
		else if (t == typeof(Event)) { return "Event"; }
		return t.ToString().FromLast('.');
	}
	
	public static bool IsInherited(this MemberInfo info) { return info.DeclaringType != info.ReflectedType; }
	public static bool IsPublic(this PropertyInfo info) {
		MethodInfo getter = info.GetGetMethod();
		MethodInfo setter = info.GetSetMethod();
		if (getter == null && setter == null) { return false; }
		
		if (getter != null && setter != null) {
			return getter.IsPublic && setter.IsPublic;
			
		} else if (setter == null) {
			return getter.IsPublic;
		} else {
			return setter.IsPublic;
		}
	}
	public static bool IsPrivate(this PropertyInfo info) {
		MethodInfo getter = info.GetGetMethod();
		MethodInfo setter = info.GetSetMethod();
		if (getter == null && setter == null) { return false; }
		
		if (getter != null && setter != null) {
			return getter.IsPrivate && setter.IsPrivate;
			
		} else if (setter == null) {
			return getter.IsPrivate;
		} else {
			return setter.IsPrivate;
		}
	}
	
	public static bool IsStatic(this PropertyInfo info) {
		MethodInfo getter = info.GetGetMethod();
		MethodInfo setter = info.GetSetMethod();
		if (getter == null && setter == null) { return false; }
		
		if (getter != null && setter != null) {
			return getter.IsStatic && setter.IsStatic;
			
		} else if (setter == null) {
			return getter.IsStatic;
		} else {
			return setter.IsStatic;
		}
	}
	
	
	
	public static void ListAllMembers(this Type type, bool showPrivate = false, bool showHidden = false) { Debug.Log(type.Summary(showPrivate, showHidden)); }
	public static string Summary(this Type type, bool showPrivate = false, bool showHidden = false) {
		//BindingFlags allPublic = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.SetField | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty;
		BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
		if (showPrivate) {
			flags = flags | BindingFlags.NonPublic;
		}
		
		List<FieldInfo> fields = type.GetFields(flags).ToList();
		
		List<PropertyInfo> properties = type.GetProperties(flags).ToList();
		
		List<MethodInfo> methods = type.GetMethods(flags).ToList();
		
		fields.Sort(CompareFields);
		methods.Sort(CompareMethods);
		
		Type lastInheritedType = null;
		
		StringBuilder output = new StringBuilder("");
		
		if (type.IsPublic) {
			output.Append("public ");
		}
		
		if (type.IsInterface) {
			output.Append("interface ");
		} else {
			if (type.IsAbstract) {
				output.Append("abstract ");
			}
			output.Append("class ");
		}
		
		output.Append(type.ShortName());
		
		
		Type[] interfaces = type.GetInterfaces();
		if (type.BaseType == typeof(System.Object) || type.BaseType == null) {
			if (interfaces.Length > 0) {
				output.Append(" : ");
			}
		} else {
			output.Append(" : " + type.BaseType.ShortName());
			if (interfaces.Length > 0) {
				output.Append(", ");
			}
			
		}

		for (int i = 0; i < interfaces.Length; i++) {
			output.Append(interfaces[i].ShortName());
			if (i != interfaces.Length-1) { 
				output.Append(", ");
			}
			
		}
		
		
		
		
		output.Append(" {");
		
		output.Append("\n\n\t//Fields:----------------------------------------------\n");
		foreach (FieldInfo info in fields) {
			if (info.IsInherited()) {
				if (info.DeclaringType != lastInheritedType) {
					lastInheritedType = info.DeclaringType;
					output.Append("\n\t//Inherited from <" + info.DeclaringType.ToString() + ">\n");
					
				}
			}
			output.Append("\t" + info.Summary() + "\n");
		}
		
		lastInheritedType = null;
		output.Append("\n\n\t//Properties:----------------------------------------------\n");
		foreach (PropertyInfo info in properties) {
			if (info.IsInherited()) {
				if (info.DeclaringType != lastInheritedType) {
					lastInheritedType = info.DeclaringType;
					output.Append("\n\t//Inherited from <" + info.DeclaringType.ToString() + ">\n");
					
				}
			}
			output.Append("\t" + info.Summary() + "\n");
		}
		
		
		//Give summary of each method info
		lastInheritedType = null;
		output.Append("\n\n\t//Methods:----------------------------------------------\n");
		foreach (MethodInfo info in methods) {
			if (info.IsSpecialName && !showHidden) { continue; }
			if (info.IsInherited()) {
				if (info.DeclaringType != lastInheritedType) {
					lastInheritedType = info.DeclaringType;
					output.Append("\n\t//Inherited from <" + info.DeclaringType.ToString() + ">\n");
					
				}
			}
			output.Append("\t" + info.Summary() + "\n");
		}
		
		
		output.Append("\n}");
		
		return output.ToString();
		
		
		
		
	}
	
	public static int CompareMethods(this MethodInfo info, MethodInfo other) {
		if (info.IsInherited() == other.IsInherited()) {
			if (info.DeclaringType == other.DeclaringType) {
				if (info.IsStatic == other.IsStatic) { 
					if (info.IsPublic == other.IsPublic) {
						if (info.IsPrivate == other.IsPrivate) {
							if (info.IsAbstract == other.IsAbstract) {
								if (info.IsVirtual == other.IsVirtual) {
									if (info.ReturnType == other.ReturnType) {
										
										return 0;
										
									} else { return info.ReturnType.ShortName().CompareTo(other.ReturnType.ShortName()); }
								} else { return (info.IsVirtual ? 1 : -1); }
							} else { return (info.IsAbstract ? 1 : -1); }
						} else { return (info.IsPrivate ? -1 : 1); }
					} else { return (info.IsPublic ? -1 : 1); }
				} else { return (info.IsStatic ? -1 : 1); }
			} else { return info.DeclaringType.ShortName().CompareTo(other.DeclaringType.ShortName()); }
		} else { return (info.IsInherited() ? 1 : -1); }
		//return 0;
	}
	
	public static int CompareFields(this FieldInfo info, FieldInfo other) {
		if (info.IsInherited() == other.IsInherited()) {
			if (info.DeclaringType == other.DeclaringType) {
				if (info.IsStatic == other.IsStatic) { 
					if (info.IsPublic == other.IsPublic) {
						if (info.IsNotSerialized == other.IsNotSerialized) {
							if (info.IsPrivate == other.IsPrivate) {
								
								return 0;
								
							} else { return (info.IsPrivate ? -1 : 1); }
						} else { return (info.IsNotSerialized ? -1 : 1); }
					} else { return (info.IsPublic ? -1 : 1); }
				} else { return (info.IsStatic ? -1 : 1); }
			} else { return info.DeclaringType.ShortName().CompareTo(other.DeclaringType.ShortName()); }
		} else { return (info.IsInherited() ? 1 : -1); }
	}
	
	
	public static string Summary(this MethodInfo info) {
		StringBuilder str = new StringBuilder();
		
		
		
		if (info.IsPublic) {
			str.Append("public ");
		} else if (info.IsPrivate) {
			str.Append("private ");
		} 
		
		if (info.IsFamily) {
			str.Append("protected ");
		}
		if (info.IsAssembly) {
			str.Append("internal ");
		}
		
		if (info.IsSpecialName) {
			str.Append("hidden ");
		}
		
		
		if (info.IsStatic) {
			str.Append("static ");
		} else {
			
			if (info.IsVirtual) {
				str.Append("virtual ");
			}
			if (info.IsAbstract) {
				str.Append("abstract ");
			}
		}
		str.Append(info.ReturnType.ShortName() + " " + info.Name + "(");
		
		ParameterInfo[] pinfos = info.GetParameters();
		for (int i = 0; i < pinfos.Length; i++) {
			ParameterInfo pinfo = pinfos[i];
			
			
			if (pinfo.IsOut) { str.Append("out "); }
			str.Append(pinfo.ParameterType.ShortName() + " " + pinfo.Name);
			/*
			//Unity's Mono does not have this functionality... :(
			if (pinfo.HasDefaultValue) {
				str.Append(" = " + pinfo.DefaultValue.ToString().RemoveAll("\n"));
			}
			//*///
			
			if (i < pinfos.Length-1) { str.Append(", "); }
			
		}
		
		str.Append(");");
		
		return str.ToString();
	}
	
	public static string Summary(this PropertyInfo info) {
		StringBuilder str = new StringBuilder("");
		
		if (info.IsPublic()) {
			str.Append("public ");
		} else if (info.IsPrivate()) {
			str.Append("private ");
		} 
		
		/*
		if (info.IsFamily) {
			str.Append("protected ");
		}
		if (info.IsAssembly) {
			str.Append("internal ");
		}
		*/
		
		if (info.IsSpecialName) {
			str.Append("hidden ");
		}
		
		
		
		if (info.IsStatic()) {
			str.Append("static ");
		} else {
			
		}
		
		str.Append(info.PropertyType.ShortName() + " " + info.Name + " {");
		
		MethodInfo getter = info.GetGetMethod();
		MethodInfo setter = info.GetSetMethod();
		
		if (getter != null) { str.Append(" get; "); }
		if (setter != null) { str.Append(" set; "); }
		
		str.Append("}");
		
		
		return str.ToString();
	}
	
	//[ZDoc("Creates A summary of a FieldInfo as an extension method.")]
	public static string Summary(this FieldInfo info) {
		StringBuilder str = new StringBuilder("");
		
		if (info.IsNotSerialized) {
			str.Append("[System.NotSerialized] ");
		}
		
		if (info.IsPublic) {
			str.Append("public ");
		} else if (info.IsPrivate) {
			str.Append("private ");
		} 
		
		
		if (info.IsFamily) {
			str.Append("protected ");
		}
		if (info.IsAssembly) {
			str.Append("internal ");
		}
		
		if (info.IsSpecialName) {
			str.Append("hidden ");
		}
		
		
		
		if (info.IsStatic) {
			str.Append("static ");
		} else {
			
		}
		
		str.Append(info.FieldType.ShortName() + " " + info.Name + ";");
		
		
		
		return str.ToString();
	}
	
	
	
}
