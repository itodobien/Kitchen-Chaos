using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    // Event to notify when the selected counter changes
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    // Custom event arguments to hold the selected counter
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    // Serialized fields to set in the Unity Editor
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance in the scene.");
        }
        // Set the singleton instance
        Instance = this;
    }

    // Private fields to manage player state
    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Start()
    {
        // Subscribe to the interact action event from the GameInput class
        gameInput.onInteractAction += GameInput_onInteractAction;
    }

    // Event handler for the interact action
    private void GameInput_onInteractAction(object sender, EventArgs e)
    {
        // Interact with the selected counter if it exists
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        // Handle player movement and interactions each frame
        HandleMovement();
        HandleInteractions();
    }

    // Method to check if the player is walking
    public bool IsWalking()
    {
        return isWalking;
    }

    // Method to handle player interactions with counters
    private void HandleInteractions()
    {
        // Get the normalized movement vector from the GameInput class
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Update the last interaction direction if the player is moving
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        RaycastHit raycastHit;
        float interactDistance = 2f;

        // Perform a raycast to detect counters in the interaction direction
        if (Physics.Raycast(transform.position, lastInteractDir, out raycastHit, interactDistance, countersLayerMask))
        {
            // Check if the raycast hit a ClearCounter component
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // If a different counter is selected, update the selected counter
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                // If no counter is hit, clear the selected counter
                SetSelectedCounter(null);
            }
        }
        else
        {
            // If no counter is hit, clear the selected counter
            SetSelectedCounter(null);
        }

    }

    // Method to handle player movement
    private void HandleMovement()
    {
        // Get the normalized movement vector from the GameInput class
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .65f;
        float playerHeight = 2f;

        // Check if the player can move in the desired direction using a capsule cast
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Attempt to move only along the x-axis if the initial move is blocked
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Update move direction to x-axis only
                moveDir = moveDirX;
            }
            else
            {
                // Attempt to move only along the z-axis if the x-axis move is blocked
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Update move direction to z-axis only
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            // Update the player's position
            transform.position += moveDir * moveDistance;
        }

        // Update the walking state
        isWalking = moveDir != Vector3.zero;

        // Smoothly rotate the player to face the movement direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Method to set the selected counter and invoke the event
    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}
