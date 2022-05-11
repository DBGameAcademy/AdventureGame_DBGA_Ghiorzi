using UnityEngine;

public class Floor : MonoBehaviour
{
    public Room[,] Rooms { get; set; }
    public FloorTransition FloorUpTransition { get; set; }
    public FloorTransition FloorDownTransition { get; set; }
}