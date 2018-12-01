using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drop : MonoBehaviour {

	public GameObject pickUpEffect;
	public AudioClip pickUpSound;
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
		AudioSource.PlayClipAtPoint(pickUpSound, transform.position, 0.15f);
	}
}
