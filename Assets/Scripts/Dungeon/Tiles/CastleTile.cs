using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTile : Tile
{
    public Animator EmoteAnimator { get => emoteAnimator; set => emoteAnimator = value; }

    [SerializeField]
    private Animator emoteAnimator;

    public override void EnterTile()
    {
        emoteAnimator.ResetTrigger("CloseEmote");
        CinematicController.Instance.StartCinematic();
        UIController.Instance.OpenQuest();
        GameController.Instance.Player.Controls.UI.Exit.performed += context => ExitTile();
        emoteAnimator.SetTrigger("PlayEmote");

    }
    private void ExitTile()
    {
        CinematicController.Instance.EndCinematic();
        GameController.Instance.Player.BeginMove(Vector2Int.down);
        UIController.Instance.CloseQuest();
        emoteAnimator.SetTrigger("CloseEmote");
        GameController.Instance.Player.Controls.UI.Exit.performed -= context => ExitTile();
    }
}
