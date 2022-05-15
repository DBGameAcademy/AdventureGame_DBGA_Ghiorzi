using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceTile : Tile
{
    public override void EnterTile()
    {
        DungeonController.Instance.CreateNewDungeon(5, new Vector2Int(3,3), new Vector2Int(6,6));
    }
}
