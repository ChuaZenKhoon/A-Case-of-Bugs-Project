using UnityEngine;

/**
 * A manager superclass that handles Player movement, Camera movement and cursor lock 
 * based on current game state/substate.
 */
public class PlayerActionsManager : MonoBehaviour {

    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Texture2D cursorCrossHairSprite;

    protected bool canCameraMove;
    protected bool canPlayerMove;
    protected bool canPlayerInteract;
    protected bool canCursorMove;

    /**
     * Updates the mouse, camera and cursor movement based on the boolean states set.
     */
    protected void UpdatePlayerActionSettings() {
        if (canCursorMove) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset to default cursor
        } else {
            Vector2 cursorHotspot = new Vector2(cursorCrossHairSprite.width / 2f, cursorCrossHairSprite.height / 2f);
            Cursor.SetCursor(cursorCrossHairSprite, cursorHotspot, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        if (canCameraMove) {
            playerCamera.ToggleActivationState(true);
        } else {
            playerCamera.ToggleActivationState(false);
        }

        if (canPlayerMove) {
            Player.Instance.ToggleMovementState(true);
        } else {
            Player.Instance.ToggleMovementState(false);
        }

        if (canPlayerInteract) {
            Player.Instance.ToggleInteractionState(true);
        } else {
            Player.Instance.ToggleInteractionState(false);
        }
    }
}
