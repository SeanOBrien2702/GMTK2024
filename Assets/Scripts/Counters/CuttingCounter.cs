using System;
using UnityEngine;

public class CuttingCounter : Counter, IHasProgress 
{
    public static event EventHandler OnAnyCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void Interact(PlayerController player) 
    {
        if (!HasKitchenObject()) 
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject()) 
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) 
                {
                    
                    // Player carrying something that can be Cut
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    player.SetKitchenObject(null);
                    InteractLogicPlaceObjectOnCounter();
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
                    }
                }
            } 
            else 
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                ClearKitchenObject();
            }
        }
    }

    private void InteractLogicPlaceObjectOnCounter() 
    {
        cuttingProgress = 0;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = 0f
        });
    }

    public override void InteractAlternate(PlayerController player) 
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) 
        {
            // There is a KitchenObject here AND it can be cut
            CutObject();
            TestCuttingProgressDone();
        }
    }

    private void CutObject() 
    {
        cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
    }

    private void TestCuttingProgressDone() 
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) 
        {
            // There is a KitchenObject here AND it can be cut
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) 
            {
                animator.SetTrigger("Transition");
                //KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                //Destroy(GetKitchenObject().gameObject);
                //SetKitchenObject(Instantiate(outputKitchenObjectSO.KitchenObject));
            }
        }
    }

    public void FoodHiddenAnimation()
    {
        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
        Destroy(GetKitchenObject().gameObject);
        SetKitchenObject(Instantiate(outputKitchenObjectSO.KitchenObject));
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) 
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) 
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}