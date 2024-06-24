using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


/**
 * A manager in charge of handling key inputs from the user,
 * calling events when it happens
 */
public class GameInput : MonoBehaviour {

    public static GameInput Instance {  get; private set; }

    //Event for when movement key(s) is/are pressed
    public event EventHandler<Vector2> OnMove;

    //Event for when inventory key is pressed
    public event EventHandler OnInventoryKeyAction;

    //Event for when inventory bar slot key is pressed
    public event EventHandler<OnInventoryBarSelectEventArgs> OnInventoryBarSelect;

    //Event for when interact key is pressed
    public event EventHandler OnInteractAction;

    //Event for when pause key is pressed
    public event EventHandler OnPauseScreenAction;

    //Event for when mouse is moved
    public event EventHandler<Vector2> OnMouseMove;

    //Event for when left mouse click is pressed
    public event EventHandler OnTakePictureLeftClick;

    //Event for when interact2 key is pressed
    public event EventHandler OnInteract2Action;

    public event EventHandler OnUndoLine;

    public event EventHandler<Vector2> OnSketchMouseMove;


    public class OnInventoryBarSelectEventArgs : EventArgs {
        public InventoryBarSlot inventoryBarSlot;
    }

    public enum InventoryBarSlot {
        First,
        Second,
        Third,
        Fourth,
        Fifth
    }

    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        //Activate input action
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.CameraMovement.performed += CameraMovement_performed;
        playerInputActions.Player.CameraMovement.canceled += CameraMovement_canceled;

        playerInputActions.Player.Move.performed += Move_performed;
        playerInputActions.Player.Move.canceled += Move_canceled;

        playerInputActions.Player.InventoryBarSelect.performed += InventoryBarSelect_performed;
        playerInputActions.Player.Inventory.performed += InventoryAction_performed;

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Interact2.performed += Interact2_performed;

        playerInputActions.Player.PauseScreen.performed += PauseScreen_performed;

        playerInputActions.Player.TakePicture.performed += TakePicture_performed;
        
        playerInputActions.Player.UndoSketch.performed += UndoSketch_performed;
        playerInputActions.Player.MouseSketchPosition.performed += MouseSketchPosition_performed;
        playerInputActions.Player.MouseSketchPosition.canceled += MouseSketchPosition_canceled;

    }

    private void MouseSketchPosition_canceled(InputAction.CallbackContext obj) {
        OnSketchMouseMove?.Invoke(this, playerInputActions.Player.MouseSketchPosition.ReadValue<Vector2>());
    }

    private void MouseSketchPosition_performed(InputAction.CallbackContext obj) {
        OnSketchMouseMove?.Invoke(this, playerInputActions.Player.MouseSketchPosition.ReadValue<Vector2>());
    }

    private void UndoSketch_performed(InputAction.CallbackContext obj) {
        OnUndoLine?.Invoke(this, EventArgs.Empty);
    }

    private void Interact2_performed(InputAction.CallbackContext obj) {
        OnInteract2Action?.Invoke(this, EventArgs.Empty);
    }

    private void TakePicture_performed(InputAction.CallbackContext obj) {
        OnTakePictureLeftClick?.Invoke(this, EventArgs.Empty);
    }

    private void CameraMovement_canceled(InputAction.CallbackContext obj) {
        OnMouseMove?.Invoke(this, Vector2.zero);
    }

    private void CameraMovement_performed(InputAction.CallbackContext obj) {
        OnMouseMove?.Invoke(this, obj.ReadValue<Vector2>());
    }

    private void OnDestroy() {
        playerInputActions.Player.CameraMovement.performed -= CameraMovement_performed;
        playerInputActions.Player.CameraMovement.canceled -= CameraMovement_canceled;
        playerInputActions.Player.Move.performed -= Move_performed;
        playerInputActions.Player.Move.canceled -= Move_canceled;
        playerInputActions.Player.InventoryBarSelect.performed -= InventoryBarSelect_performed;
        playerInputActions.Player.Inventory.performed -= InventoryAction_performed;
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.PauseScreen.performed -= PauseScreen_performed;
        playerInputActions.Player.TakePicture.performed -= TakePicture_performed;
        playerInputActions.Player.Interact2.performed -= Interact2_performed;
        playerInputActions.Player.UndoSketch.performed -= UndoSketch_performed;
        playerInputActions.Player.MouseSketchPosition.performed -= MouseSketchPosition_performed;
        playerInputActions.Player.MouseSketchPosition.canceled -= MouseSketchPosition_canceled;
        playerInputActions.Dispose();
    }

    //For move, when key is pressed -> start movement
    //For move, when key not pressed -> stop movement
    private void Move_canceled(InputAction.CallbackContext obj) {
        OnMove?.Invoke(this, Vector2.zero);
    }

    private void Move_performed(InputAction.CallbackContext obj) {
        OnMove?.Invoke(this, playerInputActions.Player.Move.ReadValue<Vector2>().normalized);
    }

    private void PauseScreen_performed(InputAction.CallbackContext obj) {
        OnPauseScreenAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InventoryAction_performed(InputAction.CallbackContext obj) {
        OnInventoryKeyAction?.Invoke(this, EventArgs.Empty);
    }

    private void InventoryBarSelect_performed(InputAction.CallbackContext obj) {
        // Get the control associated with the action

        if (obj.control is KeyControl keyControl) {
            Key key = keyControl.keyCode;

            // Get the string representation of the key
            string keyString = key.ToString();
            InventoryBarSlot inventoryBarSlot;
            // Check if the key is one of the inventory slot keys
            switch (keyString) {
                default:
                case "Digit1" :
                    inventoryBarSlot = InventoryBarSlot.First;
                    break;
                case "Digit2":
                    inventoryBarSlot = InventoryBarSlot.Second;
                    break;
                case "Digit3":
                    inventoryBarSlot = InventoryBarSlot.Third;
                    break;
                case "Digit4":
                    inventoryBarSlot = InventoryBarSlot.Fourth;
                    break;
                case "Digit5":
                    inventoryBarSlot = InventoryBarSlot.Fifth;
                    break;
            }

            OnInventoryBarSelect.Invoke(this, new OnInventoryBarSelectEventArgs { inventoryBarSlot = inventoryBarSlot });
        }
    }

}
