using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] Image recipeIcon;
    [SerializeField] Transform iconContainer;
    [SerializeField] Image icon;

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeIcon.sprite = recipeSO.RecipeImage;

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjectSOList)
        {
            Image iconTransform = Instantiate(icon, iconContainer);
            iconTransform.sprite = kitchenObjectSO.Sprite;
        }
    }
}