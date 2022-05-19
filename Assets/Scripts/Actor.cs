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

    public void Damage(int damage)
    {
        int totalDamage = damage;

        if (damage >= currentHealth)
        {
            totalDamage = currentHealth;
        }

        currentHealth -= totalDamage;

        if (currentHealth <= 0)
        {
            OnKill();
        }

        Debug.Log(this.gameObject.name + " damaged with " + damage+" current health "+currentHealth);
    }

    protected virtual void OnKill()
    {
        Debug.Log(this.gameObject.name + " DEAD");
        Destroy(gameObject);
    }

    // ...
}
