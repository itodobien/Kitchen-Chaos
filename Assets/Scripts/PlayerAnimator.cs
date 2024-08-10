using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Constant string for the "IsWalking" parameter in the Animator
    private const string IS_WALKING = "IsWalking";

    // Reference to the Player component
    [SerializeField] private Player player;

    // Reference to the Animator component
    private Animator animator;

    private void Awake()
    {
        // Initialize the animator component
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Set the "IsWalking" parameter in the Animator based on the player's walking state
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
