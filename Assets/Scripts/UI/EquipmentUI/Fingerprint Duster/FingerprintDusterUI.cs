using TMPro;
using UnityEngine;

/**
 * A UI component representing the uses left displayed to the player
 * as the player uses the fingerprint duster equipment.
 */ 
public class FingerprintDusterUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI usesLeftText;
    [SerializeField] private FingerprintDuster fingerprintDuster;

    private void Start() {
        fingerprintDuster.OnFingerprintDusterUse += FingerprintDuster_OnFingerprintDusterUse;
        UpdateText(fingerprintDuster.GetUsesLeft());
    }

    private void FingerprintDuster_OnFingerprintDusterUse(object sender, int e) {
        UpdateText(e);
    }

    private void UpdateText(int usesLeft) {
        usesLeftText.text = usesLeft.ToString() + " / " + fingerprintDuster.GetUsesAllowed().ToString();
    }

    private void OnDestroy() {
        fingerprintDuster.OnFingerprintDusterUse -= FingerprintDuster_OnFingerprintDusterUse;
    }
}
