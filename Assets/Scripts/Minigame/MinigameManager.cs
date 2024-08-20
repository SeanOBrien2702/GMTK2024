using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MinigameManager : MonoBehaviour {
    [SerializeField] private BlockBoardManager board;
    [SerializeField] private SpriteRenderer assignedDishSprite;
    [SerializeField] private SpriteRenderer assignedDishContainerSprite;
    [SerializeField] private Sprite assignedDishContainerLarge;
    [SerializeField] private SpriteRenderer plateSprite;
    [SerializeField] private SpriteRenderer draggingSprite;
    [SerializeField] private Canvas promptPlateCanvas;
    [SerializeField] private SpriteRenderer magicMiniboardFoodSprite;
    [SerializeField] private SpriteRenderer magicMiniboardOrbSprite;
    [SerializeField] private SpriteRenderer magicMiniboardFinishedSprite;

    private FoodBlockData assignedDishData;
    private FoodBlockData draggingData;
    private FoodSize draggingSize;
    private FoodBlockData plateData;
    private FoodSize plateSize;

    private FoodBlockData inputFood;

    private void Start() {
        RecipeSO recipe = GameManager.Instance.GetScaledPlateRecipeSO();
        if (recipe == null) EndMinigame();

        inputFood = recipe.FoodBlockData;
        if (inputFood == null) EndMinigame();

        Invoke(nameof(StartGame), 0.1f);
    }

    private void StartGame() {
        board.ChangeDifficulty(GameManager.Instance.Level);
        AssignDish(inputFood);
    }

    private void Update() {
        DragAndDropFood();
    }

    private void DragAndDropFood() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

            foreach (RaycastHit2D hit in hits) {
                if (hit.collider.CompareTag("Finished Mini")) {
                    if (magicMiniboardFinishedSprite.enabled) {
                        draggingData = assignedDishData;
                        draggingSize = FoodSize.Small;

                        draggingSprite.transform.position = worldPoint;
                        draggingSprite.sprite = draggingData.foodSprite;
                        draggingSprite.transform.localScale = new Vector2(0.5f, 0.5f);
                        draggingSprite.enabled = true;
                    }
                    return;
                }
            }

            draggingData = board.GetFoodBlockDataByPosition(worldPoint);

            if (draggingData != null) {
                if (draggingData != assignedDishData && draggingData.blockType != BlockType.Orb) {
                    draggingData = null;
                    return;
                }

                draggingSize = board.GetFoodSizeByPosition(draggingData, worldPoint);
                draggingSprite.transform.position = worldPoint;

                if (draggingData.blockType == BlockType.Orb && draggingSize != FoodSize.Large) return;

                switch (draggingSize) {
                    case FoodSize.Small:
                        draggingSprite.sprite = draggingData.foodSprite;
                        draggingSprite.transform.localScale = new Vector2(0.5f, 0.5f);
                        break;
                    case FoodSize.Normal:
                        draggingSprite.sprite = draggingData.foodSprite;
                        draggingSprite.transform.localScale = new Vector2(1f, 1f);
                        break;
                    case FoodSize.Large:
                        draggingSprite.sprite = draggingData.largeFoodSprite;
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
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

            foreach (RaycastHit2D hit in hits) {
                if (hit) {
                    if (hit.collider.CompareTag("Plate")) {
                        if (draggingSize != FoodSize.Normal) {
                            plateData = draggingData;
                            plateSize = draggingSize;
                            plateSprite.sprite = plateData.foodSprite;
                            switch (plateSize) {
                                case FoodSize.Small:
                                    plateSprite.transform.localPosition = new Vector3(0f, -0.175f, 0f);
                                    plateSprite.transform.localScale = new Vector2(0.25f, 0.25f);
                                    break;
                                case FoodSize.Normal:
                                    plateSprite.transform.localPosition = new Vector3(0f, -0.125f, 0f);
                                    plateSprite.transform.localScale = new Vector2(0.5f, 0.5f);
                                    break;
                                case FoodSize.Large:
                                    plateSprite.transform.localPosition = new Vector3(0f, 0f, 0f);
                                    plateSprite.transform.localScale = new Vector2(1f, 1f);
                                    break;
                            }
                            plateSprite.enabled = true;
                            promptPlateCanvas.gameObject.SetActive(true);
                            Time.timeScale = 0f;
                        }
                    } else if (hit.collider.CompareTag("Magic Miniboard")) {
                        if (magicMiniboardFinishedSprite.enabled == false) {
                            if (draggingData.blockType == BlockType.Food && draggingSize == FoodSize.Normal) {
                                magicMiniboardFoodSprite.sprite = draggingData.foodSprite;
                                magicMiniboardFoodSprite.enabled = true;
                            } else if (draggingData.blockType == BlockType.Orb && draggingSize == FoodSize.Large) {
                                magicMiniboardOrbSprite.sprite = draggingData.largeFoodSprite;
                                magicMiniboardOrbSprite.enabled = true;
                            }

                            if (magicMiniboardFoodSprite.enabled && magicMiniboardOrbSprite.enabled) {
                                magicMiniboardFoodSprite.enabled = false;
                                magicMiniboardOrbSprite.enabled = false;
                                magicMiniboardFinishedSprite.sprite = magicMiniboardFoodSprite.sprite;
                                magicMiniboardFinishedSprite.enabled = true;
                                Instantiate(board.cloudTransformationPrefab, magicMiniboardFinishedSprite.transform.position, Quaternion.identity);
                            }
                        }
                    }
                }
            }

            draggingData = null;
            draggingSprite.enabled = false;
        }
    }

    public void AssignDish(FoodBlockData data) {
        assignedDishData = data;
        assignedDishSprite.sprite = data.foodSprite;
        if (DeliveryController.Instance.GetNeededPlateScale(GameManager.Instance.PlateKitchenObject) == PlateScale.Large) {
            assignedDishContainerSprite.sprite = assignedDishContainerLarge;
        }
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
        board.SpawnScrapDelay();
    }

    public void AcceptPlate() {
        switch (plateSize) {
            case FoodSize.Small:
                GameManager.Instance.PlateKitchenObject.Scale = PlateScale.Small;
                break;
            case FoodSize.Normal:
                GameManager.Instance.PlateKitchenObject.Scale = PlateScale.Medium;
                break;
            case FoodSize.Large:
                GameManager.Instance.PlateKitchenObject.Scale = PlateScale.Large;
                break;
        }

        EndMinigame();
    }

    public void DeclinePlate() {
        plateSprite.enabled = false;
        promptPlateCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void EndMinigame() {
        GameManager.Instance.ToggleMinigameStart(false);
        SceneManager.UnloadSceneAsync("MinigameScene");
    }
}
