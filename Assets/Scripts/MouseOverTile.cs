using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseOverTile : MonoBehaviour {

	private Renderer rend;
	private Transform trans;
	private GameObject border;
	private bool pathSet = false;
	private List<ShortestPathStep> openSteps;
	private List<ShortestPathStep> closedSteps;

	
	// Use this for initialization
	void Start () {
		// not sure if we need this
		rend = GetComponent<Renderer> ();
		border = GameObject.Find ("border");
		openSteps = new List<ShortestPathStep>();
		closedSteps = new List<ShortestPathStep>();
	}

	void OnMouseEnter(){
		if (GameManager.Instance.playerState == GameManager.PlayerState.PLAYER_IDLE) {
			
			trans = GetComponent<Transform> ();

			//border.GetComponent<Transform> ().position = new Vector3 (trans.position.x, trans.position.y, trans.position.z - 0.1f);

			openSteps.Clear ();
			closedSteps.Clear ();
			GameManager.Instance.shortestPath.Clear ();
			ShowMoveTiles();
			ShowShortestPath (GameObject.Find ("AnimatedSprite").GetComponent<Unit>().moveDistance);
		
			//border.GetComponent<Renderer> ().enabled = true;
		}
	}

	void OnMouseExit(){
		RemoveIndicators();
		pathSet = false;
	}
	
	void OnMouseDown() {
		var tile = GetComponent<Tile>();
		if (tile != null) {
			GameManager.Instance.setTileClicked (gameObject);
			RemoveIndicators();
		}
		GameManager.Instance.playerState = GameManager.PlayerState.PLAYER_WALKING;
	}
	
	void ShowMoveTiles() {
		int range = GameObject.Find ("AnimatedSprite").GetComponent<Unit>().moveDistance;
		
		int xTile = GameObject.Find ("AnimatedSprite").GetComponent<PlayerController>().currentTile.x;
		int yTile = GameObject.Find ("AnimatedSprite").GetComponent<PlayerController>().currentTile.y;
		
		yTile += range + 1;
		int offset = 1;
		int z = 1;
		bool shrinking = false;
		for ( int i = 0; i <= range * 2; i++ ) {
			xTile -= offset;
			yTile--;
			for ( int j = 0; j < z; j++ ) {
				xTile++;
				
				Tile t = GameManager.Instance.getTileAt(xTile, yTile);
				if (t != null) {
					var clonedBorder = Instantiate (border, new Vector3(t.GetComponent<Transform>().position.x, t.GetComponent<Transform>().position.y, t.GetComponent<Transform>().position.z - 0.1f), Quaternion.identity) as GameObject;
					clonedBorder.tag = "clone";
					clonedBorder.GetComponent<Renderer>().enabled = true;
				}
				
			}
			
			xTile = GameObject.Find ("AnimatedSprite").GetComponent<PlayerController>().currentTile.x;
			
			if ( offset > range || shrinking ) {
				shrinking = true;
				offset--;
				z -= 2;
			} else {
				offset++;
				z += 2;
			}
		}
	}
	
	void ShowShortestPath(int limit) {
		// We only want to show the path on mouse hover. And only once. So reset this bool when mouse exits.
		if (!pathSet) {
			// Get the player's tile so we can calculate shortest path to mouse tile.
			var pTile = GameObject.Find ("AnimatedSprite").GetComponent<PlayerController>().currentTile;
			var mTile = GetComponent<Tile>();
			
			if (pTile == mTile) {
				pathSet = true;
				return;
			}
			
			InsertInOpenSteps(new ShortestPathStep(pTile));
			int i = 0;
			do {
				i++;
				ShortestPathStep currentStep = openSteps[0];
				
				closedSteps.Add(currentStep);
				
				openSteps.RemoveAt(0);
				
				if (currentStep.position == mTile || i > limit) {
					do {
						if (currentStep.parent != null) {
							GameManager.Instance.shortestPath.Add (currentStep);
						}
						currentStep = currentStep.parent;
					} while (currentStep != null);
					
					foreach (ShortestPathStep sps in GameManager.Instance.shortestPath) {
						Debug.Log ("SPS Count: " + GameManager.Instance.shortestPath.Count);
						var gObj = sps.position.GetComponent<Transform>();
						//storing the instantiate object as GameObject in clonedBorder and giving it a unique tag
						var clonedBorder = Instantiate (border, new Vector3(gObj.position.x, gObj.position.y, gObj.position.z - 0.1f), Quaternion.identity) as GameObject;
						clonedBorder.tag = "clone";
						clonedBorder.GetComponent<Renderer>().enabled = true;
					}
					
					openSteps.Clear();
					closedSteps.Clear();
					//shortestPath.Clear();
					return;
				}
				
				List<Tile> adjTiles = GameManager.Instance.GetAdjacentTiles(currentStep.position);
				
				foreach (Tile t in adjTiles) {
					
					ShortestPathStep step = new ShortestPathStep(t);
					
					bool inClosed = false;
					foreach (ShortestPathStep cs in closedSteps) {
						if (cs.position == step.position) {
							inClosed = true;
						}
					}
					if (inClosed) {
						continue;
					}
					
					int moveCost = CostToMove(currentStep, step);
					
					bool inOpen = false;
					foreach (ShortestPathStep os in openSteps) {
						if (os.position == step.position) {
							inOpen = true;
						}
					}
					
					if (!inOpen) {
						step.parent = currentStep;
						step.gScore = currentStep.gScore + moveCost;
						step.hScore = ComputeHScore(step.position, mTile);
						
						InsertInOpenSteps(step);
					}
				}
			} while (openSteps.Count > 0);
			
			if (GameManager.Instance.shortestPath.Count == 0) {
				Debug.Log("Could not find a path.");
			}
			
			openSteps.Clear();
			closedSteps.Clear();

			/*
			foreach (Tile t in adjTiles) {
				var gObj = t.GetComponent<Transform>();
				//storing the instantiate object as GameObject in clonedBorder and giving it a unique tag
				var clonedBorder = Instantiate (border, new Vector3(gObj.position.x, gObj.position.y, gObj.position.z - 0.1f), Quaternion.identity) as GameObject;
				clonedBorder.tag = "clone";
				clonedBorder.GetComponent<Renderer>().enabled = true;
			}*/
			pathSet = true;
		}
	}
	
	void InsertInOpenSteps(ShortestPathStep step) {
		int stepFScore = step.Fscore();
		int count = openSteps.Count;
		int i = 0;

		for (; i < count; i++) {
			if (stepFScore <= openSteps[i].Fscore()) {
				break;
			}
		}

		openSteps.Insert(i, step);
	}
	
	int CostToMove(ShortestPathStep a, ShortestPathStep b) {
		return 1;
	}
	
	int ComputeHScore(Tile f, Tile t) {
		return Mathf.Abs(t.x - f.x) + Mathf.Abs(t.y - f.y);
	}
	
	void RemoveIndicators() {
		//creating an array of all the cloned borders by finding the unique tag
		GameObject[] borders = GameObject.FindGameObjectsWithTag("clone");
		//disabling render and destroying them (not sure why I have to disable render but it doesnt work otherwise)
		foreach (GameObject b in borders) {
			b.GetComponent<Transform>().position = new Vector3 (0f,-1f,50f);
			b.GetComponent<Renderer>().enabled = false;
			Destroy (b);
		}
		//main border object gets repositioned and rendering turned off
		border.GetComponent<Transform>().position = new Vector3 (0f,-1f,50f);
		border.GetComponent<Renderer>().enabled = false;
	}
	
	

	// Update is called once per frame
	void Update () {

	}
}
