using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public Tile currentTile;
	public Animator animator;
	private bool walk = false;
	private bool flipped = false;

	private enum Direction {
		North,
		South,
		East,
		West
	}

	private Direction direction;

	// Use this for initialization
	void Start () {
		currentTile = GetComponent<Tile>();
		animator = GetComponent<Animator> ();
		direction = Direction.West;
	}

	IEnumerator Walk() {
		if (GameManager.Instance.shortestPath.Count > 0) {
			walk = true;

			Tile moveTile = GameManager.Instance.shortestPath [GameManager.Instance.shortestPath.Count - 1].position;
			GetComponent<Tile> ().elevation = moveTile.elevation + 1;

			var newY = (moveTile.x + moveTile.y) / -4.0f;
			var newZ = newY;
			if (GetComponent<Tile>().elevation != 0) {
				newY = newY + GetComponent<Tile>().elevation / 4.0f;
				newZ = newZ - GetComponent<Tile>().elevation / 8.0f;
			}

			var newPos = new Vector3 (moveTile.transform.position.x, newY, newZ);

			transform.position = Vector3.MoveTowards (transform.position,
				newPos, speed * Time.deltaTime);

			if (moveTile.elevation > 0) {
				animator.Play ("PlayerJumpingWest");
			}

			//Debug.Log (GetComponent<Tile> ().x + ", " + GetComponent<Tile> ().y + " -> " + moveTile.x + ", " + moveTile.y);
			if (GetComponent<Tile>().x > moveTile.x) {
				if (direction != Direction.West) {
					if (flipped) {
						Vector3 theScale = transform.localScale;
						theScale.x *= -1;
						transform.localScale = theScale;
						flipped = false;
					}
					direction = Direction.West;
				}
				animator.Play ("PlayerWalkingWest");
			} else if (GetComponent<Tile>().x < moveTile.x) {
				if (direction != Direction.East) {
					if (!flipped) {
						Vector3 theScale = transform.localScale;
						theScale.x *= -1;
						transform.localScale = theScale;
						flipped = true;
					}
					direction = Direction.East;
				}
				animator.Play ("PlayerWalkingSouth");
			} else if (GetComponent<Tile>().y < moveTile.y) {
				if (direction != Direction.South) {
					if (flipped) {
						Vector3 theScale = transform.localScale;
						theScale.x *= -1;
						transform.localScale = theScale;
						flipped = false;
					}
					direction = Direction.South;
				}
				animator.Play ("PlayerWalkingSouth");
			} else if (GetComponent<Tile>().y > moveTile.y) {
				if (direction != Direction.North) {
					if (!flipped) {
						Vector3 theScale = transform.localScale;
						theScale.x *= -1;
						transform.localScale = theScale;
						flipped = true;
					}
					direction = Direction.North;
				}
				animator.Play("PlayerWalkingWest");
			}

			if (transform.position != newPos) {
				yield return null;
			} else {
				currentTile = moveTile;
				GetComponent<Tile> ().x = moveTile.x;
				GetComponent<Tile> ().y = moveTile.y;
				GameManager.Instance.shortestPath.RemoveAt (GameManager.Instance.shortestPath.Count - 1);
			}
		} else {
			GameManager.Instance.playerState = GameManager.PlayerState.PLAYER_IDLE;
		}

		yield return true;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("State: " + GameManager.Instance.playerState);
		//Debug.Log ("Size: " + GameManager.Instance.shortestPath.Count);
		// Start walking.
		if (GameManager.Instance.shortestPath.Count > 0
		    && GameManager.Instance.playerState == GameManager.PlayerState.PLAYER_WALKING) {
			StartCoroutine (Walk ());
		} else {
			GameManager.Instance.playerState = GameManager.PlayerState.PLAYER_IDLE;
			Vector3 theScale = transform.localScale;
			switch (direction) {
				case Direction.East:
					if (!flipped) {
						theScale.x *= -1;
						transform.localScale = theScale;
						flipped = true;
					}
					animator.Play ("PlayerIdleSouth");
					break;
				case Direction.North:
					if (!flipped) {
						theScale.x *= -1;
						transform.localScale = theScale;
						flipped = true;
					}
					animator.Play ("PlayerIdleWest");
					break;
				case Direction.South:
					animator.Play ("PlayerIdleSouth");
					break;
				case Direction.West:
					animator.Play ("PlayerIdleWest");
					break;
				default:
					animator.Play ("PlayerIdleWest");
					break;
			}
			
		}
	}
}
