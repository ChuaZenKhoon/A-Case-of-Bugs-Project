using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwabSFX : SFX {

    [SerializeField] private Swab swab;

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        swab.OnSwabUse += Swab_OnSwabUse;
    }

    private void Swab_OnSwabUse(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlaySwabUseSound(Player.Instance.GetHoldPosition().position, volMultiplier);   
    }
}
