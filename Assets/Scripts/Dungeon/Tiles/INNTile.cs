using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INNTile : Tile
{
    public Animator EmoteAnimator { get => emoteAnimator; set => emoteAnimator = value; }

    [SerializeField]
    private Animator emoteAnimator;

    public override void EnterTile()
    {
        GameController.Instance.Player.IsInBuilding = true;
        GameController.Instance.Player.StopMoving();
        UIController.Instance.ShowBattleUI();
        StartCoroutine(CORestoreHealth());
        CinematicController.Instance.PlayCinematicForSeconds(2.0f, () =>
        {
            emoteAnimator.SetTrigger("CloseEmote");
            GameController.Instance.Player.BeginMove(Vector2Int.down);
            GameController.Instance.Player.IsInBuilding = false;
        });
        // Start Cinematic
        // End Cinematic -> Player sprite visible + player pos front INN
    }

    private IEnumerator CORestoreHealth()
    {
        // Emote animation
        emoteAnimator.SetTrigger("PlayEmote");
        yield return new WaitForSeconds(1.0f);
        // Restore full health
        GameController.Instance.Player.RestoreHealth();
    }
}
