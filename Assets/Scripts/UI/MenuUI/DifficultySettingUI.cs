using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultySettingUI : MonoBehaviour {

    [SerializeField] private Button easyModeButton;
    [SerializeField] private Button mediumModeButton;
    [SerializeField] private Button hardModeButton;
    [SerializeField] private Button enterGameButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI difficultyNameText;

    private void Awake() {
        easyModeButton.onClick.AddListener(() => {
            DifficultySettingManager.Instance.SetDifficulty(DifficultySettingManager.DifficultyLevel.Easy);
            UpdateDifficultyDescription(DifficultySettingManager.difficultyLevelSelected);
        });
        mediumModeButton.onClick.AddListener(() => {
            DifficultySettingManager.Instance.SetDifficulty(DifficultySettingManager.DifficultyLevel.Medium);
            UpdateDifficultyDescription(DifficultySettingManager.difficultyLevelSelected);
        });
        hardModeButton.onClick.AddListener(() => {
            DifficultySettingManager.Instance.SetDifficulty(DifficultySettingManager.DifficultyLevel.Hard);
            UpdateDifficultyDescription(DifficultySettingManager.difficultyLevelSelected);
        });
        enterGameButton.onClick.AddListener(() => {
            StartGame();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void StartGame() {
        SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
        Loader.targetScene = Loader.Scene.CrimeScene;
    }

    private void Start() {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void UpdateDifficultyDescription(DifficultySO difficultySO) {
        difficultyText.text = difficultySO.difficultyDescription;
        difficultyNameText.text = difficultySO.difficultyLevel.ToString();
    }
}
