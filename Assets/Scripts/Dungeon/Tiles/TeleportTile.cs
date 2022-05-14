using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TeleportTile : Tile
{
    public ParticleSystem Particle { get => lightParticle; set => lightParticle = value; }
    public Vector2Int Destination { get; set; }
    [SerializeField]
    private ParticleSystem lightParticle;

    public override void EnterTile()
    {
        if (Destination == null)
            return;
        GameController.Instance.Player.StopMoving();
        CinematicController.Instance.StartCinematic();
        lightParticle.Play();
        // Teleport
        StartCoroutine(COWaitBeforeAction(1.5f, () => {
            GameController.Instance.Player.SetPosition(Destination);
            StartCoroutine(COWaitBeforeAction(0.5f, () =>
            {
                CinematicController.Instance.EndCinematic();
            }));
        }));
        
    }

    private IEnumerator COWaitBeforeAction(float delay,Action Callback)
    {
        yield return new WaitForSeconds(delay);        
        Callback?.Invoke();
    }
}
