using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillTile : Tile
{
    private void Awake()
    {
        // Just for now
        IsWalkable = false;
    }

    public override void EnterTile()
    {
        // TO-DO: Open UI destory items
        // TO-DO: Open Inventory - and don't let it close by pressing I
    }

    // TO-DO: Exit Tile
}
