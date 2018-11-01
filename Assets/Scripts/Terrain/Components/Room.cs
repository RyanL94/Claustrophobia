using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type of room.
//
// Different rooms may behave differently upon certain events.
public enum RoomType {
	Spawn,
	Item,
	Shop,
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

	// Whether the player has entered this room yet or not.
	//
	// This is notably useful to determine if enemies should spawn in the room,
	// since this can only occur upon the first entry of the room.
	public bool hasBeenEntered;

	// The list of ground tile positions in the room.
	//
	// Useful for knowing the possible locations for spawning enemies in a room.
	public List<Vector2Int> groundPositions {
		get {
			var positions = new List<Vector2Int>();
			for (int x = position.x + 1; x <= position.x + size.x - 2; ++x) {
				for (int y = position.y + 1; y <= position.y + size.y - 2; ++y) {
					var position = new Vector2Int(x, y);
					positions.Add(position);
				}
			}
			return positions;
		}
	}

	// The list of wall tile positions in the room.
	//
	// Useful for knowing the possible locations of walls to destroy in a room.
	// Note that this does not account for the player destroyed walls.
	public List<Vector2Int> wallPositions {
		get {
			var positions = new List<Vector2Int>();
			for (int x = position.x; x <= position.x + size.x - 1; ++x) {
				for (int y = position.y; y <= position.y + size.y - 1; ++y) {
					if (x == position.x ||
						x == position.x + size.x ||
						y == position.y ||
						y == position.y + size.y) {
						var position = new Vector2Int(x, y);
						if (!entrances.Contains(position)) {
							positions.Add(new Vector2Int(x, y));
						}
					}
				}
			}
			return positions;
		}
	}

	// The center position of the room.
	//
	// Useful when needing to place an object in the center of a room (e.g. item drop).
	public Vector2Int centerPosition {
		get {
			return position + new Vector2Int(size.x / 2, size.y / 2);
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
		if (position.x >= this.position.x + 1 &&
			position.x <= this.position.x + size.x - 1 &&
			position.y >= this.position.y + 1 &&
			position.y <= this.position.y + size.y - 1) {
			return true;
		}
		return false;
	}
}
