using System;
using UnityEngine;

/**
 * The class representing the photography camera equipment, encompasses its components.
 */
public class PhotographyCamera: SelfInteractingEquipment {

    public static event EventHandler OnOpenPhotoGallery;

    new public static void ResetStaticData() {
        OnOpenPhotoGallery = null;
    }

    [SerializeField] private PhotoCapture photoCapture;
    [SerializeField] private PhotoGallery photoGallery;
    
    [SerializeField] private CanvasGroup gameplayCanvas;

    [SerializeField] private BoxCollider cameraCollider;
    [SerializeField] private LayerMask mainPathMask;
    private CapsuleCollider playerCollider;

    private Transform holdPosition;
    private Transform CameraAtEyePosition;

    private bool isInCameraMode;
    private bool isInGalleryMode;

    private void Awake() {
        cameraCollider.enabled = false;
        isInCameraMode = false;
        isInGalleryMode = false;
        GameObject canvas = GameObject.Find("Gameplay Canvas");
        gameplayCanvas = canvas.GetComponent<CanvasGroup>();
        GameObject player = GameObject.Find("Player");
        playerCollider = player.GetComponent<CapsuleCollider>();
    }

    private void Start() {
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
    }
    private void OnDestroy() {
        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;
    }

    //Next 3 methods deal with using the camera mode
    public override void Interact() {
        if (isInGalleryMode) {
            MessageLogManager.Instance.LogMessage("Exit the Photo Gallery first before using the camera.");
            return;
        }

        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using the camera");
            return;
        }
        
        if (!isInCameraMode) {
            OpenCameraMode();
        } else {
            CloseCameraMode();
        }
    }

    private void OpenCameraMode() {
        if (CheckObstructionToHoldCamera()) {
            MessageLogManager.Instance.LogMessage("Move away from any nearby wall or objects before holding up the camera to your eye.");
            return;
        }
        Equipment.IS_IN_ACTION = true;
        isInCameraMode = true;
        holdPosition = Player.Instance.GetHoldPosition();
        CameraAtEyePosition = Player.Instance.GetPhotoCameraMovePosition();
        this.gameObject.transform.position = CameraAtEyePosition.position;
        gameplayCanvas.alpha = 0f;
        cameraCollider.enabled = true;
        photoCapture.GoIntoCameraMode();
    }

    //Checks for sufficient space to hold up camera to prevent wall clipping
    //Except invisble colliders such as main path and player
    private bool CheckObstructionToHoldCamera() {
        Vector3 center = Player.Instance.GetPhotoCameraMovePosition().position;
        Vector3 halfExtents = cameraCollider.bounds.extents;

        Collider[] overlaps = Physics.OverlapBox(center, halfExtents, Quaternion.identity, ~mainPathMask);

        bool isObstructed = false;
        if (overlaps.Length > 0) {
            foreach (Collider b in overlaps) {
                if (b == playerCollider) {
                    continue;
                } else {
                    isObstructed = true;
                    break;
                }
            }
        }

        return isObstructed;
    }

    private void CloseCameraMode() {
        Equipment.IS_IN_ACTION = false;
        isInCameraMode = false;
        this.gameObject.transform.position = holdPosition.position;
        gameplayCanvas.alpha = 1f;
        cameraCollider.enabled = false;
        photoCapture.ExitFromCameraMode();
    }

    //Next 3 methods deal with using the photo gallery
    private void GameInput_OnInteract2Action(object sender, System.EventArgs e) {
        if (isInCameraMode) {
            MessageLogManager.Instance.LogMessage("Please exit camera mode first before viewing the photo gallery.");
            return;
        }

        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before viewing photo gallery.");
            return;
        }

        if (!isInGalleryMode) {
            if (EquipmentStorageManager.Instance.GetPhotoGallery().Count > 0) {
                OpenPhotoGallery();
            } else {
                MessageLogManager.Instance.LogMessage("There are no photographs available for viewing.");
            }
        } else {
            ClosePhotoGallery();
        }
    }

    private void OpenPhotoGallery() {
        Equipment.IS_IN_ACTION = true;
        isInGalleryMode = true;
        holdPosition = Player.Instance.GetHoldPosition();
        CameraAtEyePosition = Player.Instance.GetPhotoCameraMovePosition();
        this.gameObject.transform.position = CameraAtEyePosition.position;
        gameplayCanvas.alpha = 0f;
        photoGallery.ViewPhotoGallery();
        OnOpenPhotoGallery?.Invoke(this, EventArgs.Empty);
    }

    public void ClosePhotoGallery() {
        Equipment.IS_IN_ACTION = false;
        isInGalleryMode = false;
        gameplayCanvas.alpha = 1f;
        this.gameObject.transform.position = holdPosition.position;
        photoGallery.ClosePhotoGallery();
        OnOpenPhotoGallery?.Invoke(this, EventArgs.Empty);
    }

    public bool IsInCameraMode() {
        return isInCameraMode;
    }

    public bool IsInGalleryMode() {
        return isInGalleryMode;
    }
}