using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Configuration a game floor layout.
[System.Serializable]
public class FloorConfiguration {
    public Vector2Int layoutSize; // size of the floor in rooms
    public Vector2Int roomMinSize; // minimum room size
    public Vector2Int roomMaxSize; // maximum room size
    public Vector2Int roomMaxOffset; // maximum room grid position offset
    public Vector2Int roomGap; // gap between room rows/columns
    public int roomCount; // number of room the generate in the floor

    // Return the size of the grid required to fit the floor.
    public Vector2Int gridSize {
        get {
            return new Vector2Int(
                layoutSize.x * roomMaxSize.x + roomGap.x * (layoutSize.x + 1) + 2,
                layoutSize.y * roomMaxSize.y + roomGap.y * (layoutSize.y + 1) + 2
            );
        }
    }

    // Return the corresponding grid position of a room layout position.
    public Vector2Int ToGridPosition(Vector2Int layoutPosition) {
        return new Vector2Int(
            layoutPosition.x * roomMaxSize.x + roomGap.x * (layoutPosition.x + 1) + 1,
            layoutPosition.y * roomMaxSize.y + roomGap.y * (layoutPosition.y + 1) + 1
        );
    }

    // Return the position of the center grid cell in the floor layout.
    public Vector2Int FindCenterPosition() {
        return new Vector2Int(gridSize.x / 2, gridSize.y / 2);
    }
}