using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockBoardManager : MonoBehaviour {
    public Tilemap tilemap { get; private set; }
    public FoodBlock activeBlock { get; private set; }

    private FoodBlockData[] foodBlocksData;
    private FoodBlockData[] scrapBlocksData;

    public Vector3Int spawnPosition;
    public Vector2Int boardSize;
    public SpriteRenderer[] upcomingBlockSprites;
    public Tilemap scrapTilemap;
    public ScrapBlock activeScrapBlock;

    private FoodBlockData[] upcomingBlocks;

    public RectInt bounds {
        get {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Start() {
        LoadFoodBlocks();
        SetComponents();
    }

    private void LoadFoodBlocks() {
        object[] loadedFoodBlocks = Resources.LoadAll("FoodBlocks", typeof(FoodBlockData));
        foodBlocksData = new FoodBlockData[loadedFoodBlocks.Length];
        loadedFoodBlocks.CopyTo(foodBlocksData, 0);

        object[] loadedScrapBlocks = Resources.LoadAll("ScrapBlocks", typeof(FoodBlockData));
        scrapBlocksData = new FoodBlockData[loadedScrapBlocks.Length];
        loadedScrapBlocks.CopyTo(scrapBlocksData, 0);

        upcomingBlocks = new FoodBlockData[upcomingBlockSprites.Length];
        for (int i = 0; i < upcomingBlockSprites.Length; i++) {
            upcomingBlocks[i] = GetRandomFoodData();
            upcomingBlockSprites[i].sprite = upcomingBlocks[i].foodSprite;
        }
    }

    public FoodBlockData GetRandomFoodData() {
        int randomBlockIndex = Random.Range(0, foodBlocksData.Length);
        return foodBlocksData[randomBlockIndex];
    }

    public FoodBlockData GetRandomScrapData() {
        int randomBlockIndex = Random.Range(0, scrapBlocksData.Length);
        return scrapBlocksData[randomBlockIndex];
    }

    private void SetComponents() {
        tilemap = GetComponentInChildren<Tilemap>();
        activeBlock = GetComponentInChildren<FoodBlock>();
    }

    public void SpawnBlock() {
        FoodBlockData data = upcomingBlocks[0];
        for (int i = 0; i < upcomingBlocks.Length - 1; i++) {
            upcomingBlocks[i] = upcomingBlocks[i+1];
            upcomingBlockSprites[i].sprite = upcomingBlocks[i].foodSprite;
        }
        upcomingBlocks[upcomingBlocks.Length-1] = GetRandomFoodData();
        upcomingBlockSprites[upcomingBlockSprites.Length-1].sprite = upcomingBlocks[upcomingBlocks.Length-1].foodSprite;

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

    public void SpawnScrap() {
        FoodBlockData data = GetRandomScrapData();
        Vector3Int scrapSpawnPosition = new Vector3Int(Random.Range(bounds.xMin, bounds.xMax), spawnPosition.y, 0);

        //while (scrapSpawnPosition == activeBlock.position) yield return null;
        activeScrapBlock.Initialize(this, scrapSpawnPosition, data);

        if (IsValidPosition(activeScrapBlock.position)) {
            SetScrapBlock(activeScrapBlock);
        } else {
            // Game over
            scrapTilemap.ClearAllTiles();
        }
    }

    public void SetScrapBlock(ScrapBlock block) {
        scrapTilemap.SetTile(block.position, block.data.foodTile);
    }

    public void ClearScrapBlock(ScrapBlock block) {
        scrapTilemap.SetTile(block.position, null);
    }

    public void LockScrapBlock(ScrapBlock block) {
        tilemap.SetTile(block.position, block.data.foodTile);
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

    public FoodBlockData GetFoodBlockDataByPosition(Vector3 position) {
        Vector3Int tPos = tilemap.WorldToCell(position);

        if (tPos == activeBlock.position) return null;

        Tile tile = tilemap.GetTile<Tile>(tPos);

        foreach (FoodBlockData data in foodBlocksData) {
            if (data.foodTile != null & tile == data.foodTile) return data;
            if (data.largeBottomLeftTile != null &tile == data.largeBottomLeftTile) return data;
            if (data.largeBottomRightTile != null &tile == data.largeBottomRightTile) return data;
            if (data.largeTopLeftTile != null &tile == data.largeTopLeftTile) return data;
            if (data.largeTopRightTile != null &tile == data.largeTopRightTile) return data;
        }

        return null;
    }

    public FoodSize GetFoodSizeByPosition(FoodBlockData data, Vector3 position) {
        Vector3Int tPos = tilemap.WorldToCell(position);

        if (tPos == activeBlock.position) return FoodSize.Normal;

        Tile tile = tilemap.GetTile<Tile>(tPos);

        if (data.foodTile != null & tile == data.foodTile) return FoodSize.Normal;
        if (data.largeBottomLeftTile != null &tile == data.largeBottomLeftTile) return FoodSize.Large;
        if (data.largeBottomRightTile != null &tile == data.largeBottomRightTile) return FoodSize.Large;
        if (data.largeTopLeftTile != null &tile == data.largeTopLeftTile) return FoodSize.Large;
        if (data.largeTopRightTile != null &tile == data.largeTopRightTile) return FoodSize.Large;

        return FoodSize.Normal;
    }
}
