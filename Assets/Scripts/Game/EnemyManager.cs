using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnConfiguration {
	public float initialSpawnRate; // initial chance of block breaking resulting in enemy spawn
	public float RampDuration; // spawn probability growth duration (100% when reached)
	public float breakRampIncrease; // time increase per block break

	private float spawnRateRamp; // increase rate of spawn probability
	private float initialTime; // start time of the floor
	private float breakRamp; // time added because of broken blocks

	// Effective time for the spawn mechanism.
	//
	// The time is relative to the start of the floor and increases on every block break.
	public float effectiveTime {
		get {
			return Time.time - initialTime + breakRamp;
		}
	}

	// Current spawn rate of enemies for destroyed blocks.
	public float spawnRate {
		get {
			return Mathf.Min(initialSpawnRate + spawnRateRamp * effectiveTime, 1);
		}
	}

	// Start a new spawn timer
	public void Initialize() {
		spawnRateRamp = (1 - initialSpawnRate) / RampDuration;
		initialTime = Time.time;
		breakRamp = 0;
	}

	// Increase the timer.
	public void OnBreak() {
		breakRamp += breakRampIncrease;
	}
}

public class EnemyManager : MonoBehaviour {

	public List<GameObject> terrainEnemies; // enemies that spawn from destroyed terrain
	public List<GameObject> mazeEnemies; // standard enemies that spawn in the maze
	public List<GameObject> roomEnemies; // enemies that spawn upon room entry
	public List<GameObject> bosses; // bosses encountered in boss rooms
	public Range enemiesPerFloor; // number of enemies that spawn on the maze grounds
	public Range enemiesPerRoom; // number of enemies that spawn in a room upon entry
	public SpawnConfiguration spawnConfiguration; // configuration of the enemy spawning

	private GameController game;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	// Action to perform on block breaking.
	public void OnBreak(Vector2Int blockPosition) {
		if (Random.value <= spawnConfiguration.spawnRate) {
			spawnConfiguration.OnBreak();
			var enemy = RandomPicker.Pick(terrainEnemies);
			Spawn(enemy, blockPosition);
		}
	}

	// Spawn enemies in the maze.
	public void SpawnMazeEnemies() {
		var mazePositions = game.terrain.mazePositions;
		var spawnPositions = RandomPicker.Pick(mazePositions, enemiesPerFloor);
		foreach (Vector2Int spawnPosition in spawnPositions) {
			var enemy = RandomPicker.Pick(mazeEnemies);
			Spawn(enemy, spawnPosition);
		}
	}

	// Spawn enemies in the given room.
	public void SpawnEnemiesInRoom(Room room) {
		var roomPositions = room.groundPositions;
		roomPositions.Remove(LayoutGrid.FromWorldPosition(game.player.transform.position));
		var spawnPositions = RandomPicker.Pick(roomPositions, enemiesPerRoom);
		foreach (Vector2Int spawnPosition in spawnPositions) {
			var enemy = RandomPicker.Pick(roomEnemies);
			Spawn(enemy, spawnPosition);
		}
	}

	// Spawn the enemy at the given position.
	//
	// The enemy will be looking towards the player.
	private void Spawn(GameObject enemy, Vector2Int spawnPosition) {
        Debug.Log("enemy spawn");
		var position = LayoutGrid.ToWorldPosition(spawnPosition, true) + enemy.transform.position;
		var direction = game.player.transform.position - position;
		var rotation = Quaternion.LookRotation(direction) * enemy.transform.rotation;
		Instantiate(enemy, position, rotation);
	}
}
