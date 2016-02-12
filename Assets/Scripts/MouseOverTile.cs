using UnityEngine;
using System.Collections;

public class MouseOverTile : MonoBehaviour {

	private Renderer rend;
	private Transform trans;
	private GameObject border;
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
		//rend.material.color -= new Color (0.1F, 0, 0) * Time.deltaTime;
		border.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseExit(){
		//rend.material.color = Color.white;
		border.GetComponent<Transform>().position = new Vector3 (0f,-1f,50f);
		border.GetComponent<Renderer>().enabled = false;
		//borderRend.enabled = false;
	}
	
	void OnMouseDown() {
		var posScript = GetComponent<Position>();
		if (posScript != null) {
			Debug.Log ("Clicked tile in position " + posScript.x + ", " + posScript.y);
			
			// Might not be the right spot to put this code, might be best suited in a PlayerController.
			GameObject player = GameObject.Find("AnimatedSprite");
			if (player != null) {
				Animator pAnimator = player.GetComponent<Animator>();
				if (pAnimator.GetInteger("PlayerAction") == 1) {
					pAnimator.SetInteger("PlayerAction", 0);
				} else {
					pAnimator.SetInteger("PlayerAction", 1);
				}
				
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
