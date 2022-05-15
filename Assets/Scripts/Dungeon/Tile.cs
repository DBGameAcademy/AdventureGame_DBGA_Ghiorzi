using UnityEngine;
using System.Collections.Generic;

public abstract class Tile : MonoBehaviour
{
    public GameObject Prefab { get; set; }
    public Vector2Int Position { get; set; }
    public GameObject TileObj { get; set; }

    public bool IsWalkable = true;

    public abstract void EnterTile();
}