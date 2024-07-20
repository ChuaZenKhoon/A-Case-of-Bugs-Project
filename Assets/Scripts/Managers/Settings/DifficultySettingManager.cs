using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * A manager that handles the difficulty logic of the game.
 */
public class DifficultySettingManager : MonoBehaviour {

    public static DifficultySettingManager Instance { get; private set; }

    public static DifficultySO difficultyLevelSelected;

    private DifficultySettingUI difficultySettingUI;

    [SerializeField] private DifficultySO[] difficultySets;
    public enum DifficultyLevel {
        Easy,
        Medium,
        Hard
    }

    //Persistent Singleton
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SetUpDifficultySettingsUI();
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        SetUpDifficultySettingsUI();
    }

    private void SetUpDifficultySettingsUI() {
        DifficultySettingUI difficultySettingsUIFound = GameObject.FindObjectOfType<DifficultySettingUI>();

        if (difficultySettingsUIFound != null) {
            difficultySettingUI = difficultySettingsUIFound;
        }
    }

    public void SetDifficulty(DifficultyLevel difficulty) {
        foreach (DifficultySO difficultySO in difficultySets) {
            if (difficultySO.difficultyLevel == difficulty) {
                difficultyLevelSelected = difficultySO;
            }
        }
    }

}
