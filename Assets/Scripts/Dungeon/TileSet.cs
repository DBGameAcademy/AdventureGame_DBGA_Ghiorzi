using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tileset", menuName = "Create Tileset")]
public class TileSet : ScriptableObject
{
    public List<GameObject> tilesPrefab = new List<GameObject>();
 
    public GameObject GetTilePrefab(Tile tile) 
    {
        // Pick random tile of the same time
        // (I can have multiple tiles of the same type that looks different from each other)
        List<GameObject> possiblePrefabs = new List<GameObject> ();

        foreach (GameObject prefab in tilesPrefab) {

            if (prefab.GetComponent<Tile>().GetType() == tile.GetType())
            {
                possiblePrefabs.Add(prefab);
            }
        }

        if (possiblePrefabs.Count == 0)
            Debug.LogError("No tiles for type: " + tile.GetType());

        return possiblePrefabs[Random.Range(0, possiblePrefabs.Count)];
    }
}
