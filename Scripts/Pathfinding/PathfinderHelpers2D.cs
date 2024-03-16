using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Grid.Pathing
{
    public static class PathfinderHelpers2D
    {
        public static readonly Vector2Int[] Cardinals = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        public static readonly Vector2Int[] Diagonals = { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) };
        public static readonly Vector2Int[] AllDirections = Cardinals.Concat(Diagonals).ToArray();

        public static readonly float[] DistanceFormulaDiagonalCost = new float[] { 2, 1.41421356237f, 1 };


        public static HashSet<Vector2Int> GetLevelSubsection(this TileMap tilemap, Vector2Int position, int maxDistance, GridMath.DistanceFormula distanceFormula)
        {
            HashSet<Vector2Int> section = new();

            for (int x = -maxDistance; x <= maxDistance; x++)
            {
                for (int y = -maxDistance; y <= maxDistance; y++)
                {
                    if (tilemap.Level.ContainsKey(new(x, y)) && GridMath.CalculateDistance(new(0, 0), new(x, y), distanceFormula) <= maxDistance)
                    {
                        section.Add(position + new Vector2Int(x, y));
                    }
                }
            }

            return section;
        }

        public static bool IsTileObstructed(this TileMap level, Vector2Int currentTile)
        {

            if (level.Level.ContainsKey(currentTile) && !level.Level[currentTile].walkable)
            {
                return true;
            }

            return false;
        }

        
        public static bool IsTileTransparent(this TileMap level, Vector2Int currentTile)
        {
            return !level.Level.ContainsKey(currentTile) || level.Level[currentTile].transparent;
        }





        public static bool IsValidTile(this TileMap level, Vector2Int tile, HashSet<Vector2Int> visited, float groundDistance, float currentCost, float additionalCost)
        {
            if (visited.Contains(tile)) return false;

            if (level.Level.TryGetValue(tile, out TileType tileType))
            {
                return tileType.walkable && currentCost + tileType.cost * additionalCost <= groundDistance;
            }

            return false;
        }

        public static bool IsValidTile(this TileMap level, Vector2Int tile)
        {
            if (level.Level.TryGetValue(tile, out TileType tileType))
            {
                return tileType.walkable;
            }
            return false;
        }

        public static bool IsValidTile(this TileMap level, Vector2Int tile, float groundDistance)
        {
            if (level.Level.TryGetValue(tile, out TileType tileType))
            {
                return tileType.walkable && tileType.cost <= groundDistance;
            }
            return false;
        }

        public static bool IsValidTile(this TileMap level, Vector2Int tile, HashSet<Vector2Int> visited)
        {
            if (visited.Contains(tile)) return false;

            if (level.Level.TryGetValue(tile, out TileType tileType))
            {
                return tileType.walkable;
            }

            return false;
        }

        
    }
}
