using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Floor room.
public class Room {
	public readonly GameObject wall; // type of wall to use to create the room
	public readonly Vector2Int position; // position of the bottom left corner of the room
	public readonly Vector2Int size; // size of the room
	public readonly List<Vector2Int> entrances; // positions of the room's entrances

	// Create a new room.
	public Room(GameObject wall, Vector2Int position, Vector2Int size, List<Vector2Int> entrances) {
		this.wall = wall;
		this.position = position;
		this.size = size;
		this.entrances = entrances;
	}
}
