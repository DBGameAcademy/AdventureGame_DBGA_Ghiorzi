using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMonsterID
{
    Slime,
    Skeleton,
}

[CreateAssetMenu(fileName = "New Monster", menuName = "Create Monster")]
public class MonsterData : ScriptableObject
{
    public eMonsterID ID;
    public int Damage;
    public int Health;
    public GameObject Prefab;
    public MoveSet MoveSet;
}
