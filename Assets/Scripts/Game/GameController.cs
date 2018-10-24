using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public int numberOfFloors; // number of floors to traverse to win the game

	private GameObject player;
	private new CameraController camera;
	private TerrainManager terrain; // game terrain
	private Room currentRoom; // room that the player is currently in

	void Start() {
		player = GameObject.FindWithTag("Player");
		camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
		terrain = GameObject.Find("Terrain").GetComponent<TerrainManager>();
		CreateNewFloor();
	}

	void FixedUpdate() {
		UpdatePlayerRoom();
	}

	// Create a new floor.
	public void CreateNewFloor() {
		terrain.GenerateFloor();
		CenterPlayerOnFloor();
		--numberOfFloors;
	}

	// Center the player on the floor, putting him in the spawn room.
	//
	// The player is put at a certain elevation so that it looks like the player falls from the
	// previous floor.
	private void CenterPlayerOnFloor(float elevation=5.0f) {
		var centerPosition = terrain.floorConfiguration.FindCenterPosition();
		var worldCenterPosition = LayoutGrid.ToWorldPosition(centerPosition, true);
		player.transform.position = worldCenterPosition + Vector3.up * elevation;
		camera.CenterOnPlayer();
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
		if (numberOfFloors > 0) {
			terrain.Place(terrain.terrainBlocks.passage, centerPosition);
		}
	}
}
