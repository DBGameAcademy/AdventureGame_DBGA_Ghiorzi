using UnityEngine;

public class Tile
{
    private Vector2Int position;

    public void SetPosition(Vector2Int _position)
    {
        position = _position;
    }
    public void SetPosition(int _x, int _y)
    {
        position.x = _x;
        position.y = _y;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }
}