using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    List<Vector2Int> TilePositions { get => _tilesPosition; }
    List<Vector2Int> _tilesPosition = new List<Vector2Int>();
}
