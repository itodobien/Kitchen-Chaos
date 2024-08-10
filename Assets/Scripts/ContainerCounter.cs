using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    // Reference to the KitchenObjectScriptObj which contains the prefab to be instantiated
    [SerializeField] private KitchenObjectScriptObj kitchenObjectScriptObj;

    // Event triggered when the player grabs an object from the container
    public event EventHandler OnPlayerGrabbedObject;

    // Override the Interact method to handle player interaction with the container
    public override void Interact(Player player)
    {
        // Instantiate the kitchen object prefab
        Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptObj.prefab);

        // Set the parent of the instantiated kitchen object to the player
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        // Invoke the OnPlayerGrabbedObject event
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
