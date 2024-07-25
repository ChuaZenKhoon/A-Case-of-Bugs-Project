using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotographyCameraAnimation : MonoBehaviour {

    [SerializeField] private PhotoCapture photoCapture;

    [SerializeField] private Animator animator;

    private Coroutine takingPictureCoroutine;

    private void Start() {
        photoCapture.OnPhotoTaken += PhotoCapture_OnPhotoTaken;
    }

    private void PhotoCapture_OnPhotoTaken(object sender, System.EventArgs e) {
        if (takingPictureCoroutine != null) {
            return;
        }
        
        animator.SetBool("isTakingPicture", true);

        // Optionally, you can reset the boolean after the animation duration
        // Here we assume the TakePicture animation lasts 1 second
        takingPictureCoroutine = StartCoroutine(ResetAnimationTrigger(1.5f));
    }

    private IEnumerator ResetAnimationTrigger(float delay) {
        yield return new WaitForSeconds(delay);
        animator.SetBool("isTakingPicture", false);
        takingPictureCoroutine = null;
    }
}
