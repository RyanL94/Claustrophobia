using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public readonly GameObject wall;
	public readonly Vector2Int position;
	public readonly Vector2Int size;
	public readonly List<Vector2Int> entrances;

	public Room(GameObject wall, Vector2Int position, Vector2Int size, List<Vector2Int> entrances) {
		this.wall = wall;
		this.position = position;
		this.size = size;
		this.entrances = entrances;
	}
}
