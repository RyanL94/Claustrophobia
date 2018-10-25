using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnConfiguration {
	public Range enemiesPerRoom; // number of enemies that spawn in a room upon entry
	public float initialSpawnRate; // initial chance of block breaking resulting in enemy spawn
	public float RampDuration; // spawn probability growth duration (100% when reached)
	public float breakRampIncrease; // time increase per block break

	private float spawnRateRamp; // increase rate of spawn probability
	private float initialTime; // start time of the floor
	private float breakRamp; // time added because of broken blocks

	// Current spawn rate of enemies for destroyed blocks.
	public float spawnRate {
		get {
			var effectiveTime = Time.time - initialTime + breakRamp;
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

	public List<GameObject> standardEnemies; // standard enemies that spawn on the ground
	public List<GameObject> terrainEnemies; // enemies that spawn from destroyed terrain
	public List<GameObject> roomEnemies; // enemies that spawn upon room entry
	public List<GameObject> bosses; // bosses encountered in boss rooms
	public SpawnConfiguration spawnConfiguration; // configuration of the enemy spawning

	private GameObject player;

	void Start() {
		player = GameObject.FindWithTag("Player");
	}

	// Action to perform on block breaking.
	public void OnBreak(Vector2Int blockPosition) {
		if (Random.value <= spawnConfiguration.spawnRate) {
			spawnConfiguration.OnBreak();
			var enemy = RandomPicker.Pick(terrainEnemies);
			Spawn(enemy, blockPosition);
		}
	}

	// Spawn enemies in the given room.
	public void SpawnEnemiesInRoom(Room room) {
		var roomPositions = room.groundPositions;
		roomPositions.Remove(LayoutGrid.FromWorldPosition(player.transform.position));
		var spawnPositions = RandomPicker.Pick(roomPositions, spawnConfiguration.enemiesPerRoom);
		foreach (Vector2Int spawnPosition in spawnPositions) {
			var enemy = RandomPicker.Pick(standardEnemies);
			Spawn(enemy, spawnPosition);
		}
	}

	// Spawn the enemy at the given position.
	//
	// The enemy will be looking towards the player.
	private void Spawn(GameObject enemy, Vector2Int spawnPosition) {
		var position = LayoutGrid.ToWorldPosition(spawnPosition, true) + enemy.transform.position;
		var direction = player.transform.position - position;
		var rotation = Quaternion.LookRotation(direction) * enemy.transform.rotation;
		Instantiate(enemy, position, rotation);
	}
}
