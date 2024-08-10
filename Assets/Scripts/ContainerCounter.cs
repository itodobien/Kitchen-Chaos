using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter 
{
    [SerializeField] private KitchenObjectScriptObj kitchenObjectScriptObj;

    public event EventHandler OnPlayerGrabbedObject;
 
    public override void Interact(Player player)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptObj.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

    }    
}
