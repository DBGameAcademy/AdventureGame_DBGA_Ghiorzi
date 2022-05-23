using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public abstract string Name { get; }
    // Add other useful common attributes here ...
    protected Monster _monster;

    public abstract void Process();
    public void SetMonster(Monster monster)
    {
        _monster = monster;
    }
}
