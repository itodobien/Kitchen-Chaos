using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectScriptObj cutKitchenObjectScriptObj;
    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            // There is no kitchen object on the counter
            if (player.HasKitchenObject())
            {   // The player has a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // The player does not have a kitchen object
            }
        }
        else
        {
            // There is a kitchen object on the counter
            if (player.HasKitchenObject())
            {
                // The player has a kitchen object (carrying already)
            }
            else
            {
                // The player does not have a kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);

            }
        }

    }
    public override void InteractAlternate(Player player)
    {
       if (HasKitchenObject())
       {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectScriptObj, this);
       }
    }

}
