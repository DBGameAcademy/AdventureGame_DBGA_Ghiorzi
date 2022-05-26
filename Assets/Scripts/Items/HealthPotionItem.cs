using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Potion Item", menuName = "Items/Consumable/Create Health Potion Item")]
public class HealthPotionItem : ConsumableItem
{
    public int RestoreHealthAmount;
    public override void Use()
    {
        GameController.Instance.Player.RestoreHealth(RestoreHealthAmount);
        if (GameController.Instance.Player.IsInBattle)
        {
            GameController.Instance.EndTurn();
        }
    }
}
