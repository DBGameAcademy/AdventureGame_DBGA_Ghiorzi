using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorativeTile : Tile
{
    private void Awake()
    {
        IsWalkable = false;
    }

    public override void EnterTile()
    {

    }
}
