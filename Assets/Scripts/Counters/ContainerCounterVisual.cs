using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;

    private void Awake()
    {
        // Initialize the animator component
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe to the OnPlayerGrabbedObject event if containerCounter is not null
        if (containerCounter != null)
        {
            containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
        }
        else
        {
            Debug.LogError("ContainerCounterVisual: containerCounter reference is null");
        }
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        // Trigger the OpenClose animation when the player grabs an object
        animator.SetTrigger(OPEN_CLOSE);
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnPlayerGrabbedObject event to avoid memory leaks
        if (containerCounter != null)
        {
            containerCounter.OnPlayerGrabbedObject -= ContainerCounter_OnPlayerGrabbedObject;
        }
    }
}
