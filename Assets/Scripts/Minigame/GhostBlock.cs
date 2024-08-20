using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostBlock : MonoBehaviour {
    public Tile tile;
    public BlockBoardManager board;
    public FoodBlock trackingBlock;

    public Tilemap tilemap { get; private set; }
    public Vector3Int position { get; private set; }

    private void Start() {
        SetComponents();
    }

    private void SetComponents() {
        tilemap = GetComponentInChildren<Tilemap>();
    }

    private void LateUpdate() {
        if (MinigameManager.Instance.isPaused) return;

        ClearBlock();
        if (DropBlock()) {
            SetBlock();
        }
    }

    private void ClearBlock() {
        tilemap.SetTile(position, null);
    }

    private bool DropBlock() {
        Vector3Int position = trackingBlock.position;

        int current = position.y - 1;
        int bottom = -board.boardSize.y / 2 - 1;
        bool doDrop = false;

        for (int row = current; row >= bottom; row--) {
            position.y = row;

            if (board.IsValidPosition(position)) {
                this.position = position;
                doDrop = true;
            } else {
                break;
            }
        }

        return doDrop;
    }

    private void SetBlock() {
        tilemap.SetTile(position, tile);
    }
}
