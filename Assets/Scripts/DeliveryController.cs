using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    public event EventHandler<OnRecipeSpawnedEventArgs> OnRecipeSpawned;
    public event EventHandler<OnRecipeCompletedEventArgs> OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public class OnRecipeSpawnedEventArgs : EventArgs
    {
        public RecipeSO RecipeSO;
    }

    public class OnRecipeCompletedEventArgs : EventArgs
    {
        public RecipeSO RecipeSO;
    }

    public static DeliveryController Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 3f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (!GameManager.Instance.IsPlayingMinigame && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                //Debug.Log(waitingRecipeSOList.Count + " " + waitingRecipesMax);
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);

                SpawnNewWaitingRecipe(waitingRecipeSOIndex);
            }
        }
    }

    private void SpawnNewWaitingRecipe(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[waitingRecipeSOIndex];

        waitingRecipeSOList.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, new OnRecipeSpawnedEventArgs
        {
            RecipeSO = waitingRecipeSO
        });
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList)
                {
                    // Cycling through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    DeliverCorrectRecipe(i);
                    return;
                }
            }
        }
        // No matches found!
        // Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    private void DeliverCorrectRecipe(int waitingRecipeSOListIndex)
    {
        RecipeSO waitingRecipeSO = waitingRecipeSOList[waitingRecipeSOListIndex];

        OnRecipeCompleted?.Invoke(this, new OnRecipeCompletedEventArgs
        {
            RecipeSO = waitingRecipeSO
        });

        successfulRecipesAmount++;
        waitingRecipeSOList.RemoveAt(waitingRecipeSOListIndex); 
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }

    public PlateScale GetNeededPlateScale(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList) {
                    // Cycling through all ingredients in the Recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            // Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        // This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe) {
                    // Found the needed recipe
                    return PlateScale.Large;  // TODO: Change this to match the needed PlateScale.
                }
            }
        }
        // No matches found!
        return PlateScale.Medium;
    }
}