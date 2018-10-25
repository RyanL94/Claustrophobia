using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour {

	public float delay;

	private GameController game;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			StartCoroutine(PassThrough());
		}
	}

	// Move to a new floor when passed through by the player.
	private IEnumerator PassThrough() {
		yield return new WaitForSeconds(delay);
		game.CreateNewFloor();
	}
}
