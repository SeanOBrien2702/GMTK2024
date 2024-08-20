using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] disabledDuringMinigame;
    [SerializeField] private RecipeListSO recipeList;

    public static GameManager Instance;
    PlateKitchenObject scaledPlate;
    bool isPlayingMinigame = false;
    int level = 0;

    public bool IsPlayingMinigame { get => isPlayingMinigame; set => isPlayingMinigame = value; }
    public PlateKitchenObject PlateKitchenObject { get => scaledPlate; set => scaledPlate = value; }
    public int Level { get => level; set => level = value; }

    // Start is called before the first frame update
    void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    // Hacky way of getting the current recipe
    public RecipeSO GetScaledPlateRecipeSO() {
        var kitchenObjectSOList = scaledPlate.GetKitchenObjectSOList();
        var recipes = recipeList.recipeSOList;

        foreach (RecipeSO recipe in recipes) {
            int correctObjects = 0;
            foreach (KitchenObjectSO kitchenObjectSO in kitchenObjectSOList) {
                if (recipe.KitchenObjectSOList.Contains(kitchenObjectSO)) correctObjects++;
                else break;
            }
            if (correctObjects == recipe.KitchenObjectSOList.Count) return recipe;
        }

        return null;
    }

    public void ToggleMinigameStart(bool isEnabled) {
        isPlayingMinigame = isEnabled;

        foreach (GameObject go in disabledDuringMinigame) {
            go.SetActive(!isEnabled);
        }
    }
}
