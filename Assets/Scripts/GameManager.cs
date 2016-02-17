using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }
	private GameObject tileClicked;
	public GameObject[][] tiles;

	void Awake () {
		if (Instance != null && Instance != this) {
			Destroy (gameObject);
		}

		Instance = this;
		
		GameObject[] gTiles = GameObject.FindGameObjectsWithTag("Tile");
		
		foreach (GameObject gTile in gTiles) {
			Tile t = gTile.GetComponent<Tile>();
			Debug.Log ("Found tile at " + t.x + ", " + t.y);
		}

		DontDestroyOnLoad (gameObject);
	}

	public GameObject getTileClicked() {
		return tileClicked;
	}

	public void setTileClicked(GameObject tile) {
		tileClicked = tile;
	}
}
