using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    // Event triggered when any object is trashed
    public static event EventHandler OnAnyObjectTrashed;

    // Resets the static event data
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    // Overrides the Interact method from BaseCounter
    public override void Interact(Player player)
    {
        // Check if the player has a kitchen object
        if (player.HasKitchenObject())
        {
            // Destroy the kitchen object the player is holding
            player.GetKitchenObject().DestroySelf();

            // Trigger the OnAnyObjectTrashed event
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
