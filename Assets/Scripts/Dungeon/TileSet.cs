using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tileset", menuName = "Create Tileset")]
public class TileSet : ScriptableObject
{
    public List<TilePrototype> TilePrototypes = new List<TilePrototype>();

    public TilePrototype GetTilePrototype(TilePrototype.eTileID type) 
    {
        // Pick random tile of the same time
        // (I can have multiple tiles of the same type that looks different from each other)
        List<TilePrototype> possibleTiles = new List<TilePrototype> ();

        foreach(TilePrototype tile in possibleTiles)
        {
            if(tile.TileType == type)
            {
                possibleTiles.Add(tile);
            }
        }

        if (possibleTiles.Count == 0)
            Debug.LogError("No tiles for type: " + type);

        return possibleTiles[Random.Range(0, possibleTiles.Count)];
    }
}
