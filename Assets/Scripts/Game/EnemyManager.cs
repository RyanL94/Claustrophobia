using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnConfiguration {
	public IntRange enemiesPerFloor; // number of enemies that spawn on the maze grounds
	public IntRange enemiesPerRoom; // number of enemies that spawn in a room upon entry
	public FloatRange spawnRate; // chance of block breaking resulting in enemy spawn
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
	public float currentSpawnRate {
		get {
			return Mathf.Min(spawnRate.min + spawnRateRamp * effectiveTime, spawnRate.max);
		}
	}

	// Start a new spawn timer.
	public void Initialize() {
		spawnRateRamp = (spawnRate.min - spawnRate.max) / RampDuration;
		initialTime = Time.time;
		breakRamp = 0;
	}

	// Increase the timer.
	public void OnBreak() {
		breakRamp += breakRampIncrease;
	}
}

[System.Serializable]
public class TimerConfiguration {
	public IntRange blocksDestroyed; // number of blocks destroyed per tremor
	public FloatRange frequency; // how long apart the tremors are
	public float ramp; // how much sooner the next tremor is
	public Vector2Int range; // furthest range away for which blocks can be affected by tremors

	// Current frequency of the timer.
	// The frequency get increased every time this variable is accessed.
	private float _currentFrequency;
	public float currentFrequency {
		get {
			var frequencyToReturn = _currentFrequency;
			_currentFrequency = Mathf.Max(_currentFrequency - ramp, frequency.min);
			return frequencyToReturn;
		}
	}

	// Create a new tremor timer.
	public void Initialize() {
		_currentFrequency = frequency.max;
	}
}

public class EnemyManager : MonoBehaviour {

	public List<GameObject> terrainEnemies; // enemies that spawn from destroyed terrain
	public List<GameObject> mazeEnemies; // standard enemies that spawn in the maze
	public List<GameObject> roomEnemies; // enemies that spawn upon room entry
	public List<GameObject> bosses; // bosses encountered in boss rooms
	public SpawnConfiguration spawnConfiguration; // configuration of the enemy spawning
	public TimerConfiguration timerConfiguration; // configuration of the block breaking timer
    public AudioClip tremorSoundEffect;
	public float soundVolume;

	private GameController game;
	private IEnumerator timer;

	void Awake() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

	public void Initialize() {
		if (timer != null) {
			StopCoroutine(timer);
		}
		SpawnMazeEnemies();
		spawnConfiguration.Initialize();
		timerConfiguration.Initialize();
		timer = Timer();
		StartCoroutine(timer);
	}

	// Action to perform on block breaking.
	public void OnBreak(Vector2Int blockPosition) {
		if (Random.value <= spawnConfiguration.currentSpawnRate) {
			spawnConfiguration.OnBreak();
			var enemy = RandomPicker.Pick(terrainEnemies);
			Spawn(enemy, blockPosition);
		}
	}

	// Spawn enemies in the maze.
	public void SpawnMazeEnemies() {
		var mazePositions = game.terrain.mazePositions;
		var spawnPositions = RandomPicker.Pick(mazePositions, spawnConfiguration.enemiesPerFloor);
		foreach (Vector2Int spawnPosition in spawnPositions) {
			var enemy = RandomPicker.Pick(mazeEnemies);
			Spawn(enemy, spawnPosition);
		}
	}

	// Spawn enemies in the given room.
	public void SpawnEnemiesInRoom(Room room) {
		var roomPositions = room.groundPositions;
		roomPositions.Remove(LayoutGrid.FromWorldPosition(game.player.transform.position));
		var spawnPositions = RandomPicker.Pick(roomPositions, spawnConfiguration.enemiesPerRoom);
		foreach (Vector2Int spawnPosition in spawnPositions) {
			var enemy = RandomPicker.Pick(roomEnemies);
			Spawn(enemy, spawnPosition);
		}
	}

	// Spawn the boss.
	public GameObject SpawnBoss(Room room) {
		var boss = bosses[0];
		return Spawn(boss, room.centerPosition + Vector2Int.up);
	}

	// Spawn the enemy at the given position.
	//
	// The enemy will be looking towards the player.
	private GameObject Spawn(GameObject enemy, Vector2Int spawnPosition) {
		var position = LayoutGrid.ToWorldPosition(spawnPosition, true) + enemy.transform.position;
		var direction = game.player.transform.position - position;
		var rotation = Quaternion.LookRotation(direction) * enemy.transform.rotation;
		var instance = Instantiate(enemy, position, rotation);
		instance.transform.parent = transform;
		return instance;
	}

	// Time which creates a tremor a breaks block at an increasing frequency.
	private IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds(timerConfiguration.currentFrequency);
			AudioSource.PlayClipAtPoint(tremorSoundEffect, game.camera.transform.position, soundVolume);
			yield return new WaitForSeconds(3.0f);
			game.camera.Shake();
			yield return new WaitForSeconds(0.25f);
			var playerPosition = LayoutGrid.FromWorldPosition(game.player.transform.position);
			var wallPositions = new List<Vector2Int>();
			for (int x = playerPosition.x - timerConfiguration.range.x;
					x <= playerPosition.x + timerConfiguration.range.x; ++x) {
				for (int y = playerPosition.y - timerConfiguration.range.y;
						y <= playerPosition.y + timerConfiguration.range.y; ++y) {
					var position = new Vector2Int(x, y);
					if (game.terrain.IsWall(position)) {
						wallPositions.Add(position);
					}
				}
			}
			var destroyedWalls = RandomPicker.Pick(
				wallPositions,
				timerConfiguration.blocksDestroyed
			);
			foreach (Vector2Int position in destroyedWalls) {
				game.terrain.Break(position);
			}
		}
	} 
}
