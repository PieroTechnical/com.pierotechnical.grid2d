using Grid;
using System.Collections.Generic;
using UnityEngine;
using static Grid.Pathing.PathfinderHelpers2D;

namespace Pathfinding
{
    public static class LineOfSight2D
    {
        public static HashSet<Vector2Int> GetVisibleTiles(this TileMap tilemap, Vector2Int position, int distance, GridMath.DistanceFormula distanceFormula)
        {
            var availableTiles = tilemap.GetLevelSubsection(position, Mathf.RoundToInt(distance), distanceFormula);

            var visibleTiles = new List<Vector2Int>();

            foreach (var tile in availableTiles)
            {
                if (!tilemap.IsDirectPathObstructed(position, tile))
                {
                    visibleTiles.Add(tile);
                }
            }

            return new HashSet<Vector2Int>(visibleTiles);
        }

        public static HashSet<Vector2Int> GetVisibleTiles(this TileMap tilemap, Vector2Int position, int distance) => GetVisibleTiles(tilemap, position, distance, tilemap.defaultDistanceFormula);

        public static bool IsDirectPathObstructed(this TileMap level, Vector2Int startPoint, Vector2Int endPoint, bool ignoreEnds = true)
        {
            Vector2Int currentTile = startPoint;

            // Calculate differences and steps
            int deltaX = Mathf.Abs(endPoint.x - startPoint.x);
            int deltaY = Mathf.Abs(endPoint.y - startPoint.y);
            int stepX = startPoint.x < endPoint.x ? 1 : -1;
            int stepY = startPoint.y < endPoint.y ? 1 : -1;

            int err = deltaX - deltaY;

            while (true)
            {
                if (!ignoreEnds || (currentTile != startPoint && currentTile != endPoint))
                {
                    if (!level.IsTileTransparent(currentTile))
                    {
                        return true;
                    }
                }

                if (currentTile == endPoint) break;

                int e2 = 2 * err;
                if (e2 > -deltaY)
                {
                    err -= deltaY;
                    currentTile.x += stepX;
                }
                if (e2 < deltaX)
                {
                    err += deltaX;
                    currentTile.y += stepY;
                }
            }

            return false;
        }

    }
}
