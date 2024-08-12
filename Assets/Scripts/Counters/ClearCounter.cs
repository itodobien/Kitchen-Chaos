using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ClearCounter : BaseCounter
{
    // Reference to the KitchenObjectSO which contains the prefab to be instantiated
    [SerializeField] private KitchenObjectSO kitchenObjectScriptObj;

    // Override the Interact method to handle player interaction with the clear counter
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchen object on the counter
            if (player.HasKitchenObject())
            {
                // The player has a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // The player does not have a kitchen object
            }
        }
        else // There is a kitchen object on the counter
        {
            if (player.HasKitchenObject()) // The player has a kitchen object (carrying already)
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // The player is carrying a plate 
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // The player is not carrying a plate but has something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // The counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // The player does not have a kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
