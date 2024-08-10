using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    // Reference to the KitchenObjectScriptObj which contains the prefab to be instantiated
    [SerializeField] private KitchenObjectScriptObj kitchenObjectScriptObj;

    // Override the Interact method to handle player interaction with the clear counter
    public override void Interact(Player player)
    {
        // Currently, this method is empty and does not perform any actions
        // You can add specific interaction logic here as needed
    }
}
