using UnityEngine;

public class FoodBlock : MonoBehaviour {
    public BlockBoardManager board { get; private set; }
    public FoodBlockData data { get; private set; }
    public Vector3Int position { get; private set; }

    public float stepDelay = 1f;
    public float stepDelayDecrease = 0.1f;
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

    public void IncreaseDifficulty(int level) {
        stepDelay -= Mathf.Max(level * stepDelayDecrease, 0f);
    }

    private void Update() {
        if (board == null || Time.timeScale == 0) return;

        board.ClearBlock(this);

        lockTime += Time.deltaTime;

        MoveInput();

        if (Time.time >= stepTime) {
            Step();
        }

        board.SetBlock(this);
    }

    private void MoveInput() {
        // Move the block left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(Vector2Int.left);
        }

        // Move the block right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(Vector2Int.right);
        }

        // Move the block down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            Move(Vector2Int.down);
        }

        // Hard drop the block
        if (Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }
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

    private void HardDrop() {
        while (Move(Vector2Int.down)) continue;

        Lock();
    }

    private void Step() {
        stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (lockTime >= lockDelay) {
            Lock();
        }
    }

    private void Lock() {
        board.SetBlock(this);
        if (data.blockType == BlockType.Food || data.blockType == BlockType.Orb) {
            board.EnlargeBlocks(this);
        }

        board.SpawnBlock();
    }
}
