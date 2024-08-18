using System;
using UnityEngine;

public class ContainerCounter : Counter 
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObject kitchenObjectPrefab;
    [SerializeField] Animator splashAnimator;

    public override void Interact(PlayerController player) 
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            //KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            player.SetKitchenObject(Instantiate(kitchenObjectPrefab));
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            if(splashAnimator)
            {
                splashAnimator.SetTrigger("Splash");
            }
        }
    }
}