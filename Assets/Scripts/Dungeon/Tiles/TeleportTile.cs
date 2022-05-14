using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTile : Tile
{
    public Vector2Int Destination;

    public override void EnterTile()
    {
        if (Destination == null)
            return;
        // Teleport
        GameController.Instance.Player.SetPosition(Destination);
    }
}
