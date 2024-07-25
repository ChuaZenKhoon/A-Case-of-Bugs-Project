using System.Collections;
using UnityEngine;

/**
 * A component of the photographyCamera that handles its sound effects.
 */
public class PhotographyCameraSFX : SFX {

    [SerializeField] private PhotoCapture photoCapture;
    [SerializeField] private PhotoGallery photoGallery;

    private Coroutine cameraCooldownCoroutine;

    private void Awake() {
        volMultiplier = 1f;
        cameraCooldownCoroutine = null;
    }

    private void Start() {
        photoCapture.OnPhotoTaken += PhotoCapture_OnPhotoTaken;
        photoCapture.OnCameraStillRefocus += PhotoCapture_OnCameraStillRefocus;
        photoGallery.OnShufflePicButtonClick += PhotoGallery_OnShufflePicButtonClick;
    }

    private void PhotoGallery_OnShufflePicButtonClick(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayCameraGalleryShufflePicsSound(photoGallery.transform.position, volMultiplier);
    }

    private void PhotoCapture_OnCameraStillRefocus(object sender, System.EventArgs e) {
        if(cameraCooldownCoroutine != null) {
            return;    
        }
        cameraCooldownCoroutine = StartCoroutine(CameraCooldown());
    }
    private void PhotoCapture_OnPhotoTaken(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayTakePictureSound(photoCapture.transform.position, volMultiplier);
    }

    private IEnumerator CameraCooldown() {
        GameElementSFXPlayer.Instance.PlayCameraRefocusSound(photoCapture.transform.position, volMultiplier);
        yield return new WaitForSeconds(0.5f);
        cameraCooldownCoroutine = null;
    }


}
