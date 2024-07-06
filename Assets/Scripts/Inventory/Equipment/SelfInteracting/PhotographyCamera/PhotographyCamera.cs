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

    private bool isInCameraMode;
    private bool isInGalleryMode;

    private void Start() {
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
        isInCameraMode = false;
        isInGalleryMode = false;
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
        Equipment.isInAction = true;
        isInCameraMode = true;
        photoCapture.GoIntoCameraMode();
    }

    private void CloseCameraMode() {
        Equipment.isInAction = false;
        isInCameraMode = false;
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
        Equipment.isInAction = true;
        isInGalleryMode = true;
        photoGallery.ViewPhotoGallery();
        OnOpenPhotoGallery?.Invoke(this, EventArgs.Empty);
    }

    public void ClosePhotoGallery() {
        Equipment.isInAction = false;
        isInGalleryMode = false;
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