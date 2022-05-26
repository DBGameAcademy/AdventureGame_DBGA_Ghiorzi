using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionItem : ConsumableItem
{
    public int RestoreHealthAmount;
    public GameObject HealParticle;

    public override void Use()
    {
        GameController.Instance.Player.RestoreHealth(RestoreHealthAmount);

        UIController.Instance.HideInventory();

        GameObject particle = Instantiate(HealParticle, GameController.Instance.Player.transform);
        particle.GetComponent<ParticleSystem>().Play();

        if (GameController.Instance.Player.IsInBattle)
        {
            GameController.Instance.EndTurn();
        }
    }
}
