using UnityEngine;
using System.Collections;

public class EnumF {

	// This is in the .NET Framework but hasn't made its way to the version of Mono used by Unity yet.
	public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct {
		if(!typeof(TEnum).IsEnum) { throw new System.ArgumentException("Type provided must be an Enum."); }
		try {
			result = (TEnum)System.Enum.Parse(typeof(TEnum), value);
			return true;
		} catch(System.ArgumentException) {
			result = default(TEnum);
			return false;
		} catch(System.OverflowException) {
			result = default(TEnum);
			return false;
		}
	}
}
