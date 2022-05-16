using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchTile : Tile
{
    private void Awake()
    {
        IsWalkable = false;
    }

    public override void EnterTile()
    {
        
    }
}
