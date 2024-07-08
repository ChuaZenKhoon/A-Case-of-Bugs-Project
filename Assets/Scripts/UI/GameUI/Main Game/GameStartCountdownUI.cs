using System;
using TMPro;
using UnityEngine;

/**
 * A UI element that represents the game start countdown timer.
 */
public class GameStartCountdownUI : MonoBehaviour {

    public static GameStartCountdownUI Instance {  get; private set;}

    [SerializeField] private TextMeshProUGUI countdownTime;
    
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        CrimeSceneLevelManager.Instance.OnStateChange += LevelManager_OnStateChange;
        Hide();
    }

    private void LevelManager_OnStateChange(object sender, EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGameStartCountingDown()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Update() {
        countdownTime.text = Mathf.Ceil(CrimeSceneLevelManager.Instance.GetCountdownTime()).ToString();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
