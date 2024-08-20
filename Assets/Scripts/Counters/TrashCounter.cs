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
        if (player.HasKitchenObject()) 
        {
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}