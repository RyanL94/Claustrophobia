using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	public float openAnimationDuration;
	
	private bool interactedWith = false;
	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider collider) {
		if (!interactedWith) {
			if (collider.gameObject.tag == "Player") {
				animator.CrossFadeInFixedTime("Open", openAnimationDuration);
				interactedWith = true;

				var player = collider.gameObject;
				// TODO: give the player an item
			}
		}
	}
}
