using System;
using UnityEngine;

/**
 * A component of the Player that handles its interaction.
 */
public class PlayerInteractor : MonoBehaviour {

    //Event for handling inventory object hovering for interaction
    public event EventHandler<OnPlayerStareAtInteractableObjectChangeEventArgs> OnPlayerStareAtInteractableObjectChange;
    public event EventHandler<EquipmentSO> OnUpdateHeldItemToEquipment;

    public class OnPlayerStareAtInteractableObjectChangeEventArgs {
        public InteractableObject interactableObject;
    }

    private const float INTERACT_DISTANCE = 2.5f;


    [SerializeField] private PlayerCamera playerCamera;

    [SerializeField] private Transform objectHoldPoint;

    [SerializeField] private LayerMask interactableObjectLayer;
    [SerializeField] private LayerMask droppableLayer;

    private InventoryObject currentHeldObject;

    private InteractableObject currentInteractableObjectStaringAt;
    private Vector3 currentDroppablePosition;


    private void Start() {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    /**
     * Next 3 functions deal with player object interaction:
     * 1) Handling general interaction every frame
     * 2) Setting the current interactable object player is staring at
     * 3) Reacting when interaction event is fired
     */

    public void HandleStareAt() {
        HandleLooking();
        HandleDropping(); 
    }

    /**
     * Updates the interactable object the Player is looking at in range
     * 
     * @Param interactableObject The object that can be interacted with, currently being looked at
     */
    public void SetStareAtObject(InteractableObject interactableObject) {
        this.currentInteractableObjectStaringAt = interactableObject;
        OnPlayerStareAtInteractableObjectChange?.Invoke(this, new OnPlayerStareAtInteractableObjectChangeEventArgs {
            interactableObject = interactableObject
        });
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (!Player.Instance.IsInteractionAllowed()) {
            return;
        }

        if (currentInteractableObjectStaringAt is LabEquipment) {
            currentInteractableObjectStaringAt.Interact();
        } else if (currentHeldObject is Equipment) {
            currentHeldObject.Interact();
        } else if (currentInteractableObjectStaringAt != null) {
            currentInteractableObjectStaringAt.Interact();
        }
    }

    /**
     * Checks if there is an interactable object the Player is looking at and registers it.
     */
    private void HandleLooking() {
        bool isHit = Physics.Raycast(playerCamera.GetViewCamera().transform.position, playerCamera.GetViewCamera().transform.forward,
            out RaycastHit rayCastHit, INTERACT_DISTANCE, interactableObjectLayer);

        if (isHit) {
            if (rayCastHit.transform.TryGetComponent(out InteractableObject interactableObject)) {
                SetStareAtObject(interactableObject);
            } else {
                SetStareAtObject(null);
            }
        } else {
            SetStareAtObject(null);
        }
    }

    /**
     * Checks if there is a droppable spot the Player is looking at and registers it.
     */
    private void HandleDropping() {
        bool isLookingAtADroppableSpot = Physics.Raycast(playerCamera.GetViewCamera().transform.position, playerCamera.GetViewCamera().transform.forward,
            out RaycastHit rayCastLookAt, INTERACT_DISTANCE, droppableLayer);

        if (isLookingAtADroppableSpot) {
            currentDroppablePosition = rayCastLookAt.point;
            if (currentHeldObject is PlacardHolder) {
                PlacardHolder placardHolder = currentHeldObject as PlacardHolder;
                placardHolder.ShowPlacementPosition(currentDroppablePosition, true);
            }

        } else {
            currentDroppablePosition = Player.Instance.transform.position;
            if (currentHeldObject is PlacardHolder) {
                PlacardHolder placardHolder = currentHeldObject as PlacardHolder;
                placardHolder.ShowPlacementPosition(currentDroppablePosition, false);
            }
        }
    }

    /**
     * Updates the item held in the Player's hand
     * when changes occur to the inventory bar slot Player is currently selecting
     * 
     * @Param inventoryObjectSO The InventoryObjectSO to be used to update the held item 
     */
    public void UpdateHeldItem(InventoryObjectSO inventoryObjectSO, int equipmentID) {
        ClearHoldPoint();

        //Update to new item
        if (inventoryObjectSO == null) {
            this.currentHeldObject = null;
            OnUpdateHeldItemToEquipment?.Invoke(this, null);
        } else {
            AddNewItem(inventoryObjectSO);
            HandleItemSpecifications(equipmentID);
        }
    }

    //Remove previous object held
    private void ClearHoldPoint() {
        foreach (Transform child in objectHoldPoint) {
            Destroy(child.gameObject);
        }
    }

    /**
     * Adds new item to hold point for Player.
     * 
     * @param inventoryObjectSO the blueprint of the new item to hold.
     */
    private void AddNewItem(InventoryObjectSO inventoryObjectSO) {
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
    }

    /**
     * Resolves any actions to take regarding the new item added.
     * 
     * @param equipmentID the equipment number of the new item to hold if any.
     */
    private void HandleItemSpecifications(int equipmentID) {
        if (currentHeldObject is Equipment) {
            EquipmentSO currentHeldEquipmentSO = currentHeldObject.GetInventoryObjectSO() as EquipmentSO;
            OnUpdateHeldItemToEquipment?.Invoke(this, currentHeldEquipmentSO);
        }

        if (currentHeldObject is EvidenceInteractingEquipment) {
            EvidenceInteractingEquipment currentHeldEvidenceInteractingEquipment = currentHeldObject as EvidenceInteractingEquipment;
            currentHeldEvidenceInteractingEquipment.SetEquipmentID(equipmentID);
        }

        if (currentHeldObject is SealedEvidence) {
            SealedEvidence currentHeldEvidence = currentHeldObject as SealedEvidence;
            currentHeldEvidence.SealEvidence();
            BoxCollider[] colliders = currentHeldEvidence.GetComponents<BoxCollider>();
            if (colliders.Length > 1) {
                if (colliders[1] != null) {
                    colliders[1].enabled = false;
                }
            }
        }
    }

    public Vector3 GetStareAtPosition() {
        return currentDroppablePosition;
    }

    public InteractableObject GetStareAt() {
        return currentInteractableObjectStaringAt;
    }

    public InventoryObject GetHeldItem() {
        return currentHeldObject;
    }

    public Transform GetHoldPosition() {
        return objectHoldPoint;
    }
}
