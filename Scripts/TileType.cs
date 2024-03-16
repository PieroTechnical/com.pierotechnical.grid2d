using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewTileType", menuName = "Grid/TileType")]
public class TileType : ScriptableObject
{
    public string tileID;
    public string displayName;
    public Sprite sprite;
    public bool walkable;
    public GameObject prefab;
    [FormerlySerializedAs("seethrough")] public bool transparent;
    [Range(1,5)] public float cost = 1;
    public Color color;
}
