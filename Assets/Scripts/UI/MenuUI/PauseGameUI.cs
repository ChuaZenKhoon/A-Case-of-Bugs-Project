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
            Time.timeScale = 1.0f;
        });

        optionsMenuButton.onClick.AddListener(() => {
            optionsMenuUI.Show();
        });

        returnToMainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
            Loader.targetScene = Loader.Scene.MainMenu;
            Time.timeScale = 1.0f;
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
        Hide();
    }

    public void UnpauseGame() {
        if (gameInstructionsUI != null) {
            gameInstructionsUI.Hide();
        }
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}
