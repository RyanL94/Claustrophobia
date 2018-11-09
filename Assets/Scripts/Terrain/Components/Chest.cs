﻿using System.Collections;
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
			if (collider.gameObject.name == "MeleeAttack") {
				animator.CrossFadeInFixedTime("Open", openAnimationDuration);
				interactedWith = true;
                gameObject.GetComponent<PowerUp>().open = true;


            }
		}
	}
}
