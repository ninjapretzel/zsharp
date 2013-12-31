using UnityEngine;
using System.Collections;

public class TextMeshNewlineReplacer : MonoBehaviour {
	public bool go;
	
	void OnDrawGizmos() {
		if (go) {
			TextMesh textMesh = GetComponent<TextMesh>();
			textMesh.text = textMesh.text.Replace("\\n", "\n");
		}
	}
	
	
}
