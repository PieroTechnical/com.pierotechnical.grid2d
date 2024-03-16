using Grid;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Grid.Pathing.PathfinderHelpers2D;

public class Pathfinder2DNaive : AbstractPathfinder
{
    public override List<Vector3> FindPath(TileMap level, Vector2Int startPos, Vector2Int targetPos, float groundDistance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals)
    {
        var path = new List<Vector3>();
        var fromTile = new Dictionary<Vector2Int, Vector2Int>();  // For each tile we explored, we store the position of the last tile and the cost
        var queue = new Queue<(Vector2Int, float)>();
        var visited = new HashSet<Vector2Int>();

        queue.Enqueue((startPos, 0));
        visited.Add(startPos);

        while (queue.Count > 0)
        {
            (Vector2Int current, float currentCost) = queue.Dequeue();
            foreach (var dir in allowDiagonals ? AllDirections : Cardinals)
            {
                var neighbor = current + dir;

                if (distanceFormula < 0) distanceFormula = level.defaultDistanceFormula;

                float directionalCost = dir.magnitude > 1 ? DistanceFormulaDiagonalCost[(int)distanceFormula] : 1;

                float neighborCost = currentCost + level.Level[neighbor].cost * directionalCost;

                if (level.IsValidTile(neighbor, visited, groundDistance, currentCost, directionalCost))
                {
                    queue.Enqueue((neighbor, neighborCost));
                    visited.Add(neighbor);
                    fromTile.Add(neighbor, current);

                    // If we found the target, make a path back to the start
                    if (neighbor == targetPos)
                    {

                        Vector2Int currentTile = targetPos;

                        while (currentTile != startPos)
                        {
                            path.Add((Vector2)currentTile);
                            currentTile = fromTile[currentTile];
                        }

                        return path;
                    }
                }
            }
        }

        return new List<Vector3>();
    }

    public override HashSet<Vector3> GetLegalMoves(TileMap tilemap, Vector2Int position, int distance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals)
    {
        var legalMovesWithCost = GetLegalMovesWithCost(tilemap, position, distance);
        var legalMoves = legalMovesWithCost.Keys.ToList();

        return new(legalMoves);
    }

    public override Dictionary<Vector3, float> GetLegalMovesWithCost(TileMap tilemap, Vector2Int startPos, float groundDistance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals)
    {
        var availableTiles = new Dictionary<Vector3, float>();
        var queue = new Queue<(Vector2Int, float)>();
        var visited = new HashSet<Vector2Int>();

        queue.Enqueue((startPos, 0));
        visited.Add(startPos);


        while (queue.Count > 0)
        {
            (Vector2Int current, float currentCost) = queue.Dequeue();
            foreach (var dir in allowDiagonals ? AllDirections : Cardinals)
            {
                var neighbor = current + dir;

                if (distanceFormula < 0) distanceFormula = tilemap.defaultDistanceFormula;

                float directionalCost = dir.magnitude > 1 ? DistanceFormulaDiagonalCost[(int)distanceFormula] : 1;

                if (tilemap.IsValidTile(neighbor, visited, groundDistance, currentCost, directionalCost))
                {
                    float neighborCost = currentCost + tilemap.Level[neighbor].cost * directionalCost;
                    queue.Enqueue((neighbor, neighborCost));
                    visited.Add(neighbor);
                    availableTiles[(Vector2)neighbor] = neighborCost;
                }
            }
        }

        return availableTiles;
    }
}
