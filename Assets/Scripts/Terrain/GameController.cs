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
		CreateNewFloor();
	}

	void FixedUpdate() {
		UpdatePlayerRoom();
	}

	// Create a new floor.
	public void CreateNewFloor() {
		terrain.GenerateFloor();
		var centerPosition = terrain.floorConfiguration.FindCenterPosition();
		var worldCenterPosition = LayoutGrid.ToWorldPosition(centerPosition);
		var centerOffset = new Vector3(0.5f, 0.0f, 0.5f);
		player.transform.position = worldCenterPosition + centerOffset;
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
		Invoke("EndBossFight", 2.0f); // TODO: call function after boss death instead
	}

	// Signal the game controller that the boss fight has ended.
	private void EndBossFight() {
		var centerPosition = currentRoom.centerPosition;
		terrain.Place(terrain.terrainBlocks.passage, centerPosition);
	}
}
