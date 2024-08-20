using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject 
{
    public List<KitchenObjectSO> KitchenObjectSOList;
    public Sprite RecipeImage;
    public int CorrectRegularIncome;
    public int CorrectScaledIncome;
    public int IncorrectIncome;
}