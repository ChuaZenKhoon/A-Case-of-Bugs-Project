using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLocationArrowSFX : SFX {

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        NextLocationArrow.OnTouchArrow += NextLocationArrow_OnTouchArrow;
    }

    private void NextLocationArrow_OnTouchArrow(object sender, int e) {
        GameElementSFXPlayer.Instance.PlayNextArrowReachedSound(volMultiplier);
    }
}
