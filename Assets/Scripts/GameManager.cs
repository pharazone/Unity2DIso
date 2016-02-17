using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }
	private GameObject tileClicked;
	public List<Tile> tiles;

	void Awake () {
		if (Instance != null && Instance != this) {
			Destroy (gameObject);
		}

		Instance = this;
		
		GameObject[] gTiles = GameObject.FindGameObjectsWithTag("Tile");
		
		foreach (GameObject gTile in gTiles) {
			Tile t = gTile.GetComponent<Tile>();
			tiles.Add (t);
			Debug.Log ("Found tile at " + t.x + ", " + t.y);
		}

		DontDestroyOnLoad (gameObject);
	}
	
	public Tile getTileAt(int x, int y) {
		foreach (Tile t in tiles) {
			if (t.x == x && t.y == y) {
				return t;
			}
		}
		
		return null;
	}

	public GameObject getTileClicked() {
		return tileClicked;
	}

	public void setTileClicked(GameObject tile) {
		tileClicked = tile;
	}
}
