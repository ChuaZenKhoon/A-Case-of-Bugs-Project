using Cinemachine.Utility;
using System;
using UnityEngine;

/**
 * Handles the Player and its logic.
 */ 
public class Player : MonoBehaviour, IObjectInteractionParent {

    public static Player Instance { get; private set; }

    //Event for handling inventory object hovering for interaction
    public event EventHandler<OnPlayerStareAtInventoryObjectChangeEventArgs> OnPlayerStareAtInventoryObjectChange;
    public event EventHandler<EquipmentSO> OnUpdateHeldItemToEquipment;

    public class OnPlayerStareAtInventoryObjectChangeEventArgs {
        public InventoryObject inventoryObject;
    }
    
    [SerializeField] private Camera viewCamera;

    [SerializeField] private Transform objectHoldPoint;

    [SerializeField] private LayerMask inventoryObjectLayer;
    [SerializeField] private LayerMask droppableLayer;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Rigidbody body;

    private const float moveSpeed = 3f;

    private Vector2 currentMoveDirection = Vector2.zero;

    private bool isWalking;
    private bool isActivated;

    private InventoryObject currentHeldObject;

    //Note: For the sake of specificity, all interactable objects are inventoryObjects hence labelled as such.
    private InventoryObject currentInteractableObjectStaringAt;
    private Vector3 currentDroppablePosition;
    private void Awake() {
        Instance = this;
        isActivated = false;
    }

    //Subscribe to events
    private void Start() {
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
        }

        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
        }
        
        body.freezeRotation = true;
    }

    private void TutorialLevelManager_OnStateChange(object sender, EventArgs e) {
        if (TutorialLevelManager.Instance.IsStartingMovement()) {
            isActivated = true;
            GameInput.Instance.OnMove += GameInput_OnMove;
        }

        if (TutorialLevelManager.Instance.IsStartingInteraction()) {
            GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        }
    }

    private void FixedUpdate() {
        if (isActivated) {
            HandleWalking();
            HandleStareAt();
        }
    }

    private void CrimeSceneLevelManager_OnStateChange(object sender, EventArgs e) {
        if(CrimeSceneLevelManager.Instance.IsGamePlaying()) {
            isActivated = true;
            
            //Allow interaction only when game is playing
            GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
            GameInput.Instance.OnMove += GameInput_OnMove;
        }
    }

    /**
     * Next 3 functions deal with player movement:
     * 1) Reacting when movement event is fired
     * 2) Handling Movement of Player
     * 3) Setting IsWalking State of Player
     */

    private void GameInput_OnMove(object sender, Vector2 e) {
        currentMoveDirection = e;
    }

    private void HandleWalking() {

        Vector2 moveVector = currentMoveDirection;

        //Move relative to camera looking direction and not strict x-y values
        //X is right, Y is up, Z is forward
        Vector3 moveDirection = new Vector3(moveVector.x, 0, moveVector.y);
        Vector3 cameraForward = viewCamera.transform.forward;
        Vector3 cameraRight = viewCamera.transform.right;


        // Project the forward and right vectors onto the XZ plane (ignoring Y component) to prevent moving up or down

        cameraRight.y = 0;
        cameraForward.y = 0;


        if (cameraForward == Vector3.zero) {
            float dotUp = Vector3.Dot(viewCamera.transform.forward, Vector3.up);
            if (dotUp > 0.9f) {
                // If facing up, use -camera.transform.up
                cameraForward = -viewCamera.transform.up;
            } else if (dotUp < -0.9f) {
                // If facing down, use camera.transform.up
                cameraForward = viewCamera.transform.up;
            }
        }

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 relativeMoveDirection = (cameraRight * moveDirection.x + cameraForward * moveDirection.z).normalized;

        float moveDistance = moveSpeed * Time.deltaTime;

        //Fire Raycast if player collides with collider
        /*        bool hasCollided = Physics.CapsuleCast(transform.position + Vector3.up, transform.position + Vector3.up * playerHeight, 
                    playerRadius, relativeMoveDirection, out RaycastHit hitInfo, moveDistance);
                bool isSliding = false;

                if (hasCollided) {

                    // Check for step
                    float stepHeight = 0.2f; // The height the player can step up
                    float stepForwardDistance = 0.1f; // The forward distance to check for a step

                    Vector3 stepUpCheckPosition = body.position + relativeMoveDirection * stepForwardDistance;
                    Vector3 stepUpRayStart = stepUpCheckPosition + Vector3.up * (stepHeight + 0.1f); // Start raycast slightly above step height
                    Vector3 stepUpRayDirection = Vector3.down; // Cast ray downwards

                    if (Physics.Raycast(stepUpRayStart, stepUpRayDirection, out RaycastHit stepHit, stepHeight + 0.2f)) {
                        // If the step is within the step height, move the player up
                        if (stepHit.distance <= stepHeight + 0.1f) {
                            body.position += Vector3.up * (stepHeight + 0.1f - stepHit.distance);
                            // Adjust relativeMoveDirection to consider the new height
                            hasCollided = false; // Allow movement after stepping up
                        }
                    }


                    //Player to slide against wall instead of being stuck
                    //Check if it is a corner at the same time
                    Vector3 slideDirection = Vector3.ProjectOnPlane(relativeMoveDirection, hitInfo.normal).normalized;
                    bool isCorner = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                    playerRadius, slideDirection, moveDistance);


                    if (!Physics.Raycast(transform.position, slideDirection, playerRadius) && !isCorner) {
                        // Apply the sliding direction if possible to move in that direction, and not a corner
                        relativeMoveDirection = slideDirection;
                        isSliding = true;
                    }
                }*/

        /*        if (isSliding || !hasCollided) {


                }*/
        Vector3 velocity = relativeMoveDirection * moveSpeed;
        body.velocity = new Vector3(velocity.x, body.velocity.y, velocity.z);
        isWalking = relativeMoveDirection != Vector3.zero;
      
    }

    public bool IsWalking() {
        return isWalking;
    }

    //In inventory screen, cannot move or interact

    public void ToggleActivationState(bool isActivated) {
        this.isActivated = isActivated;

        if (!isActivated) {
            isWalking = false;
            SetStareAtObject(null);
        }
    }

    /**
     * Next 3 functions deal with player object interaction:
     * 1) Handling when mouse hovers over interactable object in range
     * 2) Setting the current interactable object player is staring at
     * 3) Reacting when interaction event is fired
     */

    private void HandleStareAt() {
        float interactDistance = 2.5f;

        bool isHit = Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, 
            out RaycastHit rayCastHit, interactDistance, inventoryObjectLayer);
        if (isHit) {
            if (rayCastHit.transform.TryGetComponent(out InventoryObject inventoryObject)) {
                SetStareAtObject(inventoryObject);
            } else {
                SetStareAtObject(null);
            }

        } else {
            SetStareAtObject(null);
        }

        bool isLookingAtADroppableSpot = Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward,
            out RaycastHit rayCastLookAt, interactDistance, droppableLayer);

        if (isLookingAtADroppableSpot) {
            currentDroppablePosition = rayCastLookAt.point;
            if (currentHeldObject is PlacardHolder) {
                PlacardHolder placardHolder = currentHeldObject as PlacardHolder;
                placardHolder.ShowPlacementPosition(currentDroppablePosition, true);
            }

        } else {
            currentDroppablePosition = this.transform.position;
            if (currentHeldObject is PlacardHolder) {
                PlacardHolder placardHolder = currentHeldObject as PlacardHolder;
                placardHolder.ShowPlacementPosition(currentDroppablePosition, false);
            }
        }

    }

    /**
     * Updates the interactable object the Player is looking at in range
     * 
     * @Param inventoryObject The InventoryObject currently being looked at
     */
    private void SetStareAtObject(InventoryObject inventoryObject) {
        this.currentInteractableObjectStaringAt = inventoryObject;
        OnPlayerStareAtInventoryObjectChange?.Invoke(this, new OnPlayerStareAtInventoryObjectChangeEventArgs {
            inventoryObject = inventoryObject
        });
    }

    public Vector3 GetStareAtPosition() {
        return currentDroppablePosition;
    }
    public InventoryObject GetStareAt() {
        return currentInteractableObjectStaringAt;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (currentHeldObject is Equipment) {
            currentHeldObject.Interact();
        } else if (currentInteractableObjectStaringAt != null) {
            currentInteractableObjectStaringAt.Interact();
        }
    }


    /**
     * Updates the item held in the Player's hand
     * when changes occur to the inventory bar slot Player is currently selecting
     * 
     * @Param inventoryObjectSO The InventoryObjectSO to be used to update the held item 
     */
    public void UpdateHeldItem(InventoryObjectSO inventoryObjectSO, int equipmentID) {

        //Remove previous object held
        foreach (Transform child in objectHoldPoint) {
            Destroy(child.gameObject);
        }

        //Update to new item
        if (inventoryObjectSO == null) {
            this.currentHeldObject = null;
            OnUpdateHeldItemToEquipment?.Invoke(this, null);
        } else {
            GameObject inventoryObjectToSpawn = Instantiate(inventoryObjectSO.prefab);

            //Remove boxCollider component as item is now held and not interactable with
            bool hasCollider = inventoryObjectToSpawn.TryGetComponent<BoxCollider>(out BoxCollider boxCollider);
            if (hasCollider) {
                boxCollider.enabled = false;
            }
            InventoryObject inventoryObjectToHold = inventoryObjectToSpawn.GetComponent<InventoryObject>();
            this.currentHeldObject = inventoryObjectToHold;

            inventoryObjectToSpawn.transform.parent = this.objectHoldPoint;
            inventoryObjectToSpawn.transform.localPosition = Vector3.zero;

            if (currentHeldObject is Equipment) {
                EquipmentSO currentHeldEquipmentSO = currentHeldObject.GetInventoryObjectSO() as EquipmentSO;
                OnUpdateHeldItemToEquipment?.Invoke(this, currentHeldEquipmentSO);
            }

            if (currentHeldObject is EvidenceInteractingEquipment) {
                EvidenceInteractingEquipment currentHeldEvidenceInteractingEquipment = currentHeldObject as EvidenceInteractingEquipment;
                currentHeldEvidenceInteractingEquipment.SetEquipmentID(equipmentID);
            }

            if (currentHeldObject is Evidence) {
                Evidence currentHeldEvidence = currentHeldObject as Evidence;
                currentHeldEvidence.SealEvidence();
                BoxCollider[] colliders = currentHeldEvidence.GetComponentsInChildren<BoxCollider>();
                if (colliders[1] != null) {
                    colliders[1].enabled = false;
                }
            }
        }
    }
}
