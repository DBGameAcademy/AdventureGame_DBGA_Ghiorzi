using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUpTile : Tile
{
    public override void EnterTile()
    {
        DungeonController.Instance.MoveFloorUp();
    }
}
