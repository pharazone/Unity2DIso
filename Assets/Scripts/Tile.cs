using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Tile : MonoBehaviour {

	public int x, y, elevation;
	GameObject border;

	// Use this for initialization
	void Start () {
		GameObject.Find("Border");
	}

	// Update is called once per frame
	void Update () {
		if (Application.isEditor && !Application.isPlaying) {
			var newX = 0.0f;
			var newY = 0.0f;
			var newZ = 0.0f;
			
			newX = (x - y) / 2.0f;
			newY = (x + y) / -4.0f;
			newZ = newY;
			if (elevation != 0) {
				newY = newY + elevation / 4.0f;
				newZ = newZ - elevation / 8.0f;
			}
			
			
			transform.position = new Vector3 (newX, newY, newZ);
		}
	}
	
	public override bool Equals(System.Object obj) {
		if (obj == null) {
			return false;
		}
		
		Tile t = obj as Tile;
		
		if ((System.Object)t == null) {
			return false;
		}
		
		return t.x == x && t.y == y;
	}
	
	public bool Equals(Tile t) {
		if ((object)t == null) {
			return false;
		}
		
		return t.x == x && t.y == y;
	}
}
