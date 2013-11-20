using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Res {

	public static Texture2D Tex2DFromBytes(string path) {
		Texture2D newtex = new Texture2D(4,4);
		TextAsset temp = Resources.Load(path, typeof(TextAsset)) as TextAsset;
		if(temp==null || !newtex.LoadImage(temp.bytes)) {
			newtex=null;
		}
		return newtex;
	}
	
}