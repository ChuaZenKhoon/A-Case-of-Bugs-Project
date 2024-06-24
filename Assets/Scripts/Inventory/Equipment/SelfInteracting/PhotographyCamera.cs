using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotographyCamera: SelfInteractingEquipment {

    [SerializeField] private Camera equipmentCamera;
    [SerializeField] private PhotographyCameraUI photographyCameraUI;
    [SerializeField] private Image cameraCrossHair;
    [SerializeField] private TextMeshProUGUI clickText;
    [SerializeField] private CanvasGroup gameplayCanvas;

    private Camera playerCamera;

    private bool isInCameraMode;

    private void Awake() {
        isInCameraMode = false;
        equipmentCamera.enabled = false;
        playerCamera = Camera.main;
        cameraCrossHair.enabled = false;
        clickText.enabled = false;
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
        cameraCrossHair.enabled = true;
        clickText.enabled = true;
        gameplayCanvas.alpha = 0f;
    }

    private void ExitFromCameraMode() {
        isInCameraMode = false;
        Equipment.isInAction = false;
        playerCamera.enabled = true;
        equipmentCamera.enabled = false;
        cameraCrossHair.enabled = false;
        clickText.enabled = false;
        gameplayCanvas.alpha = 1f;
    }

    private void TakePicture() {
        if (isInCameraMode) {
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
        }
    }

    public bool IsInCameraMode() {
        return isInCameraMode;
    }
}