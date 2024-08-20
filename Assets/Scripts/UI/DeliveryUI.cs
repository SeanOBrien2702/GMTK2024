using UnityEngine;

public class DeliveryUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private RecipeUI recipeUI;
    RectTransform rectTransform;

    private void Start()
    {
        DeliveryController.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryController.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        rectTransform = GetComponent<RectTransform>();  
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryController.Instance.GetWaitingRecipeSOList())
        {
            RecipeUI recipeTransform = Instantiate(recipeUI, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.SetRecipeSO(recipeSO);
        }
        rectTransform.sizeDelta = new Vector2(20 + 130 * DeliveryController.Instance.GetWaitingRecipeSOList().Count, 140);
    }
}