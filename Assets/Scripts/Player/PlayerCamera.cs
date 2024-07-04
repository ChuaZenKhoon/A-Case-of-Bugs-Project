using UnityEngine;

/**
 * A component of the Player that handles the camera movement.
 */
public class PlayerCamera : MonoBehaviour {

    [SerializeField] private Transform playerCameraPos;
    [SerializeField] private Camera viewCamera;

    
    private const float xSensitivity = 0.4f;
    private const float ySensitivity = 1.0f;
    
    //Mouse sensitivity setting
    private float sensitivityMultiplier;

    private Vector2 mouseMove;

    private float horizonViewVolume;
    private float verticalTilt;

    //Subscribe to events
    private void Start() {
        GameInput.Instance.OnMouseMove += GameInput_OnMouseMove;
        GameSettingsManager.Instance.OnMouseSensitivityChange += GameSettingsManager_OnMouseSensitivityChange;
        
        //attach camera to player
        viewCamera.transform.position = playerCameraPos.position;

        //Game setting
        sensitivityMultiplier = GameSettingsManager.Instance.GetScaledSensitivity();
    }

    private void GameSettingsManager_OnMouseSensitivityChange(object sender, float e) {
        sensitivityMultiplier = e;
    }

    private void GameInput_OnMouseMove(object sender, Vector2 e) {
        mouseMove = e;
    }

    /**
     * Updates camera orientation based on mouse movement.
     */
    public void HandleCameraMovement() {
        viewCamera.transform.position = playerCameraPos.position;

        horizonViewVolume += mouseMove.x * xSensitivity * sensitivityMultiplier;
        verticalTilt -= mouseMove.y * ySensitivity * sensitivityMultiplier;

        verticalTilt = Mathf.Clamp(verticalTilt, -90f, 90f);

        //Rotation is about the axis, so Y rotate then xz change, and so on
        viewCamera.transform.rotation = Quaternion.Euler(verticalTilt, horizonViewVolume, 0);

        //Update held item rotation to match camera to prevent weird look
        InventoryObject heldItem = viewCamera.GetComponentInChildren<InventoryObject>();
        if (heldItem != null ) {
            heldItem.transform.rotation = viewCamera.transform.rotation;
        }
    }

    public Camera GetViewCamera() {
        return viewCamera;
    }
}
