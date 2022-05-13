using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public List<Vector2Int> TilePositions { get => _tilesPosition; }
    private List<Vector2Int> _tilesPosition = new List<Vector2Int>();
}
