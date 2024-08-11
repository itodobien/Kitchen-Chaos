using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    // Reference to the KitchenObjectSO which contains the prefab to be instantiated
    [SerializeField] private KitchenObjectSO kitchenObjectScriptObj;

    // Event triggered when the player grabs an object from the container
    public event EventHandler OnPlayerGrabbedObject;

    // Override the Interact method to handle player interaction with the container
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
            //Player is not carrying a kitchen object
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectScriptObj, player);

            // Invoke the OnPlayerGrabbedObject event
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
