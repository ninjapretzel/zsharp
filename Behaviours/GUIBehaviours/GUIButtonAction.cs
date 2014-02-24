using UnityEngine;
using System.Collections;

public class GUIButtonAction : MonoBehaviour {

	public Transform realParent { get { return transform.parent.parent; } set { transform.parent.parent = value; } }
	
	
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
		t.localScale = scale;
		
		button.layer = 8;
		textObj.layer = 8;
		cube.layer = 8;
		
		button.transform.position = position;
		textObj.transform.parent = button.transform;
		t.parent = button.transform;
		
		cube.transform.localPosition = Vector3.zero;
		textObj.transform.localPosition = Vector3.zero;
		
		MeshRenderer mr = textObj.AddComponent<MeshRenderer>();
		TextMesh tm = textObj.AddComponent<TextMesh>();
		if (mat != null) {
			cube.renderer.sharedMaterial = mat;
		}
			
		mr.material = font.material;
		
		tm.transform.localScale *= .15f;
		tm.transform.localPosition = Vector3.forward * -.1f;
		tm.font = font;
		tm.anchor = TextAnchor.MiddleCenter;
		tm.alignment = TextAlignment.Center;
		tm.text = label;
		
		
		return cube.AddComponent<T>();
	}
}
