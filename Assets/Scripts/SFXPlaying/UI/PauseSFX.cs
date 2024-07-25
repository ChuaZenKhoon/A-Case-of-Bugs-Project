/**
 * A component of the pause menu that handles its sound effects.
 */
public class PauseSFX : SFX {

    private void Awake() {
        volMultiplier = 1.0f;
    }

    private void Start() {
        PauseManager.Instance.OnGamePause += PauseManager_OnGamePause;
        PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;
    }

    private void PauseManager_OnGameUnpause(object sender, System.EventArgs e) {
        UISFXPlayer.Instance.PlayResumeGameSound(volMultiplier);
    }

    private void PauseManager_OnGamePause(object sender, System.EventArgs e) {
        UISFXPlayer.Instance.PlayPauseGameSound(volMultiplier);
    }
}
