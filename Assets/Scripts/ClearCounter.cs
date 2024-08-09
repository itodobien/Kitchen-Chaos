using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectScriptObj kitchenObjectScriptObj;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public override void Interact(Player player)
    {
        Debug.Log("ClearCounter Interact method called"); // Debug log
        if (kitchenObject == null)
        {
            // Spawn a kitchen object at the counter top point
            Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptObj.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            Debug.Log("Kitchen object spawned"); // Debug log
        }
        else
        {
            // Give the object to the player
            kitchenObject.SetKitchenObjectParent(player);
            Debug.Log("Kitchen object given to player"); // Debug log
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}