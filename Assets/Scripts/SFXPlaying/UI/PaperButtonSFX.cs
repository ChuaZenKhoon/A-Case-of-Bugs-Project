using UnityEngine;

public class PaperButtonSFX : ButtonSFX {
    
    protected override void PlaySpecificSound() {
        SFXPlayer.Instance.PlayPaperFlipButtonSound(base.volMultiplier);
    }

}
