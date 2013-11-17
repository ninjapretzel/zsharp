using UnityEngine;
using System.Collections;


public enum Cardinal { Up,Left,Down,Right }

public enum UpdateType {
	Update,
	LateUpdate,
	FixedUpdate
}

public enum RandomType {
	Normal,
	Seeded,
	Perlin,
}

public enum CollisionAction {
	Enter,
	Stay,
	Exit,
}

public static class General {

	public static Cardinal Flipped(this Cardinal c) { return c.Flip(); }
	public static Cardinal Flip(this Cardinal c) { return (Cardinal)( ( (int)c + 2) % 4); }
	
	
	
}
