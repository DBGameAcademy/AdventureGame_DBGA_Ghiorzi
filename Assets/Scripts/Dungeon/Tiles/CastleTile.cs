using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTile : Tile
{
    public override void EnterTile()
    {
        CinematicController.Instance.StartCinematic();
        UIController.Instance.OpenQuest();
        GameController.Instance.Player.Controls.UI.Exit.performed += context => ExitTile();

    }
    private void ExitTile()
    {
        CinematicController.Instance.EndCinematic();
        GameController.Instance.Player.BeginMove(Vector2Int.down);
        UIController.Instance.CloseQuest();
        GameController.Instance.Player.Controls.UI.Exit.performed -= context => ExitTile();
    }
}
