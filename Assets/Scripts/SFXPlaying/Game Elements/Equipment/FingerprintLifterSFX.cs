using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerprintLifterSFX : SFX {

    [SerializeField] private FingerprintLifter fingerprintLifter;

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        fingerprintLifter.OnLiftFingerprint += FingerprintLifter_OnLiftFingerprint;
    }

    private void FingerprintLifter_OnLiftFingerprint(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayFingerprintLifterUseSound(Player.Instance.GetHoldPosition().position, volMultiplier);
    }
}
