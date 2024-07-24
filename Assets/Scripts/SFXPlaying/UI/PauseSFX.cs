using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSFX : SFX {

    private void Awake() {
        volMultiplier = 1.0f;
    }

    private void Start() {
        PauseManager.Instance.OnGamePause += PauseManager_OnGamePause;
        PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;
    }

    private void PauseManager_OnGameUnpause(object sender, System.EventArgs e) {
        SFXPlayer.Instance.PlayResumeGameSound(volMultiplier);
    }

    private void PauseManager_OnGamePause(object sender, System.EventArgs e) {
        SFXPlayer.Instance.PlayPauseGameSound(volMultiplier);
    }
}
