using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : CharacterObject
{    
    public int MaxHealth { get=>maxHealth;}
    public int CurrentHealth { get=>currentHealth;}

    public bool IsInBattle { get; set; }

    protected int maxHealth;
    protected int currentHealth;

    public virtual void Damage(int damage)
    {
        int totalDamage = damage;

        if (damage >= currentHealth)
        {
            totalDamage = currentHealth;
        }

        UIController.Instance.ShowDamageTag(transform.position, totalDamage);
        currentHealth -= totalDamage;

        if (currentHealth <= 0)
        {
            OnKill();
        }
    }

    protected virtual void OnKill()
    {
        Destroy(gameObject);
    }

    // ...
}
