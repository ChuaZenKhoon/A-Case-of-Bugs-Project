using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotographyCameraAnimation : CustomAnimation {

    [SerializeField] private PhotoCapture photoCapture;

    private const string TAKING_PICTURE_ANIMATION_BOOLEAN = "isTakingPicture";

    private Coroutine takingPictureCoroutine;

    private void Start() {
        photoCapture.OnPhotoTaken += PhotoCapture_OnPhotoTaken;
    }

    private void PhotoCapture_OnPhotoTaken(object sender, System.EventArgs e) {
        if (takingPictureCoroutine != null) {
            return;
        }
        
        animator.SetBool(TAKING_PICTURE_ANIMATION_BOOLEAN, true);
        takingPictureCoroutine = StartCoroutine(ResetAnimationTrigger(1.5f));
    }

    private IEnumerator ResetAnimationTrigger(float delay) {
        yield return new WaitForSeconds(delay);
        animator.SetBool(TAKING_PICTURE_ANIMATION_BOOLEAN, false);
        takingPictureCoroutine = null;
    }
}
