using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject player;
	private TerrainManager terrain; // game terrain
	private Room currentRoom; // room that the player is currently in

	void Start() {
		player = GameObject.FindWithTag("Player");
		terrain = GameObject.Find("Terrain").GetComponent<TerrainManager>();
	}

	void FixedUpdate() {
		UpdatePlayerRoom();
	}

	// Find the room that the player is in, if any, and perform actions upon room entry.
	private void UpdatePlayerRoom() {
		var playerGridPosition = LayoutGrid.FromWorldPosition(player.transform.position);
		currentRoom = terrain.FindRoomAtPosition(playerGridPosition);
		if (currentRoom != null) {
			if (!currentRoom.hasBeenEntered) {
				currentRoom.hasBeenEntered = true;

				// action triggered upon room entry
				if (currentRoom.type == RoomType.Enemy) {
					SpawnEnemies();
				} else if (currentRoom.type == RoomType.Boss) {
					StartBossFight();
				}
			}
		}
	}

	// Spawn enemies in the current room.
	private void SpawnEnemies() {
		// TODO: spawn room enemies
	}

	// Start the boss fight in the current room.
	private void StartBossFight() {
		terrain.BlockRoomEntrances(currentRoom);
		// TODO: initiate boss fight
	}

	// Signal the game controller that the boss fight has ended.
	private void EndBossFight() {
		terrain.ClearRoomEntrances(currentRoom);
		// TODO: add trap door/end game
	}
}
