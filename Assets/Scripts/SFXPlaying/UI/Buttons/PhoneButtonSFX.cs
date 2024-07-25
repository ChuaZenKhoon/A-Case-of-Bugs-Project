using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneButtonSFX : ButtonSFX {

    protected override void PlaySpecificSound() {
        GameElementSFXPlayer.Instance.PlayPhoneTapSound(Player.Instance.GetHoldPosition().position, volMultiplier);
    }
}
