using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * A UI element that represents the difficulty settings of the game.
 */
public class DifficultySettingUI : MonoBehaviour {

    [SerializeField] private Button easyModeButton;
    [SerializeField] private Button mediumModeButton;
    [SerializeField] private Button hardModeButton;
    [SerializeField] private Button enterGameButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake() {
        easyModeButton.onClick.AddListener(() => {
            DifficultySettingManager.Instance.SetDifficulty(DifficultySettingManager.DifficultyLevel.Easy);
            enterGameButton.gameObject.SetActive(true);
        });
        mediumModeButton.onClick.AddListener(() => {
            DifficultySettingManager.Instance.SetDifficulty(DifficultySettingManager.DifficultyLevel.Medium);
            enterGameButton.gameObject.SetActive(true);
        });
        hardModeButton.onClick.AddListener(() => {
            DifficultySettingManager.Instance.SetDifficulty(DifficultySettingManager.DifficultyLevel.Hard);
            enterGameButton.gameObject.SetActive(true);
        });
        enterGameButton.onClick.AddListener(() => {
            StartGame();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
        });


    }

    //Sets target scene to main game, scene to load async set, then goes to loading screen
    private void StartGame() {
        SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
        Loader.targetScene = Loader.Scene.CrimeScene;
    }

    private void Start() {
        Hide();
    }

    private void Hide() {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Show() {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        enterGameButton.gameObject.SetActive(false);
    }
}
