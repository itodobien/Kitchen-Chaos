using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlateKitchenObject : KitchenObject
{
    // Event triggered when an ingredient is added
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    // Event arguments for OnIngredientAdded event
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList; // List of valid kitchen objects
    private List<KitchenObjectSO> kitchenObjectSOList; // List of current kitchen objects on the plate

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>(); // Initialize the list of kitchen objects
    }

    // Method to try adding an ingredient to the plate
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // Check if the kitchen object is valid
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }

        // Check if the kitchen object is already on the plate
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false; // Return false if the kitchen object is already on the plate
        }
        else
        {
            // Add the kitchen object to the plate
            kitchenObjectSOList.Add(kitchenObjectSO);

            // Trigger the OnIngredientAdded event
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });

            return true; // Return true if the kitchen object was successfully added
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
