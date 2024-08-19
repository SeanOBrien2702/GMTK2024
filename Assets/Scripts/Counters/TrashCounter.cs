using System;
using UnityEngine;

public class TrashCounter : Counter 
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(PlayerController player) 
    {
        Debug.Log("interact");
        if (player.HasKitchenObject()) 
        {
            Debug.Log("has object");
            //player.Trash()
            //Destroy(player.GetKitchenObject());
            //Dest
            //Destroy(player.GetKitchenObject());
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}