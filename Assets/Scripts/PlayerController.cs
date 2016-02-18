using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public Tile currentTile;

	// Use this for initialization
	void Start () {
		var tileObj = GetComponent<Tile>();
		currentTile = GameManager.Instance.getTileAt(tileObj.x, tileObj.y);
	}
	
	// Update is called once per frame
	void Update () {
		GameObject tc = GameManager.Instance.getTileClicked ();
		if (tc != null) {
			GetComponent<Animator>().SetInteger("PlayerAction", 1);
			GetComponent<Tile>().elevation = tc.GetComponent<Tile>().elevation + 1;
			//var newX = (tc.GetComponent<Tile>().x - tc.GetComponent<Tile>().y) / 2.0f;
			var newY = (tc.GetComponent<Tile>().x + tc.GetComponent<Tile>().y) / -4.0f;
			var newZ = newY;
			if (GetComponent<Tile>().elevation != 0) {
				newY = newY + GetComponent<Tile>().elevation / 4.0f;
				newZ = newZ - GetComponent<Tile>().elevation / 8.0f;
			}
			//Debug.Log ("Tile was clicked. Moving to " + tc.transform.position.x + ", " + tc.transform.position.y);
			var newPos = new Vector3 (tc.transform.position.x, newY, newZ);
			transform.position = Vector3.MoveTowards(transform.position, 
				newPos, speed * Time.deltaTime);
			if (transform.position == newPos) {
				GetComponent<Animator>().SetInteger("PlayerAction", 0);
				GetComponent<Tile>().x = tc.GetComponent<Tile>().x;
				GetComponent<Tile>().y = tc.GetComponent<Tile>().y;
				currentTile = GameManager.Instance.getTileAt(tc.GetComponent<Tile>().x, tc.GetComponent<Tile>().y);
				GameManager.Instance.setTileClicked (null);
			}
		}
	}
}
