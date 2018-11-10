using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	public float openAnimationDuration;
	
	private bool open = false;
	private Animator animator;
	private GameObject powerUp;
	private GameController game;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		powerUp = RandomPicker.Pick(game.powerUps, true);
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider collider) {
		if (!open && collider.gameObject.tag == "Player") {
			var spawnLocation = powerUp.transform.position + transform.position;
			var instance = Instantiate(powerUp, spawnLocation, powerUp.transform.rotation);
			instance.transform.parent = transform;
			animator.CrossFadeInFixedTime("Open", openAnimationDuration);
			open = true;
		}
	}
}