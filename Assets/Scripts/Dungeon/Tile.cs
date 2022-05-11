using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public enum eTileID
    {
        Empty,
        DoorUp,
        DoorDown,
        DoorLeft,
        DoorRight,
        FloorUp,
        FloorDown,
    }
    public eTileID ID { get; set; }
    public Vector2Int Position { get; set; }
    public List<GameObject> TileObjects { get { return _tileObjects; } set { _tileObjects = value; } }
    
    private List<GameObject> _tileObjects = new List<GameObject>();
}