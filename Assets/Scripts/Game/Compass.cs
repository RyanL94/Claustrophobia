using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

	private GameController game;
	private Transform player;
	private Vector3 target;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	public void Initialize() {
		player = game.player.transform;
		foreach (Room room in game.terrain.rooms) {
			if (room.type == RoomType.Boss) {
				target = LayoutGrid.ToWorldPosition(room.centerPosition, true);
				break;
			}
		}
	}
	
	void FixedUpdate() {
		try {
			var direction = target - player.position;
			var angle = Vector3.Angle(Vector3.forward, direction);
			transform.rotation = Quaternion.Euler(0, 0, angle);
		} catch (Exception) {

		}
	}
}
