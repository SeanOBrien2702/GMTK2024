using UnityEngine;
using UnityEngine.Tilemaps;

public class MinigameManager : MonoBehaviour {
    [SerializeField] private BlockBoardManager board;
    [SerializeField] private SpriteRenderer assignedDishSprite;
    [SerializeField] private SpriteRenderer plateSprite;
    [SerializeField] private SpriteRenderer draggingSprite;
    [SerializeField] private Canvas promptPlateCanvas;

    private FoodBlockData assignedDishData;
    private FoodSize assignedDishSize = FoodSize.Normal;
    private FoodBlockData draggingData;
    private FoodSize draggingSize;
    private FoodBlockData plateData;
    private FoodSize plateSize;

    private void Start() {
        Invoke(nameof(StartGame), 1f);
    }

    private void StartGame() {
        // TODO: Assign the dish through the main game
        FoodBlockData data = board.GetRandomFoodData();
        while (data.isScrap) {
            data = board.GetRandomFoodData();
        }
        AssignDish(data);
    }

    private void Update() {
        DragAndDropFood();
    }

    private void DragAndDropFood() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;
            draggingData = board.GetFoodBlockDataByPosition(worldPoint);
            if (draggingData != assignedDishData) draggingData = null;

            if (draggingData != null) {
                draggingSize = board.GetFoodSizeByPosition(draggingData, worldPoint);
                draggingSprite.transform.position = worldPoint;
                draggingSprite.sprite = draggingData.foodSprite;
                switch (draggingSize) {
                    case FoodSize.Small:
                        draggingSprite.transform.localScale = new Vector2(0.5f, 0.5f);
                        break;
                    case FoodSize.Normal:
                        draggingSprite.transform.localScale = new Vector2(1f, 1f);
                        break;
                    case FoodSize.Large:
                        draggingSprite.transform.localScale = new Vector2(2f, 2f);
                        break;
                }
                draggingSprite.enabled = true;
            }
        } else if (Input.GetMouseButton(0)) {
            if (draggingData == null) return;

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;
            draggingSprite.transform.position = worldPoint;
        } else if (Input.GetMouseButtonUp(0)) {
            if (draggingData == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hits = Physics2D.GetRayIntersection(ray);

            if (hits && hits.collider.CompareTag("Plate")) {
                plateData = draggingData;
                plateSize = draggingSize;
                plateSprite.sprite = plateData.foodSprite;
                switch (plateSize) {
                    case FoodSize.Small:
                        plateSprite.transform.localScale = new Vector2(0.25f, 0.25f);
                        break;
                    case FoodSize.Normal:
                        plateSprite.transform.localScale = new Vector2(0.5f, 0.5f);
                        break;
                    case FoodSize.Large:
                        plateSprite.transform.localScale = new Vector2(1f, 1f);
                        break;
                }
                plateSprite.enabled = true;
                promptPlateCanvas.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }

            draggingData = null;
            draggingSprite.enabled = false;
        }
    }

    public void AssignDish(FoodBlockData data) {
        assignedDishData = data;
        assignedDishSprite.sprite = data.foodSprite;
        switch (draggingSize) {
            case FoodSize.Small:
                assignedDishSprite.transform.localScale = new Vector2(0.25f, 0.25f);
                break;
            case FoodSize.Normal:
                assignedDishSprite.transform.localScale = new Vector2(0.5f, 0.5f);
                break;
            case FoodSize.Large:
                assignedDishSprite.transform.localScale = new Vector2(1f, 1f);
                break;
        }
        board.SpawnBlock();
    }

    public void AcceptPlate() {
        // TODO: Tell the main game what dish has been made
        Debug.Log($"{plateSize} {plateData.foodName} has been made!");
    }

    public void DeclinePlate() {
        plateSprite.enabled = false;
        promptPlateCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
