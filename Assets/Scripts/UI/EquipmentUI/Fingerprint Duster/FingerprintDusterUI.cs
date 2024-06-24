using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        usesLeftText.text = usesLeft.ToString() + " / 3";
    }

    private void OnDestroy() {
        fingerprintDuster.OnFingerprintDusterUse -= FingerprintDuster_OnFingerprintDusterUse;

    }
}
