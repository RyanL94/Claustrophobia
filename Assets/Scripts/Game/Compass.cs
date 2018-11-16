using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Compass : MonoBehaviour {

	public List<RoomType> targetRooms;

	private GameController game;
	private Transform player;
	private List<Vector3> targets;
	private float updateSpeed;

	public void Initialize() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		player = game.player.transform;
		updateSpeed = game.hud.updateSpeed;
		targets = new List<Vector3>();
		foreach (Room room in game.terrain.rooms) {
			if (targetRooms.Contains(room.type)) {
				targets.Add(LayoutGrid.ToWorldPosition(room.centerPosition, true));
			}
		}
	}
	
	void FixedUpdate() {
		try {
			var distance = targets.Min(roomPosition => Vector3.Magnitude(roomPosition - player.position));
			var target = targets.Find(roomPosition => Vector3.Magnitude(roomPosition - player.position) == distance);
			var direction = target - player.position;
			var angle = Vector3.Angle(Vector3.forward, direction);
			
        	var progression = Time.deltaTime * updateSpeed;
			var newRotation = (direction.x > 0)? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, progression);
		} catch (Exception) {
			
		}
	}
}
