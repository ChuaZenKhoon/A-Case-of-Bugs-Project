using System;
using System.Collections;
using UnityEngine;

/**
 * A logic component of the photography camera that handles the function of taking photos.
 */
public class PhotoCapture : MonoBehaviour {

    public event EventHandler OnPhotoTaken;
    public event EventHandler OnCameraStillRefocus;

    [SerializeField] private PhotographyCamera photographyCamera;

    [SerializeField] private Camera equipmentCamera;
    [SerializeField] private GameObject cameraCrossHair;


    private Camera playerCamera;
    private bool isCameraCooldownOver;

    private void Awake() {
        isCameraCooldownOver = true;
        equipmentCamera.enabled = false;
        playerCamera = Camera.main;
        cameraCrossHair.SetActive(false); 
    }

    private void Start() {
        GameInput.Instance.OnTakePictureLeftClick += GameInput_OnTakePictureLeftClick;
    }

    private void OnDestroy() {
        GameInput.Instance.OnTakePictureLeftClick -= GameInput_OnTakePictureLeftClick;
    }

    public void GoIntoCameraMode() {
        equipmentCamera.enabled = true;
        playerCamera.enabled = false;
        cameraCrossHair.SetActive(true);
        Cursor.visible = false;
    }

    public void ExitFromCameraMode() {
        playerCamera.enabled = true;
        equipmentCamera.enabled = false;    
        cameraCrossHair.SetActive(false);
        Cursor.visible = true;
    }

    private void GameInput_OnTakePictureLeftClick(object sender, System.EventArgs e) {
        TakePicture();
    }

    private void TakePicture() {
        if (!photographyCamera.IsInCameraMode()) {
            return;
        }

        if (isCameraCooldownOver) {
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

            OnPhotoTaken?.Invoke(this, EventArgs.Empty);
            MessageLogManager.Instance.LogMessage("Photo successfully taken!");
        } else if (!isCameraCooldownOver) {
            OnCameraStillRefocus?.Invoke(this, EventArgs.Empty);
            MessageLogManager.Instance.LogMessage("Camera still refocusing.");
        }
    }
    private IEnumerator CameraCooldownCoroutine() {
        isCameraCooldownOver = false;
        yield return new WaitForSeconds(1.5f);
        isCameraCooldownOver = true;
    }
}
