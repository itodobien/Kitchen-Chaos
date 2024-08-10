using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptObj kitchenObjectSciptObj;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectScriptObj GetKitchenObjectScriptObj()
    {
        return kitchenObjectSciptObj;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("kitchenObjectParent already has a kitchen object");
        }

        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {        
        kitchenObjectParent.ClearKitchenObject();
        
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectScriptObj kitchenObjectScriptObj, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectScriptObj.prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
