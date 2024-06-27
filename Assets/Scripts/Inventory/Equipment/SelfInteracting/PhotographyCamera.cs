using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotographyCamera: SelfInteractingEquipment {

    [SerializeField] private Camera equipmentCamera;
    [SerializeField] private PhotographyCameraUI photographyCameraUI;
    [SerializeField] private GameObject cameraCrossHair;
    [SerializeField] private CanvasGroup gameplayCanvas;

    private Camera playerCamera;

    private bool isInCameraMode;

    private bool isCameraCooldownOver;

    private void Awake() {
        isInCameraMode = false;
        isCameraCooldownOver = true;
        equipmentCamera.enabled = false;
        playerCamera = Camera.main;
        cameraCrossHair.SetActive(false);
        GameObject canvas = GameObject.Find("Gameplay Canvas");
        gameplayCanvas = canvas.GetComponent<CanvasGroup>();
    }

    private void Start() {
        GameInput.Instance.OnTakePictureLeftClick += GameInput_OnTakePictureLeftClick;
    }

    private void OnDestroy() {
        GameInput.Instance.OnTakePictureLeftClick -= GameInput_OnTakePictureLeftClick;
    }
    private void GameInput_OnTakePictureLeftClick(object sender, System.EventArgs e) {
        TakePicture();
    }

    public override void Interact() {
        if (PhotographyCameraUI.IsInPhotoGalleryMode()) {
            MessageLogManager.Instance.LogMessage("Exit the Photo Gallery first before using the camera.");
            return;
        }

        if (InventoryScreenUI.isInAction) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using the camera");
            return;
        }
        
        if (!isInCameraMode) {
            GoIntoCameraMode();
        } else {
            ExitFromCameraMode();
        }
    }

    private void GoIntoCameraMode() {
        isInCameraMode = true;
        Equipment.isInAction = true;
        equipmentCamera.enabled = true;
        playerCamera.enabled = false;
        cameraCrossHair.SetActive(true);
        gameplayCanvas.alpha = 0f;
        Cursor.visible = false;
    }

    private void ExitFromCameraMode() {
        isInCameraMode = false;
        Equipment.isInAction = false;
        playerCamera.enabled = true;
        equipmentCamera.enabled = false;
        cameraCrossHair.SetActive(false);
        gameplayCanvas.alpha = 1f;
        Cursor.visible = true;
    }

    private void TakePicture() {
        if (isInCameraMode && isCameraCooldownOver) {
            StartCoroutine(CameraCooldownCoroutine());
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            equipmentCamera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;

            // Render the camera's view to the RenderTexture
            equipmentCamera.Render();

            EquipmentStorageManager.Instance.AddPhotoToPhotoGallery(renderTexture);

            // Reset the camera's target texture and the active render texture
            equipmentCamera.targetTexture = null;
            RenderTexture.active = null;

            MessageLogManager.Instance.LogMessage("Photo successfully taken!");
        } else if (isInCameraMode && !isCameraCooldownOver) {
            MessageLogManager.Instance.LogMessage("Camera still processing previous photo.");
        }
    }

    private IEnumerator CameraCooldownCoroutine() {
        isCameraCooldownOver = false;
        yield return new WaitForSeconds(1.5f);
        isCameraCooldownOver = true;
    }

    public bool IsInCameraMode() {
        return isInCameraMode;
    }
}