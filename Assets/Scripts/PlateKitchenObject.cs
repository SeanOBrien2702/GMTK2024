using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlateScale
{
    Small,
    Medium,
    Large
}

public class PlateKitchenObject : KitchenObject 
{
    PlateScale scale = PlateScale.Medium;
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    internal PlateScale Scale { get => scale; set => scale = value; }

    protected override void Awake() 
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) 
        {
            // Not a valid ingredient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) 
        {
            // Already has this type
            return false;
        } 
        else 
        {
            //AddIngredient(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));
            AddIngredient(kitchenObjectSO);
            return true;
        }
    }

    //private void AddIngredient(int kitchenObjectSOIndex) 
    private void AddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        //KitchenObjectSO kitchenObjectSO = null;// KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        kitchenObjectSOList.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
            kitchenObjectSO = kitchenObjectSO
        });
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() 
    {
        return kitchenObjectSOList;
    }
}