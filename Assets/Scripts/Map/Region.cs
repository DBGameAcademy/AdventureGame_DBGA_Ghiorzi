using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ConnectionPoint
{
    public Vector2Int FirstTilePoistion;
    public Vector2Int SecondTilePosition;
    public float Distance;
    public Region ConnectedRegion;
}

public class Region
{
    public int RegionIndex { get; set; }
    public List<Region> Parents { get => _parents; }

    public List<Vector2Int> TilePositions { get => _tilesPosition; }
    public List<ConnectionPoint> RegionConnections { get => _regionConnections; }
    
    private List<Vector2Int> _tilesPosition = new List<Vector2Int>();
    private List<ConnectionPoint> _regionConnections = new List<ConnectionPoint>();
    private List<Region> _parents = new List<Region>();

    public ConnectionPoint FindClosestPointToRegion(Region region)
    {
        float minDistance = Vector2Int.Distance(_tilesPosition[0], region.TilePositions[0]);

        ConnectionPoint closestPoint;
        closestPoint.Distance = 0;
        closestPoint.FirstTilePoistion = Vector2Int.zero;
        closestPoint.SecondTilePosition = Vector2Int.zero;
        closestPoint.ConnectedRegion = region;

        for(int i = 0; i < _tilesPosition.Count; ++i)
        {
            for(int j=0; j < region.TilePositions.Count; ++j)
            {
                float dist = Vector2Int.Distance(_tilesPosition[i], region.TilePositions[j]);
                if ( dist < minDistance)
                {
                    minDistance = dist;
                    closestPoint.Distance = minDistance;
                    closestPoint.FirstTilePoistion = _tilesPosition[i];
                    closestPoint.SecondTilePosition = region.TilePositions[j];
                    closestPoint.ConnectedRegion = region;
                }
            }
        }
        return closestPoint;
    }



    // NOPE
    public void RemoveToMinConnection()
    {
        float minDistance = _regionConnections[0].Distance;

        ConnectionPoint point;
        point.Distance = minDistance;
        point.FirstTilePoistion = _regionConnections[0].FirstTilePoistion;
        point.SecondTilePosition = _regionConnections[0].SecondTilePosition;

        // Average
        float sum = 0;
        float avg = 0;
        for (int i = 0; i < _regionConnections.Count; ++i)
        {
            sum+=_regionConnections[i].Distance;
        }
        avg = sum/_regionConnections.Count;
        List<ConnectionPoint> points = new List<ConnectionPoint>();
        for (int i=1; i<_regionConnections.Count; ++i)
        {
            if(_regionConnections[i].Distance < avg)
            {
                //minDistance = _regionConnections[i].Distance;
                //point.Distance = minDistance;
                //point.FirstTilePoistion = _regionConnections[i].FirstTilePoistion;
                //point.SecondTilePosition = _regionConnections[i].SecondTilePosition;
                points.Add(_regionConnections[i]);
            }
        }
        _regionConnections.Clear();
        _regionConnections = points;
    }

}
