using UnityEngine;

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
    public eTileID ID { get; private set; }

    public Vector2Int Position { get; set; }
}