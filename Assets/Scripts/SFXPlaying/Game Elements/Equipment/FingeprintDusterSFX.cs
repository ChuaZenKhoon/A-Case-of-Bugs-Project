using UnityEngine;

/**
 * A component of the fingerprint duster that handles its sound effects.
 */
public class FingeprintDusterSFX : SFX {

    [SerializeField] private FingerprintDuster fingerprintDuster;

    private void Awake() {
        volMultiplier = 3.5f;
    }

    private void Start() {
        fingerprintDuster.OnFingerprintDusterUse += FingerprintDuster_OnFingerprintDusterUse;
    }

    private void FingerprintDuster_OnFingerprintDusterUse(object sender, int e) {
        GameElementSFXPlayer.Instance.PlayFingerprintDustingSound(volMultiplier);
    }
}
