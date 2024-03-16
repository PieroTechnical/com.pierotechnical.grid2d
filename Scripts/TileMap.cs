using Grid;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLevel", menuName = "Grid/Level")]
public class TileMap : ScriptableObject
{
    public bool yAxis;

    public GridMath.DistanceFormula defaultDistanceFormula;
    public bool allowDiagonals = true;

    Dictionary<Vector2Int, TileType> level = new();

    public Dictionary<Vector2Int, TileType> Level { get { return level; } }


    public TileType[] availableTileTypes;

    public void SetTile(Vector2Int position, int index)
    {
        SetTile(position, availableTileTypes[index]);
    }

    public void SetTile(Vector2Int position, TileType tileType)
    {
        level[position] = tileType;
    }

    public void FillRect(int x1, int y1, int width, int height, TileType tileToFill)
    {
        for (int x = x1; x < x1 + width; x++)
        {
            for (int y = y1; y < y1 + height; y++)
            {
                Vector2Int coord = new(x, y);
                SetTile(coord, tileToFill);
            }
        }
    }
}
