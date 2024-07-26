using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerprintDusterAnimation : CustomAnimation {

    [SerializeField] private FingerprintDuster fingerprintDuster;

    private const string DUSTING_ANIMATION_NAME = "DustArea";
    private const string DUSTING_ANIMATION_BOOL = "isDusting";

    private Coroutine dustingCoroutine;

    private void Awake() {
        dustingCoroutine = null;
    }

    private void Start() {
        fingerprintDuster.OnFingerprintDusterUse += FingerprintDuster_OnFingerprintDusterUse;
    }

    private void FingerprintDuster_OnFingerprintDusterUse(object sender, int e) {
        if (dustingCoroutine != null) {
            StopCoroutine(dustingCoroutine);
        }

        animator.Play(DUSTING_ANIMATION_NAME, 0, 0f);
        animator.SetBool(DUSTING_ANIMATION_BOOL, true);
        dustingCoroutine = StartCoroutine(DustingCoroutine());
    }

    private IEnumerator DustingCoroutine() {
        yield return new WaitForSeconds(1f);
        animator.SetBool(DUSTING_ANIMATION_BOOL, false);
        dustingCoroutine = null;
    }
}
