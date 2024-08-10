using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    // Event triggered when the interact action is performed
    public event EventHandler onInteractAction;

    // Reference to the PlayerInputActions
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        // Initialize the PlayerInputActions
        playerInputActions = new PlayerInputActions();

        // Enable the player input actions
        playerInputActions.Player.Enable();

        // Subscribe to the Interact action performed event
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        // Invoke the onInteractAction event when the interact action is performed
        onInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // Method to get the normalized movement vector from the input
    public Vector2 GetMovementVectorNormalized()
    {
        // Read the movement input vector
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Return the normalized movement vector
        return inputVector.normalized;
    }

    private void OnDisable()
    {
        // Disable the player input actions when the object is disabled
        playerInputActions.Player.Disable();
    }
}
