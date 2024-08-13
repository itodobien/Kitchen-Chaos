using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectScriptObj;
    public event System.EventHandler OnPlayerGrabbedObject;

    /// <summary>
    /// Interacts with the player. If the player does not have a kitchen object,
    /// spawns a new kitchen object and invokes the OnPlayerGrabbedObject event.
    /// </summary>
    /// <param name="player">The player interacting with the counter.</param>
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Spawn a new kitchen object and assign it to the player
            KitchenObject.SpawnKitchenObject(kitchenObjectScriptObj, player);
            // Invoke the event to notify that the player has grabbed an object
            OnPlayerGrabbedObject?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
