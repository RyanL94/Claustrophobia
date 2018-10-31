﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wall block of a floor layout.
public class Block : MonoBehaviour {
	public bool breakable; // whether the block can be broken by the player
	public int durability; // amount of hits it takes to destory the block
	public GameObject ground; // ground under the block

	private GameController game;

	void Awake() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	void OnCollisionEnter(Collision collider) {
		CheckCollision(collider.gameObject);
	}

	void OnTriggerEnter(Collider collider) {
		CheckCollision(collider.gameObject);
	}

	private void CheckCollision(GameObject collider) {
		if (breakable && game.terrain.breakableByTag.Contains(collider.gameObject.tag)) {
			--durability;
			if (durability == 0) {
				var gridPosition = LayoutGrid.FromWorldPosition(transform.position);
				game.terrain.Break(gridPosition);
				var instance = Instantiate(
					game.terrain.breakEffect,
					LayoutGrid.ToWorldPosition(gridPosition, true),
					Quaternion.identity
				);
				instance.transform.localScale *= game.terrain.effectScale;
			}
		}
	}
}
