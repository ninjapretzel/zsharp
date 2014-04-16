using UnityEngine;
using System.Collections;

public class GUIButtonAction : MonoBehaviour {

	public Transform realParent { get { return transform.parent.parent; } set { transform.parent.parent = value; } }
	public string text { get { if(textmesh==null) { return "null"; } return textmesh.text; } set { if(textmesh != null) { textmesh.text = value; } } }
	
	[System.NonSerialized] public TextMesh textmesh = null;
	
	void OnMouseUpAsButton() {
		SendMessage("Action", SendMessageOptions.DontRequireReceiver);
		
	}
	
	public static T Factory<T>(Vector3 position, Font font = null, string label = "blank", Material mat = null) where T : GUIButtonAction { return Factory<T>(position, new Vector3(4.0f, 1.0f, 0.00001f), font, label, mat); }
	public static T Factory<T>(Vector3 position, Vector3 scale, Font font = null, string label = "blank", Material mat = null) where T : GUIButtonAction {
		if(font == null) {
			font = Resources.Load("Courier New", typeof(Font)) as Font;
		}
		
		GameObject button = new GameObject("Button");
		GameObject textObj = new GameObject("Text");
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Transform t = cube.transform;
		
		button.layer = 8;
		textObj.layer = 8;
		cube.layer = 8;
		
		
		textObj.transform.parent = button.transform;
		t.parent = button.transform;
		
		textObj.transform.localPosition = Vector3.zero;
		
		
		if (mat != null) {
			cube.renderer.sharedMaterial = mat;
		}
		
		
		MeshRenderer mr = textObj.AddComponent<MeshRenderer>();
		TextMesh tm = textObj.AddComponent<TextMesh>();
		
		
		button.transform.localPosition = position;
		t.localScale = scale;
		
		tm.transform.localScale *= .15f;
		tm.transform.localPosition = Vector3.forward * -.1f;
		tm.font = font;
		tm.anchor = TextAnchor.MiddleCenter;
		tm.alignment = TextAlignment.Center;
		mr.material = font.material;
		tm.text = label;
		cube.transform.localPosition = Vector3.zero;
		
		T comp = cube.AddComponent<T>();
		comp.textmesh = tm;
		return comp;
	}
}
