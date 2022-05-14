using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dijkstra
{
    public static void Calculate(List<Region> regions)
    {
        // Create a set sptSet (shortest path tree set) that keeps track of vertices included in the shortest-path tree
        List<Region> sptSet = new List<Region>();
        // Assign a distance value to all vertices in the input graph
        Dictionary<Region, float> distanceValue = new Dictionary<Region, float>();
        for(int i = 0; i < regions.Count; i++)
        {
            if (i == 0) // First region always as source vertex
            {
                // Assign distance value as 0 for the source vertex so that it is picked first
                distanceValue.Add(regions[i], 0);
            }
            else
            {
                // Initialize all distance values as INFINITE
                distanceValue.Add(regions[i], Mathf.Infinity);
            }
        }
        // While sptSet doesn’t include all vertices
        while (sptSet.Count < regions.Count)
        {
            //  Pick a vertex U which is not there in sptSet and has a minimum distance value
            Region U = FindMin(sptSet, distanceValue);
            // Include U to sptSet
            sptSet.Add(U);
            // Update distance value of all adjacent vertices of U
            // To update the distance values, iterate through all adjacent vertices
            foreach (ConnectionPoint connPoint in U.RegionConnections)
            {
                // For every adjacent vertex V
                Region adjacentVertexV = connPoint.ConnectedRegion;
                // If the sum of distance value of U (from source) ...
                float sum = 0;
                sum += distanceValue[U];
                // ... and weight of edge U-V ...
                sum += connPoint.Distance;
                // ... is less than the distance value of V
                if (sum < distanceValue[adjacentVertexV])
                {
                    // Then update the distance value of V
                    distanceValue[adjacentVertexV] = sum;
                    adjacentVertexV.Parents.Add(U);
                }
            }
        }
    }

    private static Region FindMin(List<Region> sptSet, Dictionary<Region,float> distanceValue)
    {
        float min = Mathf.Infinity;
        Region minRegion = null;
        foreach(Region region in distanceValue.Keys)
        {
            if(distanceValue[region] < min && (!sptSet.Contains(region)))
            {
                min = distanceValue[region];
                minRegion = region;
            }
        }
        return minRegion;
    }
}
