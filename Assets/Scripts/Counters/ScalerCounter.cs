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
                GameManager.Instance.IsPlayingMinigame = true;
                GameManager.Instance.PlateKitchenObject = plateKitchenObject;
                SceneManager.LoadScene("TestMiniGameScene", LoadSceneMode.Additive);
            }
        }
    }
}