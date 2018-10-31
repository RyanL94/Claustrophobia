using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour {

	private GameController game;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			PassThrough();
		}
	}

	// Move to a new floor when passed through by the player.
	private void PassThrough() {
		game.CreateNewFloor();
	}
}
