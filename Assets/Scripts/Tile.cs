using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Tile : MonoBehaviour {

	public int x, y, elevation;

	// Use this for initialization
	void Start () {
		
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
}
