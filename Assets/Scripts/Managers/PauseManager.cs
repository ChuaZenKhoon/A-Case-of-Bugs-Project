using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    
    public static PauseManager Instance { get; private set; }

    //Event for when game is paused
    public event EventHandler OnGamePause;

    //Event for when game is unpaused
    public event EventHandler OnGameUnpause;

    private bool isPaused = false;
    private void Awake() {
        Instance = this;

        //Case where game is paused then level is restarted
        Time.timeScale = 1.0f;
    }

    private void Start() {
        GameInput.Instance.OnPauseScreenAction += GameInput_OnPauseExitScreen;
        GameInstructionsUI.OnSkipAhead += GameInstructionsUI_OnSkipAhead;
    }

    private void GameInstructionsUI_OnSkipAhead(object sender, EventArgs e) {
        TogglePause();
    }

    private void GameInput_OnPauseExitScreen(object sender, EventArgs e) {
        TogglePause();
    }

    /**
      * Executes pause logic when pause event is fired and received
      */
    public void TogglePause() {
        isPaused = !isPaused;
        if (isPaused) {
            Time.timeScale = 0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpause?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePaused() {
        return isPaused;
    }
}
