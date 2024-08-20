using UnityEngine;
using UnityEngine.Tilemaps;

public enum FoodSize {
    None,
    Small,
    Normal,
    Large
}

public enum BlockType {
    None,
    Food,
    Scrap,
    Orb
}

[CreateAssetMenu(fileName = "Food Block", menuName = "Scriptable Objects/Food Block")]
public class FoodBlockData : ScriptableObject {
    public string foodName;

    public Sprite foodSprite;
    public Sprite largeFoodSprite;

    public Tile foodTile;
    public Tile largeBottomLeftTile;
    public Tile largeBottomRightTile;
    public Tile largeTopLeftTile;
    public Tile largeTopRightTile;

    public BlockType blockType;
}
