using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceTile : Tile
{
    public override void EnterTile()
    {
        Debug.Log("Dungeon Entrance Called!");
        DungeonController.Instance.CreateNewDungeon(5, new Vector2Int(3,3), new Vector2Int(6,6));
        // The map save should be inside DungeonController
        // Call create new dungeon with right params from DungeonController
    }
}
