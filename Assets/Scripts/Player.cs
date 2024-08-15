using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour, IKitchenObjectParent
{
    // Singleton instance of the Player class
    public static Player Instance { get; private set; }

    public event EventHandler onPickedSomething; // Event triggered when the player picks up a kitchen object

    // Event triggered when the selected counter changes
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    // Serialized fields for configuration in the Unity Editor
    [SerializeField] private float moveSpeed = 7f; // Speed at which the player moves
    [SerializeField] private GameInput gameInput; // Reference to the GameInput class for handling input
    [SerializeField] private LayerMask countersLayerMask; // Layer mask to identify counters
    [SerializeField] private Transform kitchenObjectHoldPoint; // Transform where the kitchen object will be held

    // Private fields for internal state management
    private bool isWalking; // Flag to check if the player is walking
    private Vector3 lastInteractDir; // Direction of the last interaction
    private BaseCounter selectedCounter; // Currently selected counter
    private KitchenObject kitchenObject; // Currently held kitchen object

    private void Awake()
    {
        // Ensure there is only one instance of the Player class
        if (Instance != null)
        {
        }
        Instance = this;
    }

    private void Start()
    {
        // Subscribe to the interact action event from the GameInput class
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            // Interact with the selected counter
            selectedCounter.InteractAlternate(this);
        }
        else
        {
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {

        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            // Interact with the selected counter
            selectedCounter.Interact(this);
        }
        else
        {
     
        }
    }

    private void Update()
    {
        // Handle player movement and interactions in each frame
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        // Get the normalized movement vector from the input
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Update the last interaction direction if the player is moving
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f; // Distance within which the player can interact with counters
        // Perform a raycast to detect counters in the interaction direction
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    // Set the selected counter if a new counter is detected
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                // Clear the selected counter if no counter is detected
                SetSelectedCounter(null);
            }
        }
        else
        {
            // Clear the selected counter if no counter is in range
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        // Get the normalized movement vector from the input
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime; // Calculate the distance to move in this frame
        float playerRadius = .7f; // Radius of the player's collider
        float playerHeight = 2f; // Height of the player's collider
        // Check if the player can move in the desired direction using a capsule cast
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Try moving only in the X direction if movement is blocked
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Try moving only in the Z direction if movement is still blocked
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            // Move the player in the desired direction
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero; // Update the walking state

        float rotateSpeed = 12f; // Speed at which the player rotates
        // Smoothly rotate the player to face the movement direction
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        // Trigger the OnSelectedCounterChanged event
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            onPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject() 
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
