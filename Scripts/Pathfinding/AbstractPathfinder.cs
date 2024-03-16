using System.Collections.Generic;
using UnityEngine;
using Grid;

namespace Pathfinding
{
    public abstract class AbstractPathfinder
    {
        public abstract List<Vector3> FindPath(TileMap tilemap, Vector2Int startPos, Vector2Int targetPos, float groundDistance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals);

        public virtual List<Vector3> FindPath(TileMap tilemap, Vector2Int startPos, Vector2Int targetPos, float groundDistance) => FindPath(tilemap, startPos, targetPos, groundDistance, tilemap.defaultDistanceFormula, tilemap.allowDiagonals);

        public abstract HashSet<Vector3> GetLegalMoves(TileMap tilemap, Vector2Int position, int distance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals);

        public virtual HashSet<Vector3> GetLegalMoves(TileMap tilemap, Vector2Int position, int distance) => GetLegalMoves(tilemap, position, distance, tilemap.defaultDistanceFormula, tilemap.allowDiagonals);

        public abstract Dictionary<Vector3, float> GetLegalMovesWithCost(TileMap tilemap, Vector2Int startPos, float groundDistance, GridMath.DistanceFormula distanceFormula, bool allowDiagonals);
        
        public virtual Dictionary<Vector3, float> GetLegalMovesWithCost(TileMap tilemap, Vector2Int startPos, float groundDistance) => GetLegalMovesWithCost(tilemap, startPos, groundDistance, tilemap.defaultDistanceFormula, tilemap.allowDiagonals);
    }
}
