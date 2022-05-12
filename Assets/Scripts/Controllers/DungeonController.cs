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
                            room.Tiles[tilex,tiley] = tileObj.AddComponent<EmptyTile>();

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
                            // Create object
                            GameObject tileObj = new GameObject("Door");
                            tileObj.transform.SetParent(room.Tiles[roomSize.x / 2, roomSize.y - 1].transform);
                            Vector2Int pos = room.Tiles[roomSize.x / 2, roomSize.y - 1].Position;
                            // Add
                            room.Tiles[roomSize.x / 2, roomSize.y - 1] = tileObj.AddComponent<DoorTile>();
                            // Initialize
                            DoorTile door = (DoorTile)room.Tiles[roomSize.x / 2, roomSize.y - 1];
                            door.Direction = Vector2Int.up;
                            door.Position = pos;
                        }
                        if (RoomHasNeighbour(room, Vector2Int.down))
                        {
                            // Create object
                            GameObject tileObj = new GameObject("Door");
                            tileObj.transform.SetParent(room.Tiles[roomSize.x / 2, 0].transform);
                            Vector2Int pos = room.Tiles[roomSize.x / 2, 0].Position;
                            // Add
                            room.Tiles[roomSize.x / 2, 0] = tileObj.AddComponent<DoorTile>();
                            // Initialize
                            DoorTile door = (DoorTile)room.Tiles[roomSize.x / 2, 0];
                            door.Direction = Vector2Int.down;
                            door.Position = pos;
                        }
                        if (RoomHasNeighbour(room, Vector2Int.left))
                        {
                            // Create object
                            GameObject tileObj = new GameObject("Door");
                            tileObj.transform.SetParent(room.Tiles[0, roomSize.y / 2].transform);
                            Vector2Int pos = room.Tiles[0, roomSize.y / 2].Position;
                            // Add
                            room.Tiles[0, roomSize.y / 2] = tileObj.AddComponent<DoorTile>();
                            // Initialize
                            DoorTile door = (DoorTile)room.Tiles[0, roomSize.y / 2];
                            door.Direction = Vector2Int.left;
                            door.Position = pos;
                        }
                        if (RoomHasNeighbour(room, Vector2Int.right))
                        {
                            // Create object
                            GameObject tileObj = new GameObject("Door");
                            tileObj.transform.SetParent(room.Tiles[roomSize.x - 1, roomSize.y / 2].transform);
                            Vector2Int pos = room.Tiles[roomSize.x - 1, roomSize.y / 2].Position;
                            // Add
                            room.Tiles[roomSize.x - 1, roomSize.y / 2] = tileObj.AddComponent<DoorTile>();
                            // Initialize
                            DoorTile door = (DoorTile)room.Tiles[roomSize.x - 1, roomSize.y / 2];
                            door.Direction = Vector2Int.right;
                            door.Position = pos;
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
                if(TrySetRandomTile(_currentDungeon.Floors[i],new FloorUpTile(),out Vector2Int tilePos, out Room room))
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
                if(TrySetRandomTile(_currentDungeon.Floors[i],new FloorDownTile(), out Vector2Int tilePos, out Room room))
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
                CurrentRoom.Tiles[x,y].TileObj = Instantiate(tileSet.GetTilePrefab(CurrentRoom.Tiles[x, y]),new Vector3(x,-0.5f,y), Quaternion.identity);
            }
        }
    }
    
    private void ClearCurrentRoom()
    {
        for(int x=0; x<CurrentRoom.Size.x; ++x)
        {
            for(int y=0; y<CurrentRoom.Size.y; ++y)
            {
                Destroy(CurrentRoom.Tiles[x,y].TileObj);  
            }
        }
    }
    
    public void MoveFloorUp()
    {
        ClearCurrentRoom();
        _floorIndex--;
        _roomPosition = CurrentFloor.FloorDownTransition.TargetRoom.RoomPosition;
        MakeCurrentRoom();
        GameController.Instance.Player.SetPosition(CurrentFloor.FloorDownTransition.TilePosition);
    }

    public void MoveFloorDown()
    {
        ClearCurrentRoom();
        _floorIndex++;
        _roomPosition = CurrentFloor.FloorUpTransition.TargetRoom.RoomPosition;
        MakeCurrentRoom();
        GameController.Instance.Player.SetPosition(CurrentFloor.FloorUpTransition.TilePosition);
    }
    
    
    public void MoveRoom(Vector2Int direction)
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
    
    private bool TrySetRandomTile(Floor floor, Tile tile, out Vector2Int pos, out Room room)
    {
        pos = Vector2Int.zero;
        room = floor.Rooms[Random.Range(0, floor.Rooms.GetLength(0)), Random.Range(0, floor.Rooms.GetLength(1))];
        if(room == null)
        {
            return false;
        }

        pos = new Vector2Int(Random.Range(0, room.Size.x), Random.Range(0, room.Size.y));
        if(room.Tiles[pos.x,pos.y].GetType() != typeof(EmptyTile))
        {
            return false;
        }

        // Create object
        GameObject tileObj = new GameObject("Floor");
        tileObj.transform.SetParent(room.Tiles[pos.x, pos.y].transform);
        Vector2Int position = room.Tiles[pos.x, pos.y].Position;
        // Add
        if(tile.GetType() == typeof(FloorDownTile))
            room.Tiles[pos.x, pos.y] = tileObj.AddComponent<FloorDownTile>();
        else if (tile.GetType() == typeof(FloorUpTile))
            room.Tiles[pos.x, pos.y] = tileObj.AddComponent<FloorUpTile>();

        room.Tiles[pos.x, pos.y].Position = position;

        return true;
    }
}
