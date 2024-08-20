using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : Counter
{
    public static event EventHandler OnPlateSpawned;
    public static event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObject platePrefab;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                SpawnPlate();
            }
        }
    }

    private void SpawnPlate()
    {
        platesSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    void GrabPlate()
    {
        platesSpawnedAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            if (platesSpawnedAmount > 0)
            {
                // There's at least one plate here
                //KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                player.SetKitchenObject(Instantiate(platePrefab));
                GrabPlate();
            }
        }
    }
}
