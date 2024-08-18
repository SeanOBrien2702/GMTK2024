using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockBoardManager : MonoBehaviour {
    public Tilemap tilemap { get; private set; }
    public FoodBlock activeBlock { get; private set; }

    private FoodBlockData[] _foodBlocksData;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;

    public RectInt bounds {
        get {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Start() {
        LoadFoodBlocks();
        SetComponents();

        SpawnBlock();
    }

    private void LoadFoodBlocks() {
        object[] loadedFoodBlocks = Resources.LoadAll("FoodBlocks", typeof(FoodBlockData));
        _foodBlocksData = new FoodBlockData[loadedFoodBlocks.Length];
        loadedFoodBlocks.CopyTo(_foodBlocksData, 0);
    }

    private void SetComponents() {
        tilemap = GetComponentInChildren<Tilemap>();
        activeBlock = GetComponentInChildren<FoodBlock>();
    }

    public void SpawnBlock() {
        int randomBlockIndex = Random.Range(0, _foodBlocksData.Length);
        FoodBlockData data = _foodBlocksData[randomBlockIndex];

        activeBlock.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activeBlock.position)) {
            SetBlock(activeBlock);
        } else {
            // Game over
            tilemap.ClearAllTiles();
        }
    }

    public void SetBlock(FoodBlock block) {
        tilemap.SetTile(block.position, block.data.foodTile);
    }

    public void ClearBlock(FoodBlock block) {
        tilemap.SetTile(block.position, null);
    }

    public bool IsValidPosition(Vector3Int position) {
        if (!bounds.Contains((Vector2Int)position)) return false;
        if (tilemap.HasTile(position)) return false;
        
        return true;
    }

    public void EnlargeBlocks(FoodBlock block) {
        Tile foodTile = block.data.foodTile;

        // Check below the initial block
        if (IsCorrectBlock(block.position + Vector3Int.down, foodTile)) {
            // Check left of the initial block
            if (IsCorrectBlock(block.position + Vector3Int.left, foodTile)) {
                // Check bottom left of the initial block
                if (IsCorrectBlock(block.position + Vector3Int.down + Vector3Int.left, foodTile)) {
                    tilemap.SetTile(block.position, block.data.largeTopRightTile);
                    tilemap.SetTile(block.position + Vector3Int.down, block.data.largeBottomRightTile);
                    tilemap.SetTile(block.position + Vector3Int.left, block.data.largeTopLeftTile);
                    tilemap.SetTile(block.position + Vector3Int.down + Vector3Int.left, block.data.largeBottomLeftTile);
                    return;
                }
            }

            // Check right of the initial block
            if (IsCorrectBlock(block.position + Vector3Int.right, foodTile)) {
                // Check bottom right of the initial block
                if (IsCorrectBlock(block.position + Vector3Int.down + Vector3Int.right, foodTile)) {
                    tilemap.SetTile(block.position, block.data.largeTopLeftTile);
                    tilemap.SetTile(block.position + Vector3Int.down, block.data.largeBottomLeftTile);
                    tilemap.SetTile(block.position + Vector3Int.right, block.data.largeTopRightTile);
                    tilemap.SetTile(block.position + Vector3Int.down + Vector3Int.right, block.data.largeBottomRightTile);
                    return;
                }
            }
        }

        // Check above the initial block
        if (IsCorrectBlock(block.position + Vector3Int.up, foodTile)) {
            // Check left of the initial block
            if (IsCorrectBlock(block.position + Vector3Int.left, foodTile)) {
                // Check top left of the initial block
                if (IsCorrectBlock(block.position + Vector3Int.up + Vector3Int.left, foodTile)) {
                    tilemap.SetTile(block.position, block.data.largeBottomRightTile);
                    tilemap.SetTile(block.position + Vector3Int.up, block.data.largeTopRightTile);
                    tilemap.SetTile(block.position + Vector3Int.left, block.data.largeBottomLeftTile);
                    tilemap.SetTile(block.position + Vector3Int.up + Vector3Int.left, block.data.largeTopLeftTile);
                    return;
                }
            }

            // Check right of the initial block
            if (IsCorrectBlock(block.position + Vector3Int.right, foodTile)) {
                // Check top right of the initial block
                if (IsCorrectBlock(block.position + Vector3Int.up + Vector3Int.right, foodTile)) {
                    tilemap.SetTile(block.position, block.data.largeBottomLeftTile);
                    tilemap.SetTile(block.position + Vector3Int.up, block.data.largeTopLeftTile);
                    tilemap.SetTile(block.position + Vector3Int.right, block.data.largeBottomRightTile);
                    tilemap.SetTile(block.position + Vector3Int.up + Vector3Int.right, block.data.largeTopRightTile);
                    return;
                }
            }
        }
    }

    private bool IsCorrectBlock(Vector3Int position, Tile tile) {
        if (!bounds.Contains((Vector2Int)position)) return false;
        if (!tilemap.HasTile(position)) return false;
        if (tilemap.GetTile(position) != tile) return false;

        return true;
    }
}
