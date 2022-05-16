using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : CharacterObject
{
    protected int maxHealth;
    protected int currentHealth;
    protected bool isInBattle = false;

    protected virtual void OnKill()
    {

    }

    // ...
}
