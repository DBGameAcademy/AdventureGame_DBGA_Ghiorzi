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

        for(int i=0; i<noOfFloor; ++i)
        {
            // Create floor object
            GameObject floorObj = new GameObject("Floor " + i);
            floorObj.transform.SetParent(transform);
            Floor floor = floorObj.AddComponent<Floor>();

            // Add current floor to current dungeon
            _currentDungeon.Floors = new List<Floor>();
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
    }

    public void MakeCurrentRoom()
    {
        for(int x=0; x < CurrentRoom.Size.x; ++x)
        {
            for(int y=0; y< CurrentRoom.Size.y; ++y)
            {
                GameObject defaultTile = Instantiate(tileSet.GetTilePrototype(TilePrototype.eTileID.Empty).PrefabObject, new Vector3(x,0,y), Quaternion.identity);

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
                    }
                    else
                    {
                        Debug.LogError("Missing GameObject for: " + id);
                    }
                }
            }
        }
    }
}
