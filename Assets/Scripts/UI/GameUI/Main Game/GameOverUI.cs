using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the game over screen
 */
public class GameOverUI : MonoBehaviour {

    public static GameOverUI Instance { get; private set; }

    [SerializeField] private Button nextButton;

    private void Awake() {
        Instance = this;
        nextButton.onClick.AddListener(() => {  
            CrimeSceneLevelManager.Instance.MoveToLab();
            Hide();
        });
    }

    //Subscribe to game state change event
    private void Start() {
        CrimeSceneLevelManager.Instance.OnStateChange += LevelManager_OnStateChange;
        Hide();
    }

    private void LevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGameOver()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
