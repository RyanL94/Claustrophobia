using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drop : MonoBehaviour {

	public GameObject pickUpEffect;
	protected GameController game;

	void Awake() {
        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			Take();
			Destroy(gameObject);
		}
	}

	protected virtual void Take() {
		Instantiate(pickUpEffect, game.player.transform);
	}
}
