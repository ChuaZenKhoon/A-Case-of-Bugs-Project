using UnityEngine;

/**
 * Controls the logic of the first person perspective camera
 * attached to the Player.
 */
public class PlayerCamera : MonoBehaviour {

    [SerializeField] private Transform playerCameraPos;
    [SerializeField] private Camera viewCamera;

    //Option to change mouse sensitivity later?
    private const float xSensitivity = 0.4f;
    private const float ySensitivity = 1.0f;
    private float sensitivityMultiplier;

    private Vector2 mouseMove;

    private float horizonViewVolume;
    private float verticalTilt;

    private bool isCameraAllowedToMove;

    //Subscribe to events
    private void Start() {
        GameInput.Instance.OnMouseMove += GameInput_OnMouseMove;
        GameSettingsManager.Instance.OnMouseSensitivityChange += GameSettingsManager_OnMouseSensitivityChange;
        
        //attach camera to player
        viewCamera.transform.position = playerCameraPos.position;

        sensitivityMultiplier = GameSettingsManager.Instance.GetScaledSensitivity();
    }

    private void GameSettingsManager_OnMouseSensitivityChange(object sender, float e) {
        sensitivityMultiplier = e;
    }

    private void GameInput_OnMouseMove(object sender, Vector2 e) {
        mouseMove = e;
    }

    private void Update() {
        HandleCameraMovement();
    }

    /**
     * Updates camera orientation based on mouse movement.
     */
    private void HandleCameraMovement() {

        if (isCameraAllowedToMove) {
            viewCamera.transform.position = playerCameraPos.position;

            horizonViewVolume += mouseMove.x * xSensitivity * sensitivityMultiplier;
            verticalTilt -= mouseMove.y * ySensitivity * sensitivityMultiplier;

            verticalTilt = Mathf.Clamp(verticalTilt, -90f, 90f);

            //Rotation is about the axis, so Y rotate then xz change, and so on
            viewCamera.transform.rotation = Quaternion.Euler(verticalTilt, horizonViewVolume, 0);

            //Update held item rotation to match camera to prevent weird look
            InventoryObject heldItem = viewCamera.GetComponentInChildren<InventoryObject>();
            if ( heldItem != null ) {
                heldItem.transform.rotation = viewCamera.transform.rotation;
            }
        }
    }

    public void ToggleActivationState(bool isActivated) {
        isCameraAllowedToMove = isActivated;
    }
}
