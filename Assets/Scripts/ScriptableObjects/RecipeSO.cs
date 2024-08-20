using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject 
{
    public List<KitchenObjectSO> KitchenObjectSOList;
    public Sprite RecipeImage;
    public FoodBlockData FoodBlockData;
    public int CorrectRegularIncome;
    public int CorrectScaledIncome;
    public int IncorrectIncome;
}