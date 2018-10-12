using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayout : MonoBehaviour {
    public GameObject boundary;
    public GameObject wall;
    protected Vector2Int gridSize;
    private GameObject[,] grid;
    private HashSet<Vector2Int> unvisited;

    public static Vector3 ToWorldPosition(Vector2Int position) {
        return new Vector3(position.x, 0.0f, position.y);
    }
    
    public void InitializeGrid() {
        Clear();
        grid = new GameObject[gridSize.x, gridSize.y];
        unvisited = new HashSet<Vector2Int>();
        for (int x = 0; x < gridSize.x; ++x) {
            for (int y = 0; y < gridSize.y; ++y) {
                var position = new Vector2Int(x, y);
                if (x == 0 || x == gridSize.x - 1 || y == 0 || y == gridSize.y - 1) {
                    Place(boundary, position);
                } else if (x % 2 == 0 || y % 2 == 0) {
                    Place(wall, position);
                } else {
                    unvisited.Add(position);
                }
            }
        }
    }

    public void Place(GameObject block, Vector2Int position) {
        Remove(position);
        var instance = Instantiate(block, ToWorldPosition(position), Quaternion.identity);
        instance.transform.parent = transform;
        grid[position.x, position.y] = instance;
    }

    public void Remove(Vector2Int position) {
        var instance = grid[position.x, position.y];
        if (instance != null) {
            Destroy(instance);
        }
        grid[position.x, position.y] = null;
        unvisited.Remove(position);
    }

    public void Clear() {
        if (grid != null) {
            for (int x = 0; x < grid.GetLength(0); ++x) {
                for (int y = 0; y < grid.GetLength(1); ++y) {
                    var position = new Vector2Int(x, y);
                    Remove(position);
                }
            }
        }
    }

    protected bool IsInbounds(Vector2Int position) {
        return position.x >= 0
            && position.x < gridSize.x
            && position.y >= 0
            && position.y < gridSize.y;
    }

    protected bool IsUnvisited(Vector2Int position) {
        return unvisited.Contains(position);
    }

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

    // Generate a maze to fill in the remaining grid space.
    // 
    // The maze is generated using the `Growing Tree` algorithm.
    // The maze starts generating from the given list of cells.
    // The split determines the chance that the next cell generated is next to
    // the most recent cell (like the `Recursive Backtrack algorithm`), as
    // opposed to a random one (like `Prim's algorithm`).
    protected void GenerateMaze(List<Vector2Int> cells, float split=0.5f) {
        while (cells.Count > 0) {
            int index;
            if (Random.value > 0.5) {
                index = cells.Count - 1;
            } else {
                index = Random.Range(0, cells.Count);
            }
            var cell = cells[index];
            var neighbors = FindUnvisitedNeighbors(cell);
            if (neighbors.Count == 0) {
                cells.RemoveAt(index);
                continue;
            }
            var neighbor = neighbors[Random.Range(0, neighbors.Count)];
            var delta = neighbor - cell;
            var wall = cell + new Vector2Int(delta.x / 2, delta.y / 2);
            Remove(wall);
            Remove(neighbor);
            cells.Add(neighbor);
        }
    }
}
