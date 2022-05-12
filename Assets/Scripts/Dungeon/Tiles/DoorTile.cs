using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : Tile
{
    public Vector2Int Direction { get; set; }

    public override void EnterTile()
    {
        // Move based on direction
        DungeonController.Instance.MoveRoom(Direction);
    }
}
