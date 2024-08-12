using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class GameInput : MonoBehaviour
{
    // Event triggered when the interact action is performed
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

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
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        // Invoke the OnInteractAlternateAction event when the alternate interact action is performed
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        // Invoke the OnInteractAction event when the interact action is performed
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Method to get the normalized movement vector from the input.
    /// </summary>
    /// <returns>Normalized movement vector.</returns>
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
