using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseOverTile : MonoBehaviour {

	private Renderer rend;
	private Transform trans;
	private GameObject border;
	private bool pathSet = false;
	private List<Tile> openTiles;
	private List<Tile> closedTiles;
	
	// Use this for initialization
	void Start () {

		rend = GetComponent<Renderer> ();

		//border.GetComponent<Renderer>().enabled = false;

	}

	void OnMouseEnter(){
		trans = GetComponent<Transform> ();

		border = GameObject.Find ("border");
		//Instantiate (border);

		border.GetComponent<Transform>().position = new Vector3 (trans.position.x, trans.position.y, trans.position.z - 0.1f);

		//rend.material.color = Color.red;
		border.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseOver() {
		// We only want to show the path on mouse hover. And only once. So reset this bool when mouse exits.
		if (!pathSet) {
			// Get the player's tile so we can calculate shortest path to mouse tile.
			var pTile = GameObject.Find ("AnimatedSprite").GetComponent<Tile>();
			var mTile = GetComponent<Tile>();
			Debug.Log ("Player is on tile " + pTile.x + ", " + pTile.y);
			Debug.Log ("Mouse is on tile " + mTile.x + ", " + mTile.y);
			List<Tile> adjTiles = GetAdjacentTiles(pTile);
			pathSet = true;
		}
		border.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseExit(){
		//rend.material.color = Color.white;
		border.GetComponent<Transform>().position = new Vector3 (0f,-1f,50f);
		border.GetComponent<Renderer>().enabled = false;
		pathSet = false;
		//borderRend.enabled = false;
	}
	
	void OnMouseDown() {
		var tile = GetComponent<Tile>();
		if (tile != null) {
			GameManager.Instance.setTileClicked (gameObject);
			Debug.Log ("Clicked tile in position " + tile.x + ", " + tile.y);
		}
	}
	
	List<Tile> GetAdjacentTiles(Tile t) {
		List<Tile> adjTiles = new List<Tile>();
		
		return adjTiles;
	}

	// Update is called once per frame
	void Update () {

	}
}
