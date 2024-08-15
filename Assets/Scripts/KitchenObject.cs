using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectScriptObj; // ScriptableObject containing kitchen object data

    private IKitchenObjectParent kitchenObjectParent; // Parent interface for the kitchen object

    // Returns the KitchenObjectSO associated with this kitchen object
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectScriptObj;
    }

    // Sets the parent of the kitchen object
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // Clear the current parent if it exists
        if (this.kitchenObjectParent != null)
        {
            Debug.Log($"Clearing current parent: {this.kitchenObjectParent}");
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        // Ensure the new parent does not already have a kitchen object
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("kitchenObjectParent already has a kitchen object");
        }

        // Set the new parent and update the transform
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

        Debug.Log($"Set new parent: {kitchenObjectParent}");
    }

    // Returns the current parent of the kitchen object
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    // Destroys the kitchen object and clears it from the parent
    public void DestroySelf()
    {
        Debug.Log($"Destroying kitchen object: {this}");
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    // Tries to get the kitchen object as a PlateKitchenObject
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    // Spawns a new kitchen object and sets its parent
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectScriptObj, IKitchenObjectParent kitchenObjectParent)
    {
        // Instantiate the kitchen object prefab
        Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptObj.prefab);

        // Get the KitchenObject component and set its parent
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
