using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public Tile currentTile;

	// Use this for initialization
	void Start () {
		var tile = GetComponent<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject tc = GameManager.Instance.getTileClicked ();
		if (tc != null) {
			GetComponent<Animator>().SetInteger("PlayerAction", 1);
			var newZ = tc.transform.position.y - 1 / 8.0f;
			var newY = tc.transform.position.y + 1 / 4.0f;
			Debug.Log ("Tile was clicked. Moving to " + tc.transform.position.x + ", " + tc.transform.position.y);
			var newPos = new Vector3 (tc.transform.position.x, newY, newZ);
			transform.position = Vector3.MoveTowards(transform.position, 
				newPos, speed * Time.deltaTime);
			if (transform.position == newPos) {
				GetComponent<Animator>().SetInteger("PlayerAction", 0);
				GameManager.Instance.setTileClicked (null);
			}
		}
	}
}
