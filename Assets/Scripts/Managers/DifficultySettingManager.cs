using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySettingManager : MonoBehaviour {

    public static DifficultySettingManager Instance { get; private set; }

    public static DifficultySO difficultyLevelSelected;

    [SerializeField] private DifficultySO[] difficultySets;
    public enum DifficultyLevel {
        Easy,
        Medium,
        Hard
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
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
