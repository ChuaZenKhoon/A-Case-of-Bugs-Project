using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealedEvidenceSFX : SFX {

    [SerializeField] private SealedEvidence sealedEvidence;

    private void Awake() {
        volMultiplier = 0.9f;
    }

    private void Start() {
        sealedEvidence.OnPickUp += SealedEvidence_OnPickUp;
    }

    private void SealedEvidence_OnPickUp(object sender, System.EventArgs e) {
        SFXPlayer.Instance.PlaySealEvidenceSound(Player.Instance.transform.position, volMultiplier);
    }
}
