using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseOverTile : MonoBehaviour {

	private List<ShortestPathStep> openSteps;
	private List<ShortestPathStep> closedSteps;
	private List<ShortestPathStep> shortestPath;

	
	// Use this for initialization
	void Start () {
		// not sure if we need this
		openSteps = new List<ShortestPathStep>();
		closedSteps = new List<ShortestPathStep>();
		shortestPath = new List<ShortestPathStep>();
	}

	void OnMouseEnter(){
		if (GameManager.Instance.playerState == GameManager.PlayerState.PLAYER_IDLE) {
			//border.GetComponent<Transform> ().position = new Vector3 (trans.position.x, trans.position.y, trans.position.z - 0.1f);

			openSteps.Clear ();
			closedSteps.Clear ();
			shortestPath.Clear ();
			GameManager.Instance.shortestPath.Clear ();

			GameObject animatedSprite = GameObject.Find ("AnimatedSprite");
			int moveDistance = animatedSprite.GetComponent<Unit>().moveDistance;
			Tile pTile = GameManager.Instance.getTileAt(animatedSprite.GetComponent<PlayerController>().currentTile.x,
			                                            animatedSprite.GetComponent<PlayerController>().currentTile.y);
			Tile mTile = GameManager.Instance.getTileAt(GetComponent<Tile>().x, GetComponent<Tile>().y);


			ShowMoveTiles(pTile, moveDistance);
			var tiles = GetShortestPath(pTile, mTile, moveDistance);

			if (tiles.Count > moveDistance) {
				tiles.RemoveRange (0, tiles.Count - moveDistance);
			}

			GameManager.Instance.shortestPath = tiles;

			foreach (Tile t in tiles) {
				t.pathBorder.enabled = true;
			}
		}
	}

	void OnMouseExit(){
		RemoveIndicators();
	}
	
	void OnMouseDown() {
		var tile = GetComponent<Tile>();
		if (tile != null) {
			GameManager.Instance.setTileClicked (gameObject);
			RemoveIndicators();
		}
		GameManager.Instance.playerState = GameManager.PlayerState.PLAYER_WALKING;
	}
	
	void ShowMoveTiles(Tile fromTile, int range) {
		int xTile = fromTile.x;
		int yTile = fromTile.y;
		
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
					var tiles = GetShortestPath(fromTile, t);

					if (tiles.Count > 0 && tiles.Count <= range) {
						t.moveBorder.enabled = true;
					}
				}
				
			}
			
			xTile = fromTile.x;
			
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
	
	public List<Tile> GetShortestPath(Tile from, Tile to, int limit = -1) {
		GameManager.Instance.shortestPath.Clear ();
		// We only want to show the path on mouse hover. And only once. So reset this bool when mouse exits.
		var tiles = new List<Tile>();

		if (from == to) {
			return tiles;
		}
		
		InsertInOpenSteps(new ShortestPathStep(from));
		int i = 0;
		do {
			i++;

			ShortestPathStep currentStep = openSteps[0];
			
			closedSteps.Add(currentStep);
			
			openSteps.RemoveAt(0);
			
			if (currentStep.position == to /*|| (limit != -1 && i > limit)*/) {
				do {
					if (currentStep.parent != null) {
						shortestPath.Add(currentStep);
					}
					currentStep = currentStep.parent;
				} while (currentStep != null);


				foreach (ShortestPathStep sps in shortestPath) {
					tiles.Add(sps.position);
				}
				
				openSteps.Clear();
				closedSteps.Clear();
				shortestPath.Clear();
				return tiles;
			}
			
			List<Tile> adjTiles = GameManager.Instance.GetAdjacentTiles(currentStep.position);
			
			foreach (Tile t in adjTiles) {
				ShortestPathStep step = new ShortestPathStep(t);
				
				bool inClosed = false;
				foreach (ShortestPathStep cs in closedSteps) {
					if (cs.position == step.position) {
						inClosed = true;
						break;
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
					step.hScore = ComputeHScore(step.position, to);

					InsertInOpenSteps(step);
				}
			}
		} while (openSteps.Count > 0);
		
		if (shortestPath.Count == 0) {
			//Debug.Log("Could not find a path.");
		}
		
		openSteps.Clear();
		closedSteps.Clear();
		shortestPath.Clear ();

		return tiles;
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
		GameObject[] borders = GameObject.FindGameObjectsWithTag("Indicator");
		//disabling render and destroying them (not sure why I have to disable render but it doesnt work otherwise)
		foreach (GameObject b in borders) {
			b.GetComponent<SpriteRenderer>().enabled = false;
		}

	}
	
	

	// Update is called once per frame
	void Update () {

	}
}
