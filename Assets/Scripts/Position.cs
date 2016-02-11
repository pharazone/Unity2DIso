using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Position : MonoBehaviour {

	public float x, y, elevation;
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Application.isEditor && !Application.isPlaying) {
			Debug.Log ("Should never be seen in game mode.");
			var newX = 0.0f;
			var newY = 0.0f;
			var newZ = 0.0f;
			
			newX = (x - y) / 2.0f;
			newY = (x + y) / -4.0f;
			newZ = newY;
			if (elevation != 0) {
				newY = newY + elevation / 4.0f;
			}
			
			
			transform.position = new Vector3 (newX, newY, newZ);
		}
	}
}
