using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainBlocks {
    public GameObject room;
    public GameObject itemRoom;
    public GameObject boss;
    public GameObject passage;
}

[System.Serializable]
public class TerrainProps {
    public GameObject chest;
}

// Script which manages the floor generation and terrain manipulation.
public class TerrainManager : LayoutGrid {
    public TerrainBlocks terrainBlocks; // terrain blocks to place
    public TerrainProps terrainProps; // props that can be placed on the terrain
    public FloorConfiguration floorConfiguration; // configuration of the floor
    
    // list of the floor's rooms
    public readonly List<Room> rooms = new List<Room>(); 
    // list of the room positions
    private List<Vector2Int> roomLayoutPositions = new List<Vector2Int>();
    // list of unoccupied room position
    private List<Vector2Int> availableRoomPositions = new List<Vector2Int>();

    private List<Vector2Int> paths; // path cells
    public GameController game;

    // List of maze path cells on the terrain.
    public List<Vector2Int> mazePositions {
        get {
            return paths;
        }
    }

    void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}

    // Generate a new floor according to the size settings set in the editor.
    //
    // The rooms are first placed on the layout, then a maze is created in the
    // remaining space. Finally, the rooms are connected to the maze through
    // entrances.
    public void GenerateFloor() {
        gridSize = floorConfiguration.gridSize;
        InitializeGrid();
        do {
            GenerateRoomLayout();
        } while (!IsLayoutBalanced());
        CreateRooms();
        var centerRoom = rooms[0];
        var position = centerRoom.position;
        var size = centerRoom.size;
        var bounds = new Vector2Int(size.x / 2, size.y /2);
        var mazeEntrances = new List<Vector2Int> {
            new Vector2Int(position.x + bounds.x, position.y + 1),
            new Vector2Int(position.x + bounds.x, position.y + size.y - 2),
            new Vector2Int(position.x + 1, position.y + bounds.y),
            new Vector2Int(position.x + size.x - 2, position.y + bounds.y)
        };
        paths = new List<Vector2Int>(unvisited);
        GenerateMaze(mazeEntrances);
        ConnectRooms();
    }

    // Break the block at the given position, if possible.
    public void Break(Vector2Int position) {
        var instance = grid[position.x, position.y];
        if (instance != null && instance.transform.childCount > 0) {
            var wall = instance.transform.GetChild(0);
            var block = wall.GetComponent<Block>();
            if (block.breakable) {
                Remove(position);
                game.enemyManager.OnBreak(position);
            }
        }
    }

    // Return the room which contains the position, if any.
    //
    // Useful for finding the room that the player is currently in.
    public Room FindRoomAtPosition(Vector2Int position) {
        foreach (Room room in rooms) {
            if (room.Contains(position)) {
                return room;
            }
        }
        return null;
    }

    // Block the entrances of a room.
    //
    // Useful for keeping the player in a room until a event is finished, such as a boss fight.
    // This only blocks standard room entrances, not entrances dug by the player.
    public void BlockRoomEntrances(Room room) {
        foreach (Vector2Int entrance in room.entrances) {
            Place(room.wall, entrance);
        }
    }

    // Clear the entrance of a room.
    public void ClearRoomEntrances(Room room) {
        foreach (Vector2Int entrance in room.entrances) {
            Place(room.wall, entrance, true);
        }
    }

    // Place the prop at the given position.
    public void PlaceProp(GameObject prop, Vector2Int position) {
        var instancePosition = LayoutGrid.ToWorldPosition(position) + prop.transform.position;
        var instanceRotation = prop.transform.rotation;
        var instance = Instantiate(prop, instancePosition, instanceRotation);
        instance.transform.parent = transform;
    }

    // Generate a random layout of rooms for the floor.
    private void GenerateRoomLayout() {
        rooms.Clear();
        roomLayoutPositions.Clear();
        availableRoomPositions.Clear();
        for (int x = 0; x < floorConfiguration.layoutSize.x; ++x) {
            for (int y = 0; y < floorConfiguration.layoutSize.y; ++y) {
                var position = new Vector2Int(x, y);
                availableRoomPositions.Add(position);
            }
        }
        GenerateRoom(RoomType.Spawn, terrainBlocks.room, centered:true, minSize:true);
        GenerateRoom(RoomType.Item, terrainBlocks.room, singleEntrance:true);
        GenerateRoom(RoomType.Boss, terrainBlocks.boss, maxSize:true, singleEntrance:true);
        while (rooms.Count < floorConfiguration.roomCount) {
            GenerateRoom(RoomType.Enemy, terrainBlocks.room);
        }
    }

    // Check if the floor layout is balanced.
    //
    // This is used to prevent the random generation from skewing the room
    // distribution too much.
    private bool IsLayoutBalanced(int margin=1) {
        var expectedRowRooms = (float)(rooms.Count) / floorConfiguration.layoutSize.y;
        var expectedColumnRooms = (float)(rooms.Count) / floorConfiguration.layoutSize.x;
        var rowRooms = new int[floorConfiguration.layoutSize.y];
        var columnRooms = new int[floorConfiguration.layoutSize.x];
        foreach (Vector2Int roomLayoutPosition in roomLayoutPositions) {
            rowRooms[roomLayoutPosition.y] += 1;
            columnRooms[roomLayoutPosition.x] += 1;
        }
        foreach (int roomCount in rowRooms) {
            if (roomCount + margin < expectedRowRooms ||
                roomCount - margin > expectedRowRooms) {
                return false;
            }
        }
        foreach (int roomCount in columnRooms) {
            if (roomCount + margin < expectedColumnRooms ||
                roomCount - margin > expectedColumnRooms) {
                return false;
            }
        }
        return true;
    }

    // Generate a random new room to insert into the floor layout.
    //
    // The optional parameters can be set to inforce certain characteristiccs
    // during the generation.
    private void GenerateRoom(RoomType type,
                              GameObject wall,
                              bool centered=false,
                              bool minSize=false,
                              bool maxSize=false,
                              bool noEntrances=false,
                              bool singleEntrance=false,
                              int minEntranceCount=2,
                              int maxEntranceCount=4) {
        Vector2Int layoutPosition;
        Vector2Int positionOffset;
        Vector2Int size;
        List<Vector2Int> entrances = new List<Vector2Int>();
        if (centered) {
            layoutPosition = new Vector2Int(
                floorConfiguration.layoutSize.x / 2,
                floorConfiguration.layoutSize.y / 2
            );
            positionOffset = new Vector2Int(0, 0);
        } else {
            var index = Random.Range(0, availableRoomPositions.Count);
            layoutPosition = availableRoomPositions[index];
            positionOffset = new Vector2Int(
                Random.Range(
                    -floorConfiguration.roomMaxOffset.x / 2,
                    floorConfiguration.roomMaxOffset.x / 2
                ) * 2,
                Random.Range(
                    -floorConfiguration.roomMaxOffset.y / 2,
                    floorConfiguration.roomMaxOffset.y / 2
                ) * 2
            );
        }
        roomLayoutPositions.Add(layoutPosition);
        availableRoomPositions.Remove(layoutPosition);
        var gridPosition = floorConfiguration.ToGridPosition(layoutPosition) + positionOffset;
        if (minSize) {
            size = floorConfiguration.roomMinSize;
        } else if (maxSize) {
            size = floorConfiguration.roomMaxSize;
        } else {
            var sizeDelta = floorConfiguration.roomMaxSize - floorConfiguration.roomMinSize;
            size = floorConfiguration.roomMinSize + new Vector2Int(
                Random.Range(0, sizeDelta.x / 2) * 2,
                Random.Range(0, sizeDelta.y / 2) * 2
            );
        }
        if (!noEntrances) {
            var bounds = new Vector2Int(size.x / 2, size.y / 2);
            var possibleEntrances = new List<Vector2Int> {
                gridPosition + new Vector2Int(bounds.x, 0),
                gridPosition + new Vector2Int(bounds.x, size.y - 1),
                gridPosition + new Vector2Int(0, bounds.y),
                gridPosition + new Vector2Int(size.x - 1, bounds.y)
            };
            if (singleEntrance) {
                entrances.Add(possibleEntrances[0]);
            } else {
                var entranceCount = Random.Range(minEntranceCount, maxEntranceCount + 1);
                for (int i = 0; i < entranceCount; ++i) {
                    var index = Random.Range(0, possibleEntrances.Count);
                    var entrance = possibleEntrances[index];
                    possibleEntrances.RemoveAt(index);
                    entrances.Add(entrance);
                }
            }
        }
        var room = new Room(type, wall, gridPosition, size, entrances);
        rooms.Add(room);
    }

    // Create the rooms in the layout grid.
    //
    // Walls are placed on the bounds of the room and the inside of the room is
    // carved out.
    private void CreateRooms() {
        foreach (Room room in rooms) {
            var from = room.position;
            var to = new Vector2Int(
                from.x + room.size.x - 1,
                from.y + room.size.y - 1
            );
            for (int x = from.x; x <= to.x; ++x) {
                for (int y = from.y; y <= to.y; ++y) {
                    var position = new Vector2Int(x, y);
                    if (x == from.x || x == to.x || y == from.y || y == to.y) {
                        Place(room.wall, position);
                    } else {
                        Remove(position);
                        Place(room.wall, position, true);
                    }
                }
            }
            if (room.type == RoomType.Item) {
                PlaceProp(terrainProps.chest, room.centerPosition);
            }
        }
    }

    // Connect the created rooms to the recently generated maze layout by
    // inserting the rooms' entrances on the rooms' perimeters.
    private void ConnectRooms() {
        foreach (Room room in rooms) {
            foreach (Vector2Int entrance in room.entrances) {
                Remove(entrance);
            }
        }
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
