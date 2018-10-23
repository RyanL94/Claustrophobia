using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridBlocks {
    public GameObject standard;
    public GameObject boundary;
}

// Layout grid that the terrain system is based on.
public class LayoutGrid : MonoBehaviour {
    public GridBlocks gridBlocks; // blocks to place in the layout
    protected Vector2Int gridSize; // size of the grid in cells
    protected GameObject[,] grid; // grid which contains the instances of blocks
    private HashSet<Vector2Int> unvisited; // list of unvisited cells (used for maze generation)

    // Create an empty grid layout.
    // The walls can be removed afterwards to create rooms and paths.    
    public void InitializeGrid() {
        Clear();
        grid = new GameObject[gridSize.x, gridSize.y];
        unvisited = new HashSet<Vector2Int>();
        for (int x = 0; x < gridSize.x; ++x) {
            for (int y = 0; y < gridSize.y; ++y) {
                var position = new Vector2Int(x, y);
                if (x == 0 || x == gridSize.x - 1 || y == 0 || y == gridSize.y - 1) {
                    Place(gridBlocks.boundary, position);
                } else if (x % 2 == 0 || y % 2 == 0) {
                    Place(gridBlocks.standard, position);
                } else {
                    Place(gridBlocks.standard, position, true);
                    unvisited.Add(position);
                }
            }
        }
    }
    
    // Convert a Vector2Int grid position into a Vector3 world position.
    public static Vector3 ToWorldPosition(Vector2Int position) {
        return new Vector3(position.x, 0.0f, position.y);
    }

    // Convert a Vector3 world position into a Vector2Int grid position.
    public static Vector2Int FromWorldPosition(Vector3 position) {
        return new Vector2Int((int)Mathf.Floor(position.x), (int)Mathf.Floor(position.z));
    }

    // Place a block at the given position.
    // Use the ground parameter to place a ground block without covering it with a wall.
    public void Place(GameObject block, Vector2Int position, bool ground=false) {
        Empty(position);
        var worldPosition = ToWorldPosition(position);
        try {
            // place ground block
            var groundBlock = block.GetComponent<Block>().ground;
            var blockPosition = worldPosition + groundBlock.transform.position;
            var blockRotation = groundBlock.transform.rotation;
            var groundInstance = Instantiate(groundBlock, blockPosition, blockRotation);
            groundInstance.transform.parent = transform;
            grid[position.x, position.y] = groundInstance;
            
            // place wall block
            if (!ground) {
                blockPosition = worldPosition + block.transform.position;
                blockRotation = block.transform.rotation;
                var wallInstance = Instantiate(block, blockPosition, blockRotation);
                wallInstance.transform.parent = groundInstance.transform;
            }
        }

        // if the game object to place is not an actual terrain block
        catch (NullReferenceException) {
            var blockPosition = worldPosition + block.transform.position;
            var blockRotation = block.transform.rotation;
            var instance = Instantiate(block, blockPosition, blockRotation);
            instance.transform.parent = transform;
            grid[position.x, position.y] = instance;
        }
    }

    // Remove the block at the given position, if any.
    public void Remove(Vector2Int position) {
        var instance = grid[position.x, position.y];
        var block = instance.GetComponent<Block>();
        if (block != null) {
            Place(instance, position, true);
        } else {
            Place(gridBlocks.standard, position, true);
        }
        unvisited.Remove(position);
    }

    // Empty the cell at the given position, destroying any block or tile.
    public void Empty(Vector2Int position) {
        var instance = grid[position.x, position.y];
        if (instance != null) {
            instance.transform.parent = null;
            Destroy(instance);
        }
    }

    // Destroy all of the grid's block game objects.
    public virtual void Clear() {
        while (transform.childCount > 0) {
            var child = transform.GetChild(0);
            child.parent = null;
            Destroy(child.gameObject);
        }
        grid = null;
    }

    // Check if the given grid position is a wall.
    public bool IsWall(Vector2Int position) {
        var instance = grid[position.x, position.y];
        if (instance != null && instance.transform.parent.name == "Walls") {
            return true;
        }
        return false;
    }

    // Check if the given position is within the grid.
    protected bool IsInbounds(Vector2Int position) {
        return position.x >= 0
            && position.x < gridSize.x
            && position.y >= 0
            && position.y < gridSize.y;
    }

    // Check if the given position has not been visited yet.
    protected bool IsUnvisited(Vector2Int position) {
        return unvisited.Contains(position);
    }

    // Return the list of unvisited neighboring positions .
    protected List<Vector2Int> FindUnvisitedNeighbors(Vector2Int position) {
        var unvisitedNeighbors = new List<Vector2Int>();
        var neighbors = new Vector2Int[] {
            position + new Vector2Int(2, 0),
            position + new Vector2Int(0, 2),
            position + new Vector2Int(-2, 0),
            position + new Vector2Int(0, -2)
        };
        foreach (Vector2Int neighbor in neighbors) {
            if (IsInbounds(neighbor) && IsUnvisited(neighbor)) {
                unvisitedNeighbors.Add(neighbor);
            }
        }
        return unvisitedNeighbors;
    }
}