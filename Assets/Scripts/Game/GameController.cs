using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public new CameraController camera; // main game camera
	public HUD hud; // game hud to display health and ammo
	public Transition transition; // transition to use when genererate a new floor
	public EnemyManager enemyManager; // script which manages enemy spawns
	public TerrainManager terrain; // game terrain
	public PlayerController player; // player game object
	public int numberOfFloors; // number of floors to traverse to win the game
    public List<GameObject> powerUps; // items to find in chests
	public IntRange itemCost; // cost of items in the shop

	private Room currentRoom; // room that the player is currently in

	// Whether or not the player is in a room.
	public bool playerInRoom {
		get {
			return currentRoom != null;
		}
	}

	void Start() {
		CreateNewFloor();
	}

	void FixedUpdate() {
        if (player != null) {
            UpdatePlayerRoom();
        }
	}

	// Create a new floor.
	public void CreateNewFloor() {
		StartCoroutine(CreateNewFloorHelper());
	}

	public void OnPlayerDeath() {
		StartCoroutine(OnPlayerDeathHelper());
	}

	private IEnumerator CreateNewFloorHelper() {
		transition.FadeIn();
		yield return new WaitForSeconds(transition.duration);
		terrain.GenerateFloor();
		CenterPlayerOnFloor();
		--numberOfFloors;
		enemyManager.Initialize();
		transition.FadeOut();
	}

	private IEnumerator OnPlayerDeathHelper() {
		yield return new WaitForSeconds(3.0f);
		transition.FadeIn();
		yield return new WaitForSeconds(transition.duration);
		SceneManager.LoadScene("Menu");
	}

	// Center the player on the floor, putting him in the spawn room.
	//
	// The player is put at a certain elevation so that it looks like the player falls from the
	// previous floor.
	private void CenterPlayerOnFloor(float elevation=3.0f) {
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
					enemyManager.SpawnEnemiesInRoom(currentRoom);
				} else if (currentRoom.type == RoomType.Boss) {
					StartBossFight();
				}
			}
		}
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
