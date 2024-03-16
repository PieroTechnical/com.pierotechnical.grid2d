using Grid;
using Grid.Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Grid.Pathing.PathfinderHelpers2D;
using static Grid.GridMath;

public class Pathfinder2DAStar : Pathfinder2DNaive
{
    public override List<Vector3> FindPath(TileMap level, Vector2Int startPos, Vector2Int targetPos, float groundDistance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals)
    {
        var openSet = new SimplePriorityQueue<Vector2Int>(); // Priority queue based on F cost
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>(); // For path reconstruction
        var gScore = new Dictionary<Vector2Int, float>(); // Cost from start to a node
        var fScore = new Dictionary<Vector2Int, float>(); // Total cost (gScore + heuristic)

        gScore[startPos] = 0;
        fScore[startPos] = CalculateDistance(startPos, targetPos, distanceFormula);
        openSet.Enqueue(startPos, fScore[startPos]);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == targetPos)
                return ReconstructPath(cameFrom, current, level.yAxis); // Found the path

            foreach (var neighbor in GetNeighbors(current, level, allowDiagonals))
            {
                // Tentative G Score for this neighbor
                var tentativeGScore = gScore[current] + CalculateDistance(current, neighbor, distanceFormula);
                if (tentativeGScore < gScore.GetValueOrDefault(neighbor, float.MaxValue))
                {
                    // This path to neighbor is better than any previous one. Record it!
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + CalculateDistance(neighbor, targetPos, distanceFormula);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }

        return new List<Vector3>(); // Path not found
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node, TileMap level, bool allowDiagonals)
    {
        var neighbors = new List<Vector2Int>();

        foreach (var direction in allowDiagonals ? AllDirections : Cardinals)
        {
            var neighbor = node + direction;
            if (level.IsValidTile(neighbor))
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current, bool yAxis = false)
    {
        var totalPath = new List<Vector3>();
        while (cameFrom.ContainsKey(current))
        {
            Vector3 pointInPath;

            if(yAxis)
            {
                pointInPath = new(current.x, 0, current.y);
            }

            else
            {
                pointInPath = (Vector2)current;
            }

            totalPath.Add(pointInPath);

            current = cameFrom[current];
        }
        return totalPath;
    }
}
