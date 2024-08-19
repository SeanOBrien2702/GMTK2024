using UnityEngine;

public class ScrapBlock : MonoBehaviour {
    public BlockBoardManager board { get; private set; }
    public FoodBlockData data { get; private set; }
    public Vector3Int position { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    public void Initialize(BlockBoardManager board, Vector3Int position, FoodBlockData data) {
        this.board = board;
        this.position = position;
        this.data = data;

        stepTime = Time.time + stepDelay;
        lockTime = 0f;
    }

    private void Update() {
        if (board == null || Time.timeScale == 0) return;

        board.ClearScrapBlock(this);

        lockTime += Time.deltaTime;

        if (Time.time >= stepTime) {
            Step();
        }

        board.SetScrapBlock(this);
    }

    private bool Move(Vector2Int translation) {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool isValid = board.IsValidPosition(newPosition);
        if (isValid) {
            position = newPosition;
            lockTime = 0f;
        }

        return isValid;
    }

    private void Step() {
        stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (lockTime >= lockDelay) {
            Lock();
        }
    }

    private void Lock() {
        board.LockScrapBlock(this);

        board.SpawnScrap();
    }
}
