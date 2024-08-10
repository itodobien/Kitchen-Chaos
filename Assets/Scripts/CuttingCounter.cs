using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchen object on the counter
            if (player.HasKitchenObject())
            {
                // The player has a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectScriptObj()))
                {
                    // The player has a kitchen object that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    Debug.Log("Player placed kitchen object on the counter.");
                }
                else
                {
                    Debug.Log("Player's kitchen object cannot be cut.");
                }
            }
        }
        else
        {
            // There is a kitchen object on the counter
            if (player.HasKitchenObject())
            {
                Debug.Log("Player is already carrying a kitchen object.");
            }
            else
            {
                // The player does not have a kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
                Debug.Log("Player picked up kitchen object from the counter.");
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectScriptObj()))
        {
            KitchenObjectScriptObj outputKitchenObjectScriptObj = GetOutputForInput(GetKitchenObject().GetKitchenObjectScriptObj());
            Debug.Log($"Cutting {GetKitchenObject().GetKitchenObjectScriptObj().name} into {outputKitchenObjectScriptObj.name}");

            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectScriptObj, this);

            Debug.Log("New kitchen object spawned on the counter.");
        }
        else
        {
            Debug.Log("No valid kitchen object to cut or no matching recipe found.");
        }
    }

    private bool HasRecipeWithInput(KitchenObjectScriptObj inputKitchenObjectScriptObj)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectScriptObj)
            {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectScriptObj GetOutputForInput(KitchenObjectScriptObj inputKitchenObjectScriptObj)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectScriptObj)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
}
