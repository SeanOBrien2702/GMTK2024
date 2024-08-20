using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] disabledDuringMinigame;
    [SerializeField] private RecipeListSO recipeList;

    public static event EventHandler OnRoundOver;
    public static event EventHandler<OnMoneyChangedEvent> OnMoneyChanged;
    public class OnMoneyChangedEvent : EventArgs
    {
        public int Money;
    }
    public static event EventHandler<OnGameTimeChangedEvent> OnGameTimeChanged;
    public class OnGameTimeChangedEvent : EventArgs
    {
        public float Time;
    }
    public static GameManager Instance;
    PlateKitchenObject scaledPlate;
    bool isGamePaused = false;
    int level = 0;
    int money;

    float roundTime = 180;
    float roundTimer = 0;

    public float RoundTimer
    {
        get => roundTimer;
        set
        {
            roundTimer = value;
            RoundTimer_OnValueChanged();
        }
    }


    public bool IsGamePaused { get => isGamePaused; set => isGamePaused = value; }
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
            if (kitchenObjectSOList.Count != recipe.KitchenObjectSOList.Count) continue;

            bool isCorrectRecipe = true;

            foreach (KitchenObjectSO kitchenObjectSO in kitchenObjectSOList) {
                if (!recipe.KitchenObjectSOList.Contains(kitchenObjectSO)) {
                    isCorrectRecipe = false;
                    break;
                }
            }

            if (isCorrectRecipe) return recipe;
        }

        return null;
    }

    public void ToggleMinigameStart(bool isEnabled) {
        isGamePaused = isEnabled;

        foreach (GameObject go in disabledDuringMinigame) {
            go.SetActive(!isEnabled);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RoundTimer += Time.deltaTime;
        if(roundTimer > roundTime)
        {
            OnRoundOver?.Invoke(this, EventArgs.Empty);
        }
    }

    private void RoundTimer_OnValueChanged()
    {
        OnGameTimeChanged?.Invoke(this, new OnGameTimeChangedEvent
        {
            Time = roundTimer / roundTime
        });
    }

    public void StartNextRound()
    {
        level++;
        roundTime = 0;
    }
}
