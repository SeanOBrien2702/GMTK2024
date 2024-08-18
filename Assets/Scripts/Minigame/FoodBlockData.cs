using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Food Block", menuName = "Scriptable Objects/Food Block")]
public class FoodBlockData : ScriptableObject {
    public string foodName;
    public Tile foodTile;
    public Tile largeBottomLeftTile;
    public Tile largeBottomRightTile;
    public Tile largeTopLeftTile;
    public Tile largeTopRightTile;
    public bool isScrap;
}
