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
	private List<ShortestPathStep> shortestPath;

	
	// Use this for initialization
	void Start () {
		// not sure if we need this
		rend = GetComponent<Renderer> ();
		openSteps = new List<ShortestPathStep>();
		closedSteps = new List<ShortestPathStep>();
		shortestPath = new List<ShortestPathStep>();
	}

	void OnMouseEnter(){
		trans = GetComponent<Transform> ();

		border = GameObject.Find ("border");

		border.GetComponent<Transform>().position = new Vector3 (trans.position.x, trans.position.y, trans.position.z - 0.1f);


		// We only want to show the path on mouse hover. And only once. So reset this bool when mouse exits.
		if (!pathSet) {
			// Get the player's tile so we can calculate shortest path to mouse tile.
			var pTile = GameObject.Find ("AnimatedSprite").GetComponent<Tile>();
			var mTile = GetComponent<Tile>();
			
			InsertInOpenSteps(new ShortestPathStep(pTile));
			int i = 0;
			do {
				i++;
				ShortestPathStep currentStep = openSteps[0];
				
				closedSteps.Add(currentStep);
				
				openSteps.RemoveAt(0);
				
				if (currentStep.position == mTile) {
					do {
						if (currentStep.parent != null) {
							shortestPath.Add (currentStep);
						}
						currentStep = currentStep.parent;
					} while (currentStep != null);
					
					foreach (ShortestPathStep sps in shortestPath) {
						var gObj = sps.position.GetComponent<Transform>();
						//storing the instantiate object as GameObject in clonedBorder and giving it a unique tag
						var clonedBorder = Instantiate (border, new Vector3(gObj.position.x, gObj.position.y, gObj.position.z - 0.1f), Quaternion.identity) as GameObject;
						clonedBorder.tag = "clone";
						clonedBorder.GetComponent<Renderer>().enabled = true;
					}
					
					openSteps.Clear();
					closedSteps.Clear();
					shortestPath.Clear();
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

			if (shortestPath.Count == 0) {
				Debug.Log("Could not find a path.");
			}

			openSteps.Clear();
			closedSteps.Clear();
			shortestPath.Clear();
			
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
		border.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseExit(){
		removeIndicators();
		pathSet = false;
	}
	
	void OnMouseDown() {
		var tile = GetComponent<Tile>();
		if (tile != null) {
			GameManager.Instance.setTileClicked (gameObject);
			removeIndicators();
		}
	}
	
	void InsertInOpenSteps(ShortestPathStep step) {
		int stepFScore = step.Fscore();
		int count = openSteps.Count;
		int i = 0;

		for (; i < count; i++) {
			if (stepFScore < openSteps[i].Fscore()) {
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
	
	void removeIndicators() {
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
