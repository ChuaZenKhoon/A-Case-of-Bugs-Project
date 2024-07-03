using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A manager that handles the pausing logic of the game.
 */
public class PauseManager : MonoBehaviour {
    
    public static PauseManager Instance { get; private set; }

    //Event for when game is paused
    public event EventHandler OnGamePause;

    //Event for when game is unpaused
    public event EventHandler OnGameUnpause;

    [SerializeField] private PauseGameUI pauseGameUI;

    private bool isPaused;
    private void Awake() {
        Instance = this;

        isPaused = false;

        //Case where game is paused then level is restarted
        Time.timeScale = 1.0f;
    }

    private void Start() {
        GameInput.Instance.OnPauseScreenAction += GameInput_OnPauseExitScreen;
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
            pauseGameUI.Show();
            OnGamePause?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            pauseGameUI.UnpauseGame();
            OnGameUnpause?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePaused() {
        return isPaused;
    }
}
