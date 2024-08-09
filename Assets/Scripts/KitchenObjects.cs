using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptObj kitchenObjectSciptObj;

    private ClearCounter clearCounter;

    public KitchenObjectScriptObj GetKitchenObjectScriptObj()
    {
        return kitchenObjectSciptObj;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearKitchenObject();
        }

        this.clearCounter = clearCounter;
        if (clearCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a kitchen object");
        }

        clearCounter.SetKitchenObject(this);
        transform.parent = clearCounter.GetkitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }

    //Just making a not to test automation of commits. need something to commit.

}
