using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * A UI component representing the message screen displayed to the player when transitioning
 * to the lab portion of the game and during that portion.
 */
public class LabSceneMessageUI : MonoBehaviour {

    public static event EventHandler OnMessageClickFinish;

    [SerializeField] private TextMeshProUGUI welcomeMessage;
    [SerializeField] private TextMeshProUGUI leavingMessage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button leaveButton;

    public static void ResetStaticData() {
        OnMessageClickFinish = null;
    }

    private void Awake() {
        nextButton.onClick.AddListener(() => {
            Hide();
            OnMessageClickFinish?.Invoke(this, EventArgs.Empty);
        });
        leaveButton.onClick.AddListener(() => {
            SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
            Loader.targetScene = Loader.Scene.MainMenu;
        });
    }

    private void Start() {
        CrimeSceneLevelManager.Instance.OnStateChange += CrimsSceneLevelManager_OnStateChange;
        Hide();
    }

    private void CrimsSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsLabStarted()) {
            Show();
        }

        if (CrimeSceneLevelManager.Instance.IsLabEnded()) {
            Show();
            welcomeMessage.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            leavingMessage.gameObject.SetActive(true);
            leaveButton.gameObject.SetActive(true);
        }
    }
    
    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
