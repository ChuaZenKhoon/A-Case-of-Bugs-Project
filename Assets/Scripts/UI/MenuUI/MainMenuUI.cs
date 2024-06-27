using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * A UI element representing the main menu.
 */
public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private OptionsMenuUI optionsMenuUI;
    [SerializeField] private DifficultySettingUI difficultySettingUI;
    [SerializeField] private CreditsScreenUI creditsScreenUI;

    //Add listeners to main menu buttons
    private void Awake() {
        playButton.onClick.AddListener(() => {
            SelectDifficulty();
        });
        tutorialButton.onClick.AddListener(() => {
            StartTutorial();
        });
        optionsButton.onClick.AddListener(() => {
            optionsMenuUI.Show();
        });
        creditsButton.onClick.AddListener(() => {
            creditsScreenUI.Show();
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    public void SelectDifficulty() {
        difficultySettingUI.Show();
    }

    private void StartTutorial() {
        SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
        Loader.targetScene = Loader.Scene.TutorialScene;
    }
}
