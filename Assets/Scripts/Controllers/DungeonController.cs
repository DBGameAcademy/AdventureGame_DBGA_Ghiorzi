using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : Singleton<DungeonController>
{
    
    public Floor CurrentFloor { get { return _currentDungeon.Floors[_floorIndex]; } }
    public Room CurrentRoom { get { return _currentDungeon.Floors[_floorIndex].Rooms[_roomPosition.x,_roomPosition.y]; } }

    [Header("Skyboxes")]
    [SerializeField]
    private Light light;
    [SerializeField]
    private Material mapSkybox;
    [SerializeField]
    private Material dungeonSkybox;

    [Header("TileSets")]
    [SerializeField]
    private TileSet mapTileSet;
    [SerializeField]
    private TileSet dungeonTileSet;

    [Header("Map generation")]
    [SerializeField]
    private int height;
    [SerializeField]
    private int width;
    [SerializeField]
    private int iterations = 3; // Conway GoL iterations
    [SerializeField]
    private int neighbours = 5;
    [SerializeField]
    private int numberOfDungeons = 3;

    [Header("Monsters settings")]
    [SerializeField]
    private int mapMobDensity;
    [SerializeField]
    private int dungeonMobDensity;

    private Dungeon _currentDungeon;
    private int _floorIndex = 0;
    private Vector2Int _roomPosition;

    private Floor _savedMapFloor;
    private Vector2Int _savedPlayerPos;

    public void CreateNewMap()
    {
        _currentDungeon = new Dungeon();
        _currentDungeon.Floors = new List<Floor>();
        // Create floor object
        GameObject floorObj = new GameObject("Floor Map " + 0);
        floorObj.transform.SetParent(transform);
        Floor floor = floorObj.AddComponent<Floor>();

        // Add current floor to current dungeon
        _currentDungeon.Floors.Add(floor);

        // Initialize floor's rooms
        floor.Rooms = new Room[1, 1];

        // Create Room object
        GameObject roomObj = new GameObject("Room (" + 0 + ";" + 0 + ")");
        roomObj.transform.SetParent(floorObj.transform);
        Room room = roomObj.AddComponent<Room>();

        // Add current room to the floor
        floor.Rooms[0, 0] = room;

        // Initialize room
        room.RoomPosition = new Vector2Int(0, 0);
        room.Tiles = new Tile[width, height];


        Vector2Int size = new Vector2Int(width, height);

        bool[,] positions = new bool[size.x, size.y];

        // Random first pass
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                positions[x, y] = (Random.Range(0, 2) == 0);
            }
        }

        // Smoothing - Conway
        for (int i = 0; i < iterations; i++)
        {
            bool[,] newPositions = new bool[size.x, size.y];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    int neighbourCount = 0;
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        for (int yOffset = -1; yOffset <= 1; yOffset++)
                        {
                            if (x + xOffset < 0 || y + yOffset < 0 || x + xOffset >= size.x || y + yOffset >= size.y)
                            {
                                neighbourCount++;
                            }
                            else if (positions[x + xOffset, y + yOffset])
                            {
                                neighbourCount++;
                            }
                        }
                    }
                    if (neighbourCount >= neighbours)
                    {
                        newPositions[x, y] = true;
                    }
                }
            }
            positions = newPositions;
        }

        bool isPlayerPlaced = false;
        // Populate tiles
        for (int tilex = 0; tilex < width; ++tilex)
        {
            for (int tiley = 0; tiley < height; ++tiley)
            {
                if (!positions[tilex, tiley])
                {
                    // Create Tile object
                    GameObject tileObj = new GameObject("Tile (" + tilex + ";" + tiley + ")");
                    tileObj.transform.SetParent(roomObj.transform);

                    // Add tile to room's tiles
                    room.Tiles[tilex, tiley] = tileObj.AddComponent<EmptyTile>();

                    // Initialize tile
                    room.Tiles[tilex, tiley].Position = new Vector2Int(tilex, tiley);

                    // Create Map
                    CurrentRoom.Tiles[tilex, tiley].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[tilex, tiley]),
                                                                            new Vector3(tilex, -0.5f, tiley),
                                                                            Quaternion.identity);
                    // Place player
                    if (!isPlayerPlaced)
                    {
                        GameController.Instance.Player.SetPosition(new Vector2Int(tilex, tiley));
                        GameController.Instance.Player.SetHeight(6.0f);
                        isPlayerPlaced = true;
                    }
                }
            }
        }

        // Find Regions
        List<Region> regions = new List<Region>();
        int count = IslandFinder.CountIslands(positions, out regions, false);
        Debug.Log(count);
        for (int i = 0; i < regions.Count; i++)
        {
            for (int j = 0; j < regions.Count; j++)
            {
                if (i != j)
                {
                    var closePoint = regions[i].FindClosestPointToRegion(regions[j]);
                    regions[i].RegionConnections.Add(closePoint);
                }
            }
        }
        
        // Fill parents for regions
        Dijkstra.Calculate(regions);

        // Create teleports
        foreach (Region region in regions)
        {
            foreach (Region parent in region.Parents)
            {
                foreach (ConnectionPoint point in parent.RegionConnections)
                {
                    if (point.ConnectedRegion == region)
                    {
                        if (CurrentRoom.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y] == null)
                            continue;
                        // FIRST
                        // Destroy tile before
                        if (CurrentRoom.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y].TileObj != null)
                            Destroy(CurrentRoom.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y].TileObj);

                        // Create Tile object
                        GameObject tileObj = new GameObject("Teleport (" + point.FirstTilePoistion.x + ";" + point.FirstTilePoistion.y + ")");
                        tileObj.transform.SetParent(room.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y].transform);

                        // Add tile to room's tiles
                        room.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y] = tileObj.AddComponent<TeleportTile>();

                        // Initialize tile
                        room.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y].Position = new Vector2Int(point.FirstTilePoistion.x, point.FirstTilePoistion.y);
                        TeleportTile tp = (TeleportTile)room.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y];
                        tp.Destination = point.SecondTilePosition;

                        // Create obj in Map
                        CurrentRoom.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y]),
                                                                                new Vector3(point.FirstTilePoistion.x, -0.5f, point.FirstTilePoistion.y),
                                                                                Quaternion.identity);
                        tp.Particle = CurrentRoom.Tiles[point.FirstTilePoistion.x, point.FirstTilePoistion.y].TileObj.GetComponent<TeleportTile>().Particle;

                        // SECOND
                        if (CurrentRoom.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y].TileObj != null)
                            Destroy(CurrentRoom.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y].TileObj);

                        // Create Tile object
                        tileObj = new GameObject("Teleport (" + point.SecondTilePosition.x + ";" + point.SecondTilePosition.y + ")");
                        tileObj.transform.SetParent(room.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y].transform);

                        // Add tile to room's tiles
                        room.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y] = tileObj.AddComponent<TeleportTile>();

                        // Initialize tile
                        room.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y].Position = new Vector2Int(point.SecondTilePosition.x, point.SecondTilePosition.y);
                        tp = (TeleportTile)room.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y];
                        tp.Destination = point.FirstTilePoistion;

                        // Create obj in Map
                        CurrentRoom.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y]),
                                                                                new Vector3(point.SecondTilePosition.x, -0.5f, point.SecondTilePosition.y),
                                                                                Quaternion.identity);
                        tp.Particle = CurrentRoom.Tiles[point.SecondTilePosition.x, point.SecondTilePosition.y].TileObj.GetComponent<TeleportTile>().Particle;
                    }
                }
            }
        }

        // Create DungeonEntrance - only 1 for each region
        List<Region> regionsWithDungeon = new List<Region>();
        for(int i=0; i<numberOfDungeons; i++) 
        {
            Region randomRegion = null;
            do
            {
                randomRegion = regions[Random.Range(0, regions.Count)];

            } while (regionsWithDungeon.Contains(randomRegion));
            regionsWithDungeon.Add(randomRegion);

            // Find random position
            bool isPlaced = false;
            do
            {
                Vector2Int pos = randomRegion.TilePositions[Random.Range(0, randomRegion.TilePositions.Count)];
                if (room.Tiles[pos.x, pos.y] == null || room.Tiles[pos.x, pos.y].GetType() != typeof(EmptyTile))
                {
                    isPlaced =  false;
                    continue;
                }
                isPlaced = true;
                // Create object
                // Destroy prev if any
                if (CurrentRoom.Tiles[pos.x, pos.y].TileObj != null)
                    Destroy(CurrentRoom.Tiles[pos.x, pos.y].TileObj);
                // Create Tile object
                GameObject tileObj = new GameObject("DungeonEntrance (" + pos.x + ";" + pos.y + ")");
                tileObj.transform.SetParent(room.Tiles[pos.x, pos.y].transform);
                // Add tile to room's tiles
                room.Tiles[pos.x, pos.y] = tileObj.AddComponent<DungeonEntranceTile>();
                // Initialize tile
                room.Tiles[pos.x, pos.y].Position = new Vector2Int(pos.x, pos.y);
                // Create map object
                CurrentRoom.Tiles[pos.x, pos.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[pos.x, pos.y]),
                                                                           new Vector3(pos.x, -1.0f, pos.y + 1),
                                                                           Quaternion.Euler(0.0f, 180.0f, 0.0f));
                // Fill empty tiles around Dungeon Entrance
                if (!TileHasNeighbour(pos, Vector2Int.up))
                {
                    Vector2Int newPos = pos + Vector2Int.up;
                    if (!(newPos.x < 0
                      || newPos.y < 0
                      || newPos.x >= CurrentRoom.Tiles.GetLength(0)
                      || newPos.y >= CurrentRoom.Tiles.GetLength(1)))
                    {
                        // Create Tile object
                        tileObj = new GameObject("Dungeon Extra Tile " + Vector2Int.up.ToString());
                        tileObj.transform.SetParent(room.Tiles[pos.x, pos.y].transform);
                        // Add tile to room's tiles
                        room.Tiles[newPos.x, newPos.y] = tileObj.AddComponent<EmptyTile>();
                        // Initialize tile
                        room.Tiles[newPos.x, newPos.y].Position = new Vector2Int(newPos.x, newPos.y);
                        // Create map object
                        CurrentRoom.Tiles[newPos.x, newPos.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[newPos.x, newPos.y]),
                                                                                   new Vector3(newPos.x, -0.5f, newPos.y),
                                                                                   Quaternion.identity);
                    }
                }
                if (!TileHasNeighbour(pos, Vector2Int.down))
                {
                    Vector2Int newPos = pos + Vector2Int.down;
                    if (!(newPos.x < 0
                      || newPos.y < 0
                      || newPos.x >= CurrentRoom.Tiles.GetLength(0)
                      || newPos.y >= CurrentRoom.Tiles.GetLength(1)))
                    {
                        // Create Tile object
                        tileObj = new GameObject("Dungeon Extra Tile " + Vector2Int.down.ToString());
                        tileObj.transform.SetParent(room.Tiles[pos.x, pos.y].transform);
                        // Add tile to room's tiles
                        room.Tiles[newPos.x, newPos.y] = tileObj.AddComponent<EmptyTile>();
                        // Initialize tile
                        room.Tiles[newPos.x, newPos.y].Position = new Vector2Int(newPos.x, newPos.y);
                        // Create map object
                        CurrentRoom.Tiles[newPos.x, newPos.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[newPos.x, newPos.y]),
                                                                                   new Vector3(newPos.x, -0.5f, newPos.y),
                                                                                   Quaternion.identity);
                    }
                }
                if (!TileHasNeighbour(pos, Vector2Int.right))
                {
                    Vector2Int newPos = pos + Vector2Int.right;
                    if (!(newPos.x < 0
                      || newPos.y < 0
                      || newPos.x >= CurrentRoom.Tiles.GetLength(0)
                      || newPos.y >= CurrentRoom.Tiles.GetLength(1)))
                    {
                        // Create Tile object
                        tileObj = new GameObject("Dungeon Extra Tile " + Vector2Int.right.ToString());
                        tileObj.transform.SetParent(room.Tiles[pos.x, pos.y].transform);
                        // Add tile to room's tiles
                        room.Tiles[newPos.x, newPos.y] = tileObj.AddComponent<EmptyTile>();
                        // Initialize tile
                        room.Tiles[newPos.x, newPos.y].Position = new Vector2Int(newPos.x, newPos.y);
                        // Create map object
                        CurrentRoom.Tiles[newPos.x, newPos.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[newPos.x, newPos.y]),
                                                                                   new Vector3(newPos.x, -0.5f, newPos.y),
                                                                                   Quaternion.identity);
                    }
                }
                if (!TileHasNeighbour(pos, Vector2Int.left))
                {
                    Vector2Int newPos = pos + Vector2Int.left;
                    if (!(newPos.x < 0
                       || newPos.y < 0
                       || newPos.x >= CurrentRoom.Tiles.GetLength(0)
                       || newPos.y >= CurrentRoom.Tiles.GetLength(1)))
                    {
                        // Create Tile object
                        tileObj = new GameObject("Dungeon Extra Tile " + Vector2Int.left.ToString());
                        tileObj.transform.SetParent(room.Tiles[pos.x, pos.y].transform);
                        // Add tile to room's tiles
                        room.Tiles[newPos.x, newPos.y] = tileObj.AddComponent<EmptyTile>();
                        // Initialize tile
                        room.Tiles[newPos.x, newPos.y].Position = new Vector2Int(newPos.x, newPos.y);
                        // Create map object
                        CurrentRoom.Tiles[newPos.x, newPos.y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[newPos.x, newPos.y]),
                                                                                   new Vector3(newPos.x, -0.5f, newPos.y),
                                                                                   Quaternion.identity);
                    }
                }

            } while (!isPlaced);
        }
        
        // Spawn Monsters
        int spawnAttempts = 0;
        do
        {
            if(TryGetRandomTile(_currentDungeon.Floors[0],out Tile tile, out Room tileRoom))
            {
                // Spawn only Slimes in the map for now
                Monster monster = MonsterController.Instance.AddMonster(eMonsterID.Slime);
                tile.SetCharacterObject(monster);
                monster.SetPosition(tile.Position);
            }
            spawnAttempts++;
        }
        while (spawnAttempts < mapMobDensity);
        
        SaveMap();
    }

    public void CreateNewDungeon(int noOfFloor, Vector2Int roomsPerFloor, Vector2Int roomSize)
    {
        UIController.Instance.ShowDungeonInfo(_floorIndex);
        MonsterController.Instance.DestroyAllMonster();
        SavePlayerPosition();
        ClearCurrentRoom();
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
                        GameController.Instance.Player.SetPosition(tilePos);
                        GameController.Instance.Player.SetHeight(5.0f);
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
        // Place torch
        foreach(Floor floor in _currentDungeon.Floors)
        {
            for (int x = 0; x < floor.Rooms.GetLength(0); ++x)
            {
                for (int y = 0; y < floor.Rooms.GetLength(1); ++y)
                {
                    Room room = floor.Rooms[x, y];
                    int torchNumbers = Random.Range(0,3);
                    List<int> positionList = new List<int>() { 0, 1, 2 }; 
                    for(int i=0; i<torchNumbers; ++i)
                    {
                        int randomPosition = Random.Range(0,positionList.Count);
                        if(positionList[randomPosition] == 0)
                        {
                            // Find Free tiles
                            List<Tile> freeTiles = new List<Tile>();
                            for(int tiley = 0; tiley < room.Tiles.GetLength(1); ++tiley)
                            {
                                if(room.Tiles[0, tiley].GetType() == typeof(EmptyTile))
                                {
                                    freeTiles.Add(room.Tiles[0, tiley]);
                                }
                            }
                            int randomTilePos = Random.Range(0,freeTiles.Count);
                            int randY = freeTiles[randomTilePos].Position.y;
                            // Destroy tile before
                            Destroy(room.Tiles[0, randY].TileObj);
                            // Create Tile object
                            GameObject tileObj = new GameObject("Torch (" + 0 + ";" +randY + ")");
                            tileObj.transform.SetParent(room.Tiles[0, randY].transform);
                            // Add tile to room's tiles
                            room.Tiles[0, randY] = tileObj.AddComponent<TorchTile>();
                            // Init
                            room.Tiles[0, randY].Position = new Vector2Int(0, randY);
                            
                        }
                        if (positionList[randomPosition] == 1)
                        {
                            // Find Free tiles
                            List<Tile> freeTiles = new List<Tile>();
                            int tiley = room.Tiles.GetLength(1) - 1;
                            for (int tilex = 0; tilex < room.Tiles.GetLength(0); ++tilex)
                            {
                                if (room.Tiles[tilex, tiley].GetType() == typeof(EmptyTile))
                                {
                                    freeTiles.Add(room.Tiles[tilex, tiley]);
                                }
                            }
                            int randomTilePos = Random.Range(0, freeTiles.Count);
                            int randX = freeTiles[randomTilePos].Position.x;
                            // Destroy tile before
                            Destroy(room.Tiles[randX, tiley].TileObj);
                            // Create Tile object
                            GameObject tileObj = new GameObject("Torch (" + randX + ";" + tiley + ")");
                            tileObj.transform.SetParent(room.Tiles[randX, tiley].transform);
                            // Add tile to room's tiles
                            room.Tiles[randX, tiley] = tileObj.AddComponent<TorchTile>();
                            // Init
                            room.Tiles[randX, tiley].Position = new Vector2Int(randX, tiley);
                           
                        }
                        if (positionList[randomPosition] == 2)
                        {
                            // Find Free tiles
                            List<Tile> freeTiles = new List<Tile>();
                            int tilex = room.Tiles.GetLength(0) - 1;
                            for (int tiley = 0; tiley < room.Tiles.GetLength(1); ++tiley)
                            {
                                if (room.Tiles[tilex, tiley].GetType() == typeof(EmptyTile))
                                {
                                    freeTiles.Add(room.Tiles[tilex, tiley]);
                                }
                            }
                            int randomTilePos = Random.Range(0, freeTiles.Count);
                            int randY = freeTiles[randomTilePos].Position.y;
                            // Destroy tile before
                            Destroy(room.Tiles[tilex, randY].TileObj);
                            // Create Tile object
                            GameObject tileObj = new GameObject("Torch (" + tilex + ";" + randY + ")");
                            tileObj.transform.SetParent(room.Tiles[tilex, randY].transform);
                            // Add tile to room's tiles
                            room.Tiles[tilex, randY] = tileObj.AddComponent<TorchTile>();
                            // Init
                            room.Tiles[tilex, randY].Position = new Vector2Int(tilex, randY);
                            
                        }
                        positionList.Remove(positionList[randomPosition]);
                    }
                }
            }
        }

        MakeCurrentRoom();
    }

    private void MakeCurrentRoom()
    {
        light.intensity = 0.2f;
        if (RenderSettings.skybox != dungeonSkybox)
            RenderSettings.skybox = dungeonSkybox;
        for(int x=0; x < CurrentRoom.Size.x; ++x)
        {
            for(int y=0; y< CurrentRoom.Size.y; ++y)
            {
                Quaternion rotation = Quaternion.identity;
                if(CurrentRoom.Tiles[x,y].GetType() == typeof(TorchTile))
                {
                    if (x == 0 && y < (CurrentRoom.Tiles.GetLength(1) - 1))
                        rotation = Quaternion.Euler(0, -90, 0);
                    else if (x == (CurrentRoom.Tiles.GetLength(0) - 1))
                        rotation = Quaternion.Euler(0,90,0);
                }
                CurrentRoom.Tiles[x,y].TileObj = Instantiate(dungeonTileSet.GetTilePrefab(CurrentRoom.Tiles[x, y]),new Vector3(x,-0.5f,y), rotation);
            }
        }
        // Spawn Monsters again
        int spawnAttempts = 0;
        do
        {
            if (TryGetRandomTile(CurrentFloor, out Tile tile, out Room tileRoom))
            {
                Monster monster = MonsterController.Instance.AddMonster(eMonsterID.Skeleton);
                tile.SetCharacterObject(monster);
                monster.SetPosition(tile.Position);
            }
            spawnAttempts++;
        }
        while (spawnAttempts < dungeonMobDensity);
        Debug.Log("Spawn attempts " + spawnAttempts);
    }
    
    public void ClearCurrentRoom()
    {
        for(int x=0; x<CurrentRoom.Size.x; ++x)
        {
            for(int y=0; y<CurrentRoom.Size.y; ++y)
            {
                if (CurrentRoom.Tiles[x, y] != null)
                {
                    // Destroy Map prefab
                    Destroy(CurrentRoom.Tiles[x, y].TileObj);
                }
            }
        }
        MonsterController.Instance.DestroyAllMonster();
    }

    public void MoveFloorUp()
    {
        ClearCurrentRoom();
        if (_floorIndex-1 < 0)
        {
            // Back to Map
            LoadMap();
            MakeMap();
            return;
        }
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
        UIController.Instance.ShowDungeonInfo(_floorIndex);
        GameController.Instance.Player.SetPosition(CurrentFloor.FloorUpTransition.TilePosition);
        GameController.Instance.Player.SetHeight(5.0f);

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

    public Tile GetTile(Vector2Int position)
    {
        return CurrentRoom.Tiles[position.x, position.y];
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
    
    private bool TileHasNeighbour(Vector2Int pos, Vector2Int direction)
    {
        Vector2Int testPos = pos + direction;
        if (testPos.x < 0
           || testPos.y < 0
           || testPos.x >= CurrentRoom.Tiles.GetLength(0)
           || testPos.y >= CurrentRoom.Tiles.GetLength(1))
        {
            return false;
        }
        if(CurrentRoom.Tiles[testPos.x,testPos.y] == null)
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

    private bool TryGetRandomTile(Floor floor, out Tile tile, out Room room)
    {
        Vector2Int pos = Vector2Int.zero;
        tile = null;
        room = floor.Rooms[Random.Range(0, floor.Rooms.GetLength(0)), Random.Range(0, floor.Rooms.GetLength(1))];
        if (room == null)
        {
            return false;
        }

        pos = new Vector2Int(Random.Range(0, room.Size.x), Random.Range(0, room.Size.y));
        tile = room.Tiles[pos.x, pos.y];
        if (room.Tiles[pos.x,pos.y] == null || room.Tiles[pos.x, pos.y].GetType() != typeof(EmptyTile))
        {
            return false;
        }

        if (room.Tiles[pos.x, pos.y].CharacterObject != null)
            return false;

        return true;
    }

    private void SaveMap()
    {
        _savedMapFloor = CurrentFloor;
    }
    private void SavePlayerPosition()
    {
        _savedPlayerPos = new Vector2Int((int)GameController.Instance.Player.transform.position.x, (int)GameController.Instance.Player.transform.position.z);
    }
    private void LoadMap()
    {
        for (int i = 0; i < _currentDungeon.Floors.Count; ++i)
        {
            Destroy(_currentDungeon.Floors[i].gameObject);
        }
        _currentDungeon.Floors[0] = _savedMapFloor;
    }
    private void MakeMap()
    {
        light.intensity = 1.0f;
        if (RenderSettings.skybox != mapSkybox)
            RenderSettings.skybox = mapSkybox;

        _floorIndex = 0;
        _roomPosition = Vector2Int.zero;
        for (int x = 0; x < CurrentRoom.Size.x; ++x)
        {
            for (int y = 0; y < CurrentRoom.Size.y; ++y)
            {
                if (CurrentRoom.Tiles[x, y] == null)
                    continue;
                if (CurrentRoom.Tiles[x, y].GetType() == typeof(DungeonEntranceTile))
                {
                    CurrentRoom.Tiles[x, y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[x, y]),
                                                                           new Vector3(x, -1.0f, y + 1),
                                                                           Quaternion.Euler(0.0f, 180.0f, 0.0f));
                }
                else if (CurrentRoom.Tiles[x, y].GetType() == typeof(TeleportTile))
                {
                    CurrentRoom.Tiles[x, y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[x, y]), new Vector3(x, -0.5f, y), Quaternion.identity);
                    TeleportTile tp = (TeleportTile)CurrentRoom.Tiles[x, y];
                    tp.Particle = CurrentRoom.Tiles[x, y].TileObj.GetComponent<TeleportTile>().Particle;
                }
                else
                    CurrentRoom.Tiles[x, y].TileObj = Instantiate(mapTileSet.GetTilePrefab(CurrentRoom.Tiles[x, y]), new Vector3(x, -0.5f, y), Quaternion.identity);
            }
        }
        GameController.Instance.Player.SetPosition(_savedPlayerPos);

        // Spawn Monsters again
        int spawnAttempts = 0;
        do
        {
            if (TryGetRandomTile(_currentDungeon.Floors[0], out Tile tile, out Room tileRoom))
            {
                // Spawn only Slimes in the map for now
                Monster monster = MonsterController.Instance.AddMonster(eMonsterID.Slime);
                tile.SetCharacterObject(monster);
                monster.SetPosition(tile.Position);
            }
            spawnAttempts++;
        }
        while (spawnAttempts < mapMobDensity);
    }
}
