using Cinemachine.Utility;
using System;
using UnityEngine;
using static PlayerInteractor;

/**
 * An encapsulation of the player character in the game.
 */ 
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public event EventHandler<OnPlayerStareAtInteractableObjectChangeEventArgs> OnPlayerStareAtInteractableObjectChange;
    public event EventHandler<EquipmentSO> OnUpdateHeldItemToEquipment;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerInteractor playerInteractor;
    [SerializeField] private PlayerSound playerSound;

    [SerializeField] private Transform photoCameraToPlayerEyePosition;

    private bool isMovementActivated;
    private bool isInteractionActivated;
    private bool isCameraAllowedToMove;

    private void Awake() {
        Instance = this;
        isMovementActivated = false;
        isInteractionActivated = false;
        isCameraAllowedToMove = false;
    }

    private void Start() {
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
        }

        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
        }

        playerInteractor.OnPlayerStareAtInteractableObjectChange += PlayerInteractor_OnPlayerStareAtInteractableObjectChange;
        playerInteractor.OnUpdateHeldItemToEquipment += PlayerInteractor_OnUpdateHeldItemToEquipment;
    }

    //Events are from interaction, to inform other classes
    private void PlayerInteractor_OnUpdateHeldItemToEquipment(object sender, EquipmentSO e) {
        OnUpdateHeldItemToEquipment?.Invoke(this, e);
    }

    private void PlayerInteractor_OnPlayerStareAtInteractableObjectChange(object sender, PlayerInteractor.OnPlayerStareAtInteractableObjectChangeEventArgs e) {
        OnPlayerStareAtInteractableObjectChange?.Invoke(this, e);
    }

    //Enable movement and interaction from correct tutorial stage
    private void TutorialLevelManager_OnStateChange(object sender, EventArgs e) {
        if (TutorialLevelManager.Instance.IsStartingMovement()) {
            isMovementActivated = true;
            isCameraAllowedToMove = true;
        }

        if (TutorialLevelManager.Instance.IsStartingInteraction()) {
            isInteractionActivated = true;
        }
    }

    //Enable movement and interaction only when game starts
    private void CrimeSceneLevelManager_OnStateChange(object sender, EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGamePlaying()) {
            isMovementActivated = true;
            isInteractionActivated = true;
            isCameraAllowedToMove = true;
        }
    }

    //Using rigid body physics so fixedUpdate instead of normal update
    private void FixedUpdate() {
        if (isMovementActivated) {
            playerMovement.HandleWalking();
            
        } 

        if (isInteractionActivated) {
            playerInteractor.HandleStareAt();
        }
    }

    //Camera movement and sound to be per frame
    private void Update() {
        if (isMovementActivated) {
            playerSound.HandlePlayerWalkingSFX();
        }
        
        if (isCameraAllowedToMove) {
            playerCamera.HandleCameraMovement();
        }
    }

    //Adjusting which player components can act according to current game state
    public void ToggleMovementState(bool isActivated) {
        this.isMovementActivated = isActivated;
    }

    public void ToggleInteractionState(bool isActivated) {
        this.isInteractionActivated = isActivated;
        playerInteractor.SetStareAtObject(null);
    }

    public void ToggleCameraMovementState(bool isActivated) {
        this.isCameraAllowedToMove = isActivated;
    }

    //facade to deal with other classes asking
    public bool IsInteractionAllowed() {
        return isInteractionActivated;
    }

    public InventoryObject GetHeldItem() {
        return playerInteractor.GetHeldItem();
    }

    public InteractableObject GetStareAt() {
        return playerInteractor.GetStareAt();
    }

    public Vector3 GetStareAtPosition() {
        return playerInteractor.GetStareAtPosition();
    }

    public void UpdateHeldItem(InventoryObjectSO inventoryObjectSO, int equipmentID) {
        playerInteractor.UpdateHeldItem(inventoryObjectSO, equipmentID);
    }

    public Transform GetPhotoCameraMovePosition() {
        return photoCameraToPlayerEyePosition;
    }

    public Transform GetHoldPosition() {
        return playerInteractor.GetHoldPosition();
    }
}
