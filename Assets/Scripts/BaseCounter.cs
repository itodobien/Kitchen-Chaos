using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    // Reference to the point where the kitchen object will be placed on the counter
    [SerializeField] private Transform counterTopPoint;

    // Reference to the kitchen object currently on the counter
    private KitchenObject kitchenObject;

    // Virtual method for interacting with the counter, can be overridden by derived classes
    public virtual void Interact(Player player)
    {
        Debug.Log("Interacting with base counter");
    }

    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("InteractingAlternate with base counter");
    }

    // Method to get the transform where the kitchen object should follow
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    // Method to set the kitchen object on the counter
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    // Method to get the kitchen object currently on the counter
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Method to clear the kitchen object from the counter
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // Method to check if there is a kitchen object on the counter
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
