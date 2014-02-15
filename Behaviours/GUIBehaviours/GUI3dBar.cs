using UnityEngine;
using System.Collections;

public class GUI3dBar : MonoBehaviour {
	
	public float value;
	
	public Color frontColor = new Color(1, 0, 1, 1);
	public Color backColor = new Color(.6f, 0, .6f, 1);
	
	public Transform barFront;
	public Transform barBack;
	public bool drainToLeft = true;
	
	Vector3 barSize;
	
	
	float width;
	
	void Start() {
		if (barBack == null) { barBack = transform.Find("BarBack"); }	
		if (barFront == null) { barFront = transform.Find("BarFront"); }
		if (barBack == null) {
			barBack = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			barBack.transform.position = transform.position;
		}
		
		if (barFront == null) {
			barFront = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			barFront.transform.position = transform.position - Vector3.forward;
		}
		
		if (barBack.parent != transform) { barBack.parent = transform; }
		if (barFront.parent != transform) { barFront.parent = transform; }
		
		//width = barBack.localScale.x;
		barSize = barFront.localScale;
		SetColors();
		
	}
	
	
	void Update() {
		value = value.Clamp01();
		Vector3 pos = -Vector3.forward;
		Vector3 size = barSize;
		if (drainToLeft) {
			pos.x = -barSize.x * .5f;
			
			pos.x += value * barSize.x * .5f;
		} else {
			pos.x = barSize.x * .5f;
			pos.x -= value * barSize.x * .5f;
		}
		
		size.x = value * barSize.x;
		barFront.localPosition = pos;
		barFront.localScale = size;
	}
	
	void SetColors() {
		barBack.renderer.material.color = backColor;
		barFront.renderer.material.color = frontColor;
	}
	
	public void SetColors(Color f, Color b) {
		backColor = b;
		frontColor = f;
		SetColors();
	}
	
}
