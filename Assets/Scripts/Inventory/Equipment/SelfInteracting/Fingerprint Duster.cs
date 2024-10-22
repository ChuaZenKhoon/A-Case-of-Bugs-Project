using System;
using UnityEngine;

/**
 * The class representing the fingerprint duster equipment.
 */
public class FingerprintDuster : SelfInteractingEquipment {

    public event EventHandler<int> OnFingerprintDusterUse;

    private const int USES_ALLOWED = 5;
    private const float DUSTING_RADIUS = 2f;
    private const string FINGERPRINT_TAG = "Fingerprint";

    private static int USES_LEFT;

    [SerializeField] private LayerMask dustingLayer;

    new public static void ResetStaticData() {
        USES_LEFT = 5;
    }

    public override void Interact() {

        if (USES_LEFT > 0) {
            DustArea();
            MessageLogManager.Instance.LogMessage("Area dusted.");
        } else {
            MessageLogManager.Instance.LogMessage("No more dusting powder left.");
        }
    }

    /**
     * Checks the area around where player is looking at to uncover any hidden fingerprints.
     */
    private void DustArea() {
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
    }

    private void RevealFingerprint(Fingerprint fingerprint) {
        fingerprint.RevealSelf();
    }

    public int GetUsesLeft() {
        return USES_LEFT;
    }

    public int GetUsesAllowed() {
        return USES_ALLOWED;
    }

}
