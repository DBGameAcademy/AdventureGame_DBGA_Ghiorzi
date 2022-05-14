using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleportTile : Tile
{
    public Vector2Int Destination;

    public override void EnterTile()
    {
        if (Destination == null)
            return;
        CinematicController.Instance.StartCinematic();
        GameController.Instance.Player.SetPosition(Destination);
        // Teleport
        StartCoroutine(COWaitBeforeAction(1.0f, () => { CinematicController.Instance.EndCinematic(); }));
        
    }

    private IEnumerator COWaitBeforeAction(float delay,Action Callback)
    {
        yield return new WaitForSeconds(delay);        
        Callback?.Invoke();
    }
}
