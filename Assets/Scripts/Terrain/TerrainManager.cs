using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script which manages the floor generation and terrain manipulation .
public class TerrainManager : LayoutGrid {
    public GameObject bossWallBlock; // block that encases the boss room
    public Vector2Int layoutSize; // size of the floor in rooms
    public Vector2Int roomMinSize; // minimum room size
    public Vector2Int roomMaxSize; // maximum room size
    public Vector2Int roomMaxOffset; // maximum room grid position offset
    public Vector2Int roomGap; // gap between room rows/columns
    public int roomCount; // number of room the generate in the floor
    
    // list of the floor's rooms
    public readonly List<Room> rooms = new List<Room>(); 
    // list of the room positions
    private List<Vector2Int> roomLayoutPositions = new List<Vector2Int>();
    // list of unoccupied room position
    private List<Vector2Int> availableRoomPositions = new List<Vector2Int>();

    protected override void Start() {
        base.Start();
        GenerateFloor();
    }

    // Generate a new floor according to the size settings set in the editor.
    //
    // The rooms are first placed on the layout, then a maze is created in the
    // remaining space. Finally, the rooms are connected to the maze through
    // entrances.
    public void GenerateFloor() {
        gridSize = new Vector2Int(
            layoutSize.x * roomMaxSize.x + roomGap.x * (layoutSize.x + 1) + 2,
            layoutSize.y * roomMaxSize.y + roomGap.y * (layoutSize.y + 1) + 2
        );
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
        GenerateMaze(mazeEntrances);
        ConnectRooms();
    }

    // Return the position of the center grid cell in the floor layout.
    public Vector2Int FindCenterPosition() {
        return new Vector2Int(gridSize.x / 2, gridSize.y / 2);
    }

    // Return the corresponding grid position of a room layout position.
    private Vector2Int ToGridPosition(Vector2Int layoutPosition) {
        return new Vector2Int(
            layoutPosition.x * roomMaxSize.x + roomGap.x * (layoutPosition.x + 1) + 1,
            layoutPosition.y * roomMaxSize.y + roomGap.y * (layoutPosition.y + 1) + 1
        );
    }

    // Generate a random layout of rooms for the floor.
    private void GenerateRoomLayout() {
        rooms.Clear();
        roomLayoutPositions.Clear();
        availableRoomPositions.Clear();
        for (int x = 0; x < layoutSize.x; ++x) {
            for (int y = 0; y < layoutSize.y; ++y) {
                var position = new Vector2Int(x, y);
                availableRoomPositions.Add(position);
            }
        }
        GenerateRoom(standardWallBlock, centered:true, minSize:true); // spawn room
        GenerateRoom(bossWallBlock, maxSize:true, singleEntrance:true); // boss room
        while (rooms.Count < roomCount) {
            GenerateRoom(standardWallBlock); // regular rooms
        }
    }

    // Check if the floor layout is balanced.
    //
    // This is used to prevent the random generation from skewing the room
    // distribution too much.
    private bool IsLayoutBalanced(int margin=1) {
        var expectedRowRooms = (float)(rooms.Count) / layoutSize.y;
        var expectedColumnRooms = (float)(rooms.Count) / layoutSize.x;
        var rowRooms = new int[layoutSize.y];
        var columnRooms = new int[layoutSize.x];
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
    private void GenerateRoom(GameObject wall,
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
            layoutPosition = new Vector2Int(layoutSize.x / 2, layoutSize.y / 2);
            positionOffset = new Vector2Int(0, 0);
        } else {
            var index = Random.Range(0, availableRoomPositions.Count);
            layoutPosition = availableRoomPositions[index];
            positionOffset = new Vector2Int(
                Random.Range(-roomMaxOffset.x / 2, roomMaxOffset.x / 2) * 2,
                Random.Range(-roomMaxOffset.y / 2, roomMaxOffset.y / 2) * 2
            );
        }
        roomLayoutPositions.Add(layoutPosition);
        availableRoomPositions.Remove(layoutPosition);
        var gridPosition = ToGridPosition(layoutPosition) + positionOffset;
        if (minSize) {
            size = roomMinSize;
        } else if (maxSize) {
            size = roomMaxSize;
        } else {
            var sizeDelta = roomMaxSize - roomMinSize;
            size = roomMinSize + new Vector2Int(
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
        var room = new Room(wall, gridPosition, size, entrances);
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
                    }
                }
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
