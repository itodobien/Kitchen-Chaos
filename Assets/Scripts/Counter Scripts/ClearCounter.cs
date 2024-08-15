using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class ClearCounter : BaseCounter
{
    // Serialized field to hold a reference to a KitchenObject ScriptableObject
    [SerializeField] private KitchenObjectSO kitchenObjectScriptObj;

    // Override the Interact method to define specific interaction behavior
    public override void Interact(Player player)
    {
        // Check if the counter does not have a kitchen object
        if (!HasKitchenObject())
        {
            // If the player has a kitchen object, set the counter as its parent
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // If the counter has a kitchen object
            if (player.HasKitchenObject())
            {
                // Try to get the kitchen object held by the player as a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Try to add the counter's kitchen object as an ingredient to the player's plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Destroy the counter's kitchen object if added successfully
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Try to get the counter's kitchen object as a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Try to add the player's kitchen object as an ingredient to the counter's plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // Destroy the player's kitchen object if added successfully
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // If the player does not have a kitchen object, set the player as the parent of the counter's kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
