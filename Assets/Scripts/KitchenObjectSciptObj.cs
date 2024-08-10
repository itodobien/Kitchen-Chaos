using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    // Reference to the prefab of the kitchen object
    public Transform prefab;

    // Reference to the sprite representing the kitchen object
    public Sprite sprite;

    // Name of the kitchen object
    public string objectName;
}
