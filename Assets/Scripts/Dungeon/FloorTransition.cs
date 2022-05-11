using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTransition 
{
    public Room TargetRoom { get; private set; }
    public Vector2Int TilePosition { get; private set; }

    public FloorTransition(Room room, Vector2Int tilePosition)
    {
        TargetRoom = room;
        TilePosition = tilePosition;
    }
}
