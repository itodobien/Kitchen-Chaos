using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    // Constant string for the animation trigger
    private const string OPEN_CLOSE = "OpenClose";

    // Reference to the ContainerCounter component
    [SerializeField] private ContainerCounter containerCounter;

    // Reference to the Animator component
    private Animator animator;

    private void Awake()
    {
        // Initialize the animator component
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe to the OnPlayerGrabbedObject event
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        // Trigger the open/close animation
        animator.SetTrigger(OPEN_CLOSE);
    }
}
