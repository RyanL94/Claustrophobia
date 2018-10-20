using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type of room.
//
// Different rooms may behave differently upon certain events.
public enum RoomType {
	Spawn,
	Enemy,
	Boss
}

// Floor room.
public class Room {
	public readonly RoomType type; // type that the room is
	public readonly GameObject wall; // type of wall to use to create the room
	public readonly Vector2Int position; // position of the bottom left corner of the room
	public readonly Vector2Int size; // size of the room
	public readonly List<Vector2Int> entrances; // positions of the room's entrances

	// Whether the player has entered this room yet or not
	//
	// This is notably useful to determine if enemies should spawn in the room,
	// since this can only occur upon the first entry of the room.
	public bool hasBeenEntered {
		get {
			return hasBeenEntered;
		}
		set {
			hasBeenEntered = value;
		}
	}

	// Create a new room.
	public Room(RoomType type,
				GameObject wall,
				Vector2Int position,
				Vector2Int size,
				List<Vector2Int> entrances) {
		this.type = type;
		this.wall = wall;
		this.position = position;
		this.size = size;
		this.entrances = entrances;
	}

	// Check if the given position is contained within the room.
	//
	// The walls surrounding the room are considered outside.
	public bool Contains(Vector2Int position) {
		if (position.x > this.position.x + 1 &&
			position.x <= this.position.x + size.x - 1 &&
			position.y > this.position.y + 1 &&
			position.y <= this.position.y + size.y - 1) {
			return true;
		}
		return false;
	}
}
