using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Potion Item", menuName = "Items/Consumable/Create Health Potion Item")]
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
