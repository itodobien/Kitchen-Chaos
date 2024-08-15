using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for any class that wants to act as a parent for KitchenObject instances.
/// This interface ensures that any class implementing it will provide specific methods
/// related to handling kitchen objects.

public interface IKitchenObjectParent
{
    // Gets the Transform where the kitchen object should be positioned relative to its parent.
    public Transform GetKitchenObjectFollowTransform();

    // Sets the kitchen object that the parent is currently holding or managing.
    public void SetKitchenObject(KitchenObject kitchenObject);

    // Gets the kitchen object that the parent is currently holding or managing.
    public KitchenObject GetKitchenObject();

    // Clears the reference to the kitchen object, indicating that the parent is no longer
    // holding or managing any kitchen object.
    public void ClearKitchenObject();

    // Checks if the parent currently has a kitchen object.
    public bool HasKitchenObject();
}
    