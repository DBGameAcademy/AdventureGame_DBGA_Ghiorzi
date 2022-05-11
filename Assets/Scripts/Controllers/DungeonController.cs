using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : Singleton<DungeonController>
{
    public Floor CurrentFloor { get { return _currentDungeon.Floors[_floorIndex]; } }
    public Room CurrentRoom { get { return _currentDungeon.Floors[_floorIndex].Rooms[_roomPosition.x,_roomPosition.y]; } }

    [SerializeField]
    private TileSet tileSet;

    [SerializeField]
    private int noOfFloor;
    [SerializeField]
    private Vector2Int roomsPerFloor;
    [SerializeField]
    private Vector2Int roomSize;
    
    private Dungeon _currentDungeon;
    private int _floorIndex = 0;
    private Vector2Int _roomPosition;

    public void EnterTile(Tile tile)
    {
        // Handle tile enter based on tile type
        switch (tile.ID)
        {
            case Tile.eTileID.DoorDown:
                MoveRoom(Vector2Int.down);
                break;
            case Tile.eTileID.DoorUp:
                MoveRoom(Vector2Int.up);
                break;
            case Tile.eTileID.DoorLeft:
                MoveRoom(Vector2Int.left);
                break;
            case Tile.eTileID.DoorRight:
                MoveRoom(Vector2Int.right);
                break;

            case Tile.eTileID.FloorDown:
                MoveFloorDown();
                break;
            case Tile.eTileID.FloorUp:
                MoveFloorUp();
                break;
        }
    }

    public void CreateNewDungeon()
    {
        _currentDungeon = new Dungeon();
        _currentDungeon.Floors = new List<Floor>();

        for(int i=0; i<noOfFloor; ++i)
        {
            // Create floor object
            GameObject floorObj = new GameObject("Floor " + i);
            floorObj.transform.SetParent(transform);
            Floor floor = floorObj.AddComponent<Floor>();

            // Add current floor to current dungeon
            _currentDungeon.Floors.Add(floor);

            // Initialize floor's rooms
            floor.Rooms = new Room[roomsPerFloor.x, roomsPerFloor.y];
            // Populate floors with rooms
            for(int x = 0; x < roomsPerFloor.x; ++x)
            {
                for(int y=0; y<roomsPerFloor.y; ++y)
                {
                    // Create Room object
                    GameObject roomObj = new GameObject("Room (" + x + ";" + y+")");
                    roomObj.transform.SetParent(floorObj.transform);
                    Room room = roomObj.AddComponent<Room>();

                    // Add current room to the floor
                    floor.Rooms[x, y] = room;

                    // Initialize room
                    room.RoomPosition = new Vector2Int(x, y);
                    room.Tiles = new Tile[roomSize.x, roomSize.y];

                    // Populate tiles
                    for(int tilex =0; tilex<roomSize.x; ++tilex)
                    {
                        for(int tiley=0; tiley<roomSize.y; ++tiley)
                        {
                            // Create Tile object
                            GameObject tileObj = new GameObject("Tile (" + tilex + ";" + tiley + ")");
                            tileObj.transform.SetParent(roomObj.transform);

                            // Add tile to room's tiles
                            room.Tiles[tilex,tiley] = tileObj.AddComponent<Tile>();

                            // Initialize tile
                            room.Tiles[tilex, tiley].Position = new Vector2Int(tilex, tiley);
                        }
                    }
                }
            }
        }

        // Add doors in appropriate places
        foreach(Floor floor in _currentDungeon.Floors)
        {
            for(int x=0;x<floor.Rooms.GetLength(0); ++x)
            {
                for(int y=0;y<floor.Rooms.GetLength(1); ++y)
                {
                    Room room = floor.Rooms[x, y];
                    if (room != null)
                    {
                        if(RoomHasNeighbour(room, Vector2Int.up))
                        {
                            room.Tiles[roomSize.x / 2, roomSize.y - 1].ID = Tile.eTileID.DoorUp;
                        }
                        if (RoomHasNeighbour(room, Vector2Int.down))
                        {
                            room.Tiles[roomSize.x / 2, 0].ID = Tile.eTileID.DoorDown;
                        }
                        if (RoomHasNeighbour(room, Vector2Int.left))
                        {
                            room.Tiles[0, roomSize.y / 2].ID = Tile.eTileID.DoorLeft;
                        }
                        if (RoomHasNeighbour(room, Vector2Int.right))
                        {
                            room.Tiles[roomSize.x - 1, roomSize.y / 2].ID = Tile.eTileID.DoorRight;
                        }
                    }
                }
            }
        }

        // Place FloorUp and FloorDown
        for(int i = 0; i < _currentDungeon.Floors.Count; ++i)
        {
            bool placedFloorUp = false;
            do
            {
                // What if random we set FloorUp and FloorDown on the same tile? - we are not checking that, we are checking only the type, with typef(Tile)
                // but ofc they will be of type Tile (?)
                if(TrySetRandomTile(_currentDungeon.Floors[i],Tile.eTileID.FloorUp,out Vector2Int tilePos, out Room room))
                {
                    // If floor 0
                    if (i == 0)
                    {
                        _roomPosition = room.RoomPosition;
                        GameController.Instance.Player.SetPosition(_roomPosition);
                    }
                    placedFloorUp = true;
                    _currentDungeon.Floors[i].FloorUpTransition = new FloorTransition(room, tilePos);
                }
            }
            while (!placedFloorUp);

            bool placedFloorDown = false;
            do
            {
                if(TrySetRandomTile(_currentDungeon.Floors[i],Tile.eTileID.FloorDown, out Vector2Int tilePos, out Room room))
                {
                    placedFloorDown = true;
                    _currentDungeon.Floors[i].FloorDownTransition = new FloorTransition(room, tilePos);
                }

            } while (!placedFloorDown);
        }
    }

    public void MakeCurrentRoom()
    {
        for(int x=0; x < CurrentRoom.Size.x; ++x)
        {
            for(int y=0; y< CurrentRoom.Size.y; ++y)
            {
                GameObject defaultTile = Instantiate(tileSet.GetTilePrototype(TilePrototype.eTileID.Empty).PrefabObject, new Vector3(x,0,y), Quaternion.identity);

                CurrentRoom.Tiles[x, y].TileObjects.Add(defaultTile);

                TilePrototype.eTileID id = TilePrototype.eTileID.Empty;
                switch (CurrentRoom.Tiles[x, y].ID)
                {
                    case Tile.eTileID.DoorUp:
                    case Tile.eTileID.DoorLeft:
                    case Tile.eTileID.DoorRight:
                    case Tile.eTileID.DoorDown:
                        id = TilePrototype.eTileID.Door;
                        break;
                    case Tile.eTileID.FloorDown:
                        id = TilePrototype.eTileID.FloorDown;
                        break;
                    case Tile.eTileID.FloorUp:
                        id = TilePrototype.eTileID.FloorUp;
                        break;
                }
                if(id!= TilePrototype.eTileID.Empty)
                {
                    GameObject prefabObject = tileSet.GetTilePrototype(id).PrefabObject;
                    if (prefabObject != null)
                    {
                        GameObject newTileObj = Instantiate(prefabObject, new Vector3(x, 0, y), Quaternion.identity);
                        newTileObj.transform.SetParent(CurrentRoom.Tiles[x, y].transform);
                        CurrentRoom.Tiles[x, y].TileObjects.Add(newTileObj);
                    }
                    else
                    {
                        Debug.LogError("Missing GameObject for: " + id);
                    }
                }
            }
        }
    }

    private void ClearCurrentRoom()
    {
        for(int x=0; x<CurrentRoom.Size.x; ++x)
        {
            for(int y=0; y<CurrentRoom.Size.y; ++y)
            {
                for(int i = CurrentRoom.Tiles[x,y].TileObjects.Count - 1; i>=0; i--)
                {
                    Destroy(CurrentRoom.Tiles[x,y].TileObjects[i]);
                }
                CurrentRoom.Tiles[x, y].TileObjects.Clear();
            }
        }
    }

    private void MoveFloorUp()
    {
        ClearCurrentRoom();
        _floorIndex--;
        _roomPosition = CurrentFloor.FloorDownTransition.TargetRoom.RoomPosition;
        MakeCurrentRoom();
        GameController.Instance.Player.SetPosition(CurrentFloor.FloorDownTransition.TilePosition);
    }

    private void MoveFloorDown()
    {
        ClearCurrentRoom();
        _floorIndex++;
        _roomPosition = CurrentFloor.FloorUpTransition.TargetRoom.RoomPosition;
        MakeCurrentRoom();
        GameController.Instance.Player.SetPosition(CurrentFloor.FloorUpTransition.TilePosition);
    }

    private void MoveRoom(Vector2Int direction)
    {
        Vector2Int targetRoomPos = CurrentRoom.RoomPosition + direction;
        Room targetRoom = CurrentFloor.Rooms[targetRoomPos.x, targetRoomPos.y];

        ClearCurrentRoom();

        _roomPosition = targetRoom.RoomPosition;

        if(direction.x < 0) // Left
        {
            GameController.Instance.Player.SetPosition(new Vector2Int(CurrentRoom.Size.x - 1, CurrentRoom.Size.y / 2));
        }
        else if(direction.x > 0) // Right
        {
            GameController.Instance.Player.SetPosition(new Vector2Int(0, CurrentRoom.Size.y / 2));
        }
        else if (direction.y < 0) // Down
        {
            GameController.Instance.Player.SetPosition(new Vector2Int(CurrentRoom.Size.x /2, CurrentRoom.Size.y -1));
        }
        else if(direction.y > 0) // Up
        {
            GameController.Instance.Player.SetPosition(new Vector2Int(CurrentRoom.Size.x / 2, 0));
        }

        MakeCurrentRoom();
    }

    private bool RoomHasNeighbour(Room checkRoom, Vector2Int direction)
    {
        Vector2Int testPos = checkRoom.RoomPosition + direction;
        if(testPos.x < 0 
            || testPos.y<0
            || testPos.x >= CurrentFloor.Rooms.GetLength(0) 
            || testPos.y >= CurrentFloor.Rooms.GetLength(1))
        {
            return false;
        }
        if(CurrentFloor.Rooms[testPos.x, testPos.y] == null)
        {
            return false;
        }

        return true;
    }

    private bool TrySetRandomTile(Floor floor, Tile.eTileID tileID, out Vector2Int pos, out Room room)
    {
        pos = Vector2Int.zero;
        room = floor.Rooms[Random.Range(0, floor.Rooms.GetLength(0)), Random.Range(0, floor.Rooms.GetLength(1))];
        if(room == null)
        {
            return false;
        }

        pos = new Vector2Int(Random.Range(0, room.Size.x), Random.Range(0, room.Size.y));
        if(room.Tiles[pos.x,pos.y].GetType() != typeof(Tile))
        {
            return false;
        }

        room.Tiles[pos.x, pos.y].ID = tileID;
        return true;
    }
}
