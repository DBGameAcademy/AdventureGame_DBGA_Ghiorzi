using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster MoveSet", menuName = "Create MoveSet")]
public class MoveSet : ScriptableObject
{
    public List<string> MoveNames = new List<string>();

    public Move GetRandomMove()
    {
        int randomIndex = Random.Range(0, MoveNames.Count);
        return MoveFactory.GetAbility(MoveNames[randomIndex]);
    }
}
