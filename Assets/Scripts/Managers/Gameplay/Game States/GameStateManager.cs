

using UnityEngine;

/**
 * A manager that handles the player actions based on the state of the game.
 */
public class GameStateManager : PlayerActionsManager {

    public static GameStateManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        canCameraMove = false;
        canPlayerMove = false;
        canPlayerInteract = false;
        canCursorMove = true;
    }

    private void Start() {
        SubscribeToGameStates();
    }

    private void SubscribeToGameStates() {

        //Game Pause State
        PauseManager.Instance.OnGamePause += PauseManager_OnGamePause;
        PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;


        //Game Play State
        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
        }
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
        }
    }

    /**
     * Checks the current game state and updates the boolean values for setting later.
     */
    private void CheckGameState() {
        bool isGamePaused = PauseManager.Instance.IsGamePaused();
        bool isGamePlaying = false;

        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            isGamePlaying = CrimeSceneLevelManager.Instance.IsGamePlaying() || CrimeSceneLevelManager.Instance.IsLabPlaying();
        }

        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            isGamePlaying = TutorialLevelManager.Instance.IsGamePlaying();
        }

        if (isGamePaused) {
            canCameraMove = false;
            canPlayerMove = false;
            canPlayerInteract = false;
            canCursorMove = true;
        } else if (isGamePlaying) {
            GameSubStateManager.Instance.CheckGameSubState();
            return;
        } else {
            canCameraMove = false;
            canPlayerMove = false;
            canPlayerInteract =false;
            canCursorMove = true;
        }

        UpdatePlayerActionSettings();
    }

    //State change
    private void TutorialLevelManager_OnStateChange(object sender, System.EventArgs e) {
        CheckGameState();
    }

    private void PauseManager_OnGameUnpause(object sender, System.EventArgs e) {
        CheckGameState();
    }

    private void PauseManager_OnGamePause(object sender, System.EventArgs e) {
        CheckGameState();
    }

    private void CrimeSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        CheckGameState();
    }
}
