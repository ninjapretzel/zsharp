using UnityEngine;
using System.Collections;

public class SimulatedControlStick : MonoBehaviour {
	public Vector2 position;
	public float size = .125f;
	public float paddingRatio = .0555556f;
	public int padding = 40;
	public float overzoneRatio = .222222f;
	public int overzone = 160;
	
	public Vector2 pixelPosition;
	public Vector2 pixelCenter;
	
	public float pixelSize;
	
	public bool tracking = true;
	
	public bool lockXaxis = false;
	public bool lockYaxis = false;
	
	public bool invertXout = false;
	public bool invertYout = false;
	
	public Texture2D mainGraphic;
	public Texture2D backGraphic;
	public Texture2D perimGraphic;
	
	public Color mainColor = new Color(1, 1, 1, .8f);
	public Color backColor = new Color(1, 1, 1, .3f);
	public Color perimColor = new Color(1, 1, 1, .5f);
	
	public Vector2 value {
		get { return val; }
	}
	private Vector2 val;
	
	void OnGUI() {
		overzone = (int) (Screen.height * overzoneRatio);
		padding = (int) (Screen.height * paddingRatio);
		
		pixelSize = size * Screen.height;
		pixelCenter = new Vector2(position.x * Screen.width, position.y * Screen.height);
		pixelPosition = pixelCenter - (Vector2.one * pixelSize/2.0f);
		
		Rect insideBrush = new Rect(pixelPosition.x, pixelPosition.y, pixelSize, pixelSize);
		Rect outsideBrush = insideBrush.Pad(padding/2.0f);
		Rect perimBrush = outsideBrush.Pad(overzone/2.0f);
		
		GUI.color = perimColor;
		GUI.DrawTexture(perimBrush, perimGraphic);
		
		GUI.color = backColor;
		GUI.DrawTexture(outsideBrush, backGraphic);
		
		Vector2 difference;
		Vector2 realTouchPosition;
		Rect tempBrush;
		bool hasHadGoodTouch = false;
		
		foreach (Touch t in Input.touches) {
			realTouchPosition = t.position;
			realTouchPosition.y = Screen.height - realTouchPosition.y;
			
			difference = pixelCenter - realTouchPosition;
			if (difference.magnitude < pixelSize + overzone) {
				if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) { continue; }
				
				hasHadGoodTouch = true;
				val = -difference.normalized * Mathf.Min(1, (difference.magnitude / pixelSize));
				if (lockXaxis) { val.x = 0; }
				if (lockYaxis) { val.y = 0; }
				val.Normalize();
				
				break;
			}
			
			if (!hasHadGoodTouch) { val = Vector2.zero; }
			insideBrush.x += value.x * pixelSize;
			insideBrush.y += value.y * pixelSize;
			
			if (invertXout) { val.x *= -1; }
			if (invertYout) { val.y *= -1; }
			
			GUI.color = mainColor;
			GUI.DrawTexture(insideBrush, mainGraphic);
			
		}
		
		
		
	}
	
}




























