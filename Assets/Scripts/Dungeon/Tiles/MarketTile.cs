using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTile : Tile
{
    public Animator EmoteAnimator { get => emoteAnimator; set => emoteAnimator = value; }

    [SerializeField]
    private Animator emoteAnimator;

    public override void EnterTile()
    {
        // Show Market UI
        GameController.Instance.Player.StopMoving();
        CinematicController.Instance.StartCinematic();
        UIController.Instance.ShowShop();
        emoteAnimator.SetTrigger("PlayEmote");
    }
}
