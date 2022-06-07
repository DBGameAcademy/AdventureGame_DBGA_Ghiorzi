using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastleTile : Tile
{
    public Animator EmoteAnimator { get => emoteAnimator; set => emoteAnimator = value; }

    [SerializeField]
    private Animator emoteAnimator;

    public override void EnterTile()
    {
        GameController.Instance.Player.StopMoving();
        emoteAnimator.ResetTrigger("CloseEmote");
        CinematicController.Instance.StartCinematic();
        UIController.Instance.OpenQuest();
        GameController.Instance.Player.Controls.UI.Exit.performed += ExitTile;
        emoteAnimator.SetTrigger("PlayEmote");

    }
    private void ExitTile(InputAction.CallbackContext obj)
    {
        CinematicController.Instance.EndCinematic();
        GameController.Instance.Player.BeginMove(Vector2Int.down);
        UIController.Instance.CloseQuest();
        emoteAnimator.SetTrigger("CloseEmote");
        GameController.Instance.Player.Controls.UI.Exit.performed -= ExitTile;
    }
}
