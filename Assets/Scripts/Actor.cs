using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : CharacterObject
{    
    public bool IsInBattle { get; set; }

    protected int maxHealth;
    protected int currentHealth;

    public void Damage(int damage)
    {
        Debug.Log(this.gameObject.name + " damged with " + damage);
    }

    protected virtual void OnKill()
    {

    }

    // ...
}
