/**
 * A component of the UI buttons attached to paper instruction UI images that handles its sound effects.
 */
public class PaperButtonSFX : ButtonSFX {
    
    protected override void PlaySpecificSound() {
        UISFXPlayer.Instance.PlayPaperFlipButtonSound(base.volMultiplier);
    }

}
