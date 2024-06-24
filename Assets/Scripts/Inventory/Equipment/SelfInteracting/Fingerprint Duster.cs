using System;
using UnityEngine;

public class FingerprintDuster : SelfInteractingEquipment {

    public event EventHandler<int> OnFingerprintDusterUse;

    private static int USES_LEFT = 3;

    private const float DUSTING_RADIUS = 2f;

    private const string FINGERPRINT_TAG = "Fingerprint";

    [SerializeField] private LayerMask dustingLayer;

    new public static void ResetStaticData() {
        USES_LEFT = 3;
    }

    public override void Interact() {

        if (USES_LEFT > 0) {
            Vector3 currentStareAt = Player.Instance.GetStareAtPosition();

            //dusts area around lookat spot
            Collider[] hitColliders = Physics.OverlapSphere(currentStareAt, DUSTING_RADIUS, dustingLayer);

            foreach (Collider hitCollider in hitColliders) {
                // Check if the object has the target tag (optional if using layers)
                if (hitCollider.CompareTag(FINGERPRINT_TAG)) {
                    // Apply the effect to the target
                    Fingerprint fingerprint = hitCollider.GetComponent<Fingerprint>();
                    RevealFingerprint(fingerprint);
                }
            }

            USES_LEFT -= 1;
            OnFingerprintDusterUse?.Invoke(this, USES_LEFT);
            MessageLogManager.Instance.LogMessage("Area dusted.");
        } else {
            MessageLogManager.Instance.LogMessage("No more dusting powder left.");
        }
    }

    private void RevealFingerprint(Fingerprint fingerprint) {
        fingerprint.RevealSelf();
    }

    public int GetUsesLeft() {
        return USES_LEFT;
    }

}
