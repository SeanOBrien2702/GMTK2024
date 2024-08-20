using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScalerCounter : Counter 
{
    public override void Interact(PlayerController player) 
    {
        if (player.HasPlate())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                GameManager.Instance.PlateKitchenObject = plateKitchenObject;
                if (GameManager.Instance.GetScaledPlateRecipeSO() != null) {
                    GameManager.Instance.ToggleMinigameStart(true);
                    GameManager.Instance.PlateKitchenObject = plateKitchenObject;
                    SceneManager.LoadScene("MinigameScene", LoadSceneMode.Additive);
                } else {
                    GameManager.Instance.PlateKitchenObject = null;
                }
            }
        }
    }
}