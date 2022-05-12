using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDownTile : Tile
{
    public override void EnterTile()
    {
        DungeonController.Instance.MoveFloorDown();
    }
}
