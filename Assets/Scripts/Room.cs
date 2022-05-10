using UnityEngine;

public class Room : MonoBehaviour
{
    public Tile[,] Tiles { get; set; }
    public Vector2Int RoomPosition { get; set;}
    public Vector2Int Size { get{ return new Vector2Int(Tiles.GetLength(0), Tiles.GetLength(1)); } }
}