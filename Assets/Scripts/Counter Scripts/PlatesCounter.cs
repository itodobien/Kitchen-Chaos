using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax =4f;
    private int platesSpawnAmountMax = 4;
    private int platesSpawnAmount; 

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnAmount < platesSpawnAmountMax)
            {
                platesSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);

            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) // If player has no kitchen object
        {            

            if (platesSpawnAmount > 0) // If there are plates on the counter
            {
                platesSpawnAmount--; // Decrease the amount of plates on the counter

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty); // Invoke the OnPlateRemoved event
            }
        }
    }
}
