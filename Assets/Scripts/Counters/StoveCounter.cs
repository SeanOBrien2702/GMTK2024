using System;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounter : Counter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    State state = State.Idle;
    float fryingTimer = 0f;
    private FryingRecipeSO fryingRecipeSO;
    float burningTimer = 0f;
    private BurningRecipeSO burningRecipeSO;

    public float FryingTimer
    {
        get => fryingTimer;
        set
        {
            fryingTimer = value;
            FryingTimer_OnValueChanged();
        }
    }

    public float BurningTimer
    {
        get => burningTimer;
        set
        {
            burningTimer = value;
            BurningTimer_OnValueChanged();
        }
    }

    public State StoveState 
    { 
        get => state;
        set 
        { 
            state = value;
            State_OnValueChanged();
        }
    }

    private void FryingTimer_OnValueChanged()
    {
        float fryingTimerMax = fryingRecipeSO != null ? fryingRecipeSO.fryingTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer / fryingTimerMax
        });
    }

    private void BurningTimer_OnValueChanged()
    {
        float burningTimerMax = burningRecipeSO != null ? burningRecipeSO.burningTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = burningTimer / burningTimerMax
        });
    }

    private void State_OnValueChanged()
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });

        if (state == State.Burned || state == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;
        if (GameManager.Instance.IsGamePaused) return;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                FryingTimer += Time.deltaTime;
                if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                {
                    Destroy(GetKitchenObject().gameObject);
                    SetKitchenObject(Instantiate(fryingRecipeSO.output.KitchenObject));
                    state = State.Fried;
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                }
                break;
            case State.Fried:
                BurningTimer += Time.deltaTime;
                if (burningTimer > burningRecipeSO.burningTimerMax)
                {
                    Destroy(GetKitchenObject().gameObject);
                    SetKitchenObject(Instantiate(burningRecipeSO.output.KitchenObject));
                    StoveState = State.Burned;
                }
                break;
            case State.Burned:
                break;
        }
        
    }

    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                fryingRecipeSO = HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO());
                if (fryingRecipeSO != null)
                {
                    //bug.Log("food placed");
                    // Player carrying something that can be Fried
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    player.SetKitchenObject(null);
                    fryingTimer = 0f;
                    state = State.Frying;
                }
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        Destroy(GetKitchenObject().gameObject);
                        StoveState = State.Idle;
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                ClearKitchenObject();
                StoveState = State.Idle;
            }
        }
    }

    private FryingRecipeSO HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}