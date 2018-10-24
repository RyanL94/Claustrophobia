using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public GameObject[] standardEnemies; // standard enemies that spawn on the ground
	public GameObject[] terrainEnemies; // enemies that spawn from destroyed terrain
	public GameObject[] roomEnemies; // enemies that spawn upon room entry
	public GameObject[] bosses; // bosses encountered in boss rooms
	public Range enemiesPerRoom; // number of enemies that spawn in a room upon entry

	private GameObject player;

	void Start() {
		player = GameObject.FindWithTag("Player");
	}

	// Spawn enemies in the given room.
	public void SpawnEnemiesInRoom(Room room) {
		var roomPositions = room.groundPositions;
		roomPositions.Remove(LayoutGrid.FromWorldPosition(player.transform.position));
		var spawnPositions = RandomPicker.Pick(roomPositions, enemiesPerRoom);
		foreach (Vector2Int spawnPosition in spawnPositions) {
			var index = Random.Range(0, standardEnemies.Length);
			var enemy = standardEnemies[index];
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
