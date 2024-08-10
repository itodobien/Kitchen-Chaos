using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    // Constant string for the animation trigger
    private const string CUT = "Cut";

    // Reference to the ContainerCounter component
    [SerializeField] private CuttingCounter cuttingCounter;

    // Reference to the Animator component
    private Animator animator;

    private void Awake()
    {
        // Initialize the animator component
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
        // Subscribe to the OnPlayerGrabbedObject event
    }
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        // Trigger the cut animation
        animator.SetTrigger(CUT);
    }
    
}
