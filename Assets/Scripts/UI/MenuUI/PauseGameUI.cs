using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * A UI element that represents the pause menu
 */
public class PauseGameUI : MonoBehaviour {

    [SerializeField] private OptionsMenuUI optionsMenuUI;
    [SerializeField] private GameInstructionsUI gameInstructionsUI;

    [SerializeField] private Button restartLevelButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Button closePauseMenuButton;
    
    [SerializeField] private Button instructionsMenuButton;

    //Add listeners to pause menu buttons
    private void Awake() {
        restartLevelButton.onClick.AddListener(() => {
            SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
        });

        optionsMenuButton.onClick.AddListener(() => {
            optionsMenuUI.Show();
        });

        returnToMainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
            Loader.targetScene = Loader.Scene.MainMenu;
        });

        closePauseMenuButton.onClick.AddListener(() => {
            PauseManager.Instance.TogglePause();
        });

        if (instructionsMenuButton != null) {
            instructionsMenuButton.onClick.AddListener(() => {
                gameInstructionsUI.Show();
            });
        }
    }

    //Subscribe to pause game events
    private void Start() {
        PauseManager.Instance.OnGamePause += PauseManager_OnGamePause;
        PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;
        Hide();
    }

    private void GameInstructionsUI_OnSkipAhead(object sender, EventArgs e) {
        Hide();
    }

    private void PauseManager_OnGameUnpause(object sender, EventArgs e) {
        gameInstructionsUI.Hide();
        Hide();
    }

    private void PauseManager_OnGamePause(object sender, EventArgs e) {
        Show();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}
