using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceTile : Tile
{
    public override void EnterTile()
    {
        Debug.Log("Dungeon Entrance Called!");
        // The map save should be inside DungeonController
        // Call create new dungeon with right params from DungeonController
    }
}
