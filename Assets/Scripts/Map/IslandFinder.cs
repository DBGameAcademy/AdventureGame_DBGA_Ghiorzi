using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IslandFinder
{
    // check param is used for checking in the matrix true (1) or false (0) - user preference

    /// Check if a given cell (x, y) can be included in DFS
    public static bool IsSafe(bool[,] matrix, int x, int y, bool[,] visited, bool check = true)
    {
        return (x>=0) && (x<matrix.GetLength(0)) && (y>=0) && (y<matrix.GetLength(1)) && (matrix[x,y]==check && !visited[x,y]);
    }

    /// DFS for a 2D boolean matrix. It only considers the 8 neighbours as adjacent vertices
    public static void DFS(bool[,] matrix, int x, int y, bool[,] visited, Region region, bool check = true)
    {
        // These arrays are used to get row and column numbers of 8 neighbors of a given cell
        int[] rowNbr = new int[] { -1, -1, -1, 0,
                                   0, 1, 1, 1 };
        int[] colNbr = new int[] { -1, 0, 1, -1,
                                   1, -1, 0, 1 };

        // Mark this cell as visited
        visited[x, y] = true;
        region.TilePositions.Add(new Vector2Int(x, y));

        // Recur for all connected neighbours
        for (int k = 0; k < 8; ++k)
            if (IsSafe(matrix, x + rowNbr[k], y + colNbr[k], visited, check))
                DFS(matrix, x + rowNbr[k],y + colNbr[k], visited, region,check);
    }

    /// Returns count of islands in a given boolean 2D matrix
    public static int CountIslands(bool[,] matrix, out List<Region> regions, bool check=true)
    {
        // Make a bool array to mark visited cells.
        // Initially all cells are unvisited
        bool[,] visited = new bool[matrix.GetLength(0), matrix.GetLength(1)];

        // Initialize count as 0
        int count = 0;

        regions = new List<Region>();

        // Traverse through the all cells of given matrix
        for (int i = 0; i < matrix.GetLength(0); ++i)
            for (int j = 0; j < matrix.GetLength(1); ++j)
                if (matrix[i, j] == check && !visited[i, j])
                { 
                    Region region = new Region();
                    // If a cell with value 1 is not visited yet, then new island found.
                    // Visit all cells in this island and increment island count
                    DFS(matrix, i, j, visited,region,check);
                    region.RegionIndex = count;
                    if(!(region.TilePositions.Count < 3))
                    {
                        regions.Add(region);
                        ++count;
                    }
                }

        return count;
    }
}
