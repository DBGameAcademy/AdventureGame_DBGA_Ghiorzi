using UnityEngine;
using System.Collections.Generic;

public abstract class Tile : MonoBehaviour
{
    public GameObject Prefab { get; set; }
    public Vector2Int Position { get; set; }
    public GameObject TileObj { get; set; }

    public bool IsWalkable = true;

    public CharacterObject CharacterObject { get; private set; }

    public void SetCharacterObject(CharacterObject obj)
    {
        CharacterObject = obj;
        IsWalkable = false;
    }

    public void UnsetCharacterObject()
    {
        CharacterObject = null;
        IsWalkable = true;
    }

    public abstract void EnterTile();
}