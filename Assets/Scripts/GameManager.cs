using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }
	private GameObject tileClicked;

	void Awake () {
		if (Instance != null && Instance != this) {
			Destroy (gameObject);
		}

		Instance = this;

		DontDestroyOnLoad (gameObject);
	}

	public GameObject getTileClicked() {
		return tileClicked;
	}

	public void setTileClicked(GameObject tile) {
		tileClicked = tile;
	}
}
