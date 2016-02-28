using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public enum PlayerState {
		PLAYER_WALKING,
		PLAYER_IDLE
	}

	public enum GameState {
		PLAYER_TURN,
		ENEMY_TURN
	}

	public PlayerState playerState;
	public GameState gameState;
	public static GameManager Instance { get; private set; }
	private GameObject tileClicked;
	public List<Tile> tiles;
	public List<Tile> shortestPath;
	public Tile playerTile;
	private Text playerStateText;
	private Text gameStateText;

	void Awake () {
		if (Instance != null && Instance != this) {
			Destroy (gameObject);
		}

		Instance = this;
		
		GameObject[] gTiles = GameObject.FindGameObjectsWithTag("Tile");
		
		foreach (GameObject gTile in gTiles) {
			Tile t = gTile.GetComponent<Tile>();
			tiles.Add (t);
		}

		playerState = PlayerState.PLAYER_IDLE;
		gameState = GameState.PLAYER_TURN;
		shortestPath = new List<Tile>();
		playerStateText = GameObject.Find ("PlayerState").GetComponent<Text>();
		gameStateText = GameObject.Find ("GameState").GetComponent<Text> ();
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
	
	public List<Tile> GetAdjacentTiles(Tile t) {
		List<Tile> adjTiles = new List<Tile>();
		
		Tile top = getTileAt(t.x, t.y + 1);
		Tile bot = getTileAt(t.x, t.y - 1);
		Tile left = getTileAt(t.x + 1, t.y);
		Tile right = getTileAt(t.x - 1, t.y);
		
		if (top != null && top.obstacle == null) {
			//Debug.Log ("Found adjacent tile at " + top.x + ", " + top.y);
			adjTiles.Add(top);
		}
		
		if (bot != null && bot.obstacle == null) {
			//Debug.Log ("Foud adjacent tile at " + bot.x + ", " + bot.y);
			adjTiles.Add (bot);
		}
		
		if (left != null && left.obstacle == null) {
			//Debug.Log ("Found adjacent tile at " + left.x + ", " + left.y);
			adjTiles.Add (left);
		}
		
		if (right != null && right.obstacle == null) {
			//Debug.Log ("Found adjacent tile at " + right.x + ", " + right.y);
			adjTiles.Add(right);
		}
		
		return adjTiles;
	}
	
	

	public GameObject getTileClicked() {
		return tileClicked;
	}

	public void setTileClicked(GameObject tile) {
		tileClicked = tile;
	}

	void Update() {
		playerStateText.text = "Player State: " + playerState;
		gameStateText.text = "Game State: " + gameState;
	}
}
