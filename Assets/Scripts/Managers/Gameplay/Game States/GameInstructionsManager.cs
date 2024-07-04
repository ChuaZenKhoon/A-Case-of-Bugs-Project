using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A manager that handles the game instructions in the main game.
 */
public class GameInstructionsManager : MonoBehaviour {

    public static GameInstructionsManager Instance { get; private set; }

    public event EventHandler OnGameSkipAhead;

    [SerializeField] private GameInstructionsUI gameInstructionsUI;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;
    }

    private void PauseManager_OnGameUnpause(object sender, EventArgs e) {
        if (gameInstructionsUI.gameObject.activeSelf) {
            gameInstructionsUI.Hide();
        }
    }

    /**
     * Skips the main game of 15 minutes to the end of that game portion
     */
    public void SkipAhead() {
        OnGameSkipAhead?.Invoke(this, EventArgs.Empty);
        PauseManager.Instance.TogglePause();
    }
}
