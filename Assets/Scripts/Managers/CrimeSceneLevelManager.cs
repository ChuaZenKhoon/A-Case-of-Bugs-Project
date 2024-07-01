using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * A manager in charge of handling the gameplay logic and game flow.
 */
public class CrimeSceneLevelManager : MonoBehaviour {

    public static CrimeSceneLevelManager Instance { get; private set; }

    //Event for when game state changes
    public event EventHandler OnStateChange;

    [SerializeField] private List<DifficultyEvidenceList> difficultyEvidenceList;

    [Serializable]
    public struct DifficultyEvidenceList {
        public DifficultySO difficultySO;
        public List<GameObject> evidenceToPlace;
    }

    //Designate game states for easy tracking
    public enum State {
        ReadingReport,
        GameStartCountdown,
        GamePlaying,
        GameEnd,
        LabMessage,
        LabPlaying,
        LabEnd
    }

    [SerializeField] private Transform labSpawnLocation;
    [SerializeField] private ExitDoor exitDoor;

    private State state;

    private float gameStartCountdownTime = 5f;
    private float gameTimerMin;
    private float gameTimerSeconds;
    private float gameTime = 900f; //15 minutes

    private void Awake() {
        Instance = this;
        state = State.ReadingReport;

        foreach (DifficultyEvidenceList evidenceList in difficultyEvidenceList) {
            if (evidenceList.difficultySO == DifficultySettingManager.difficultyLevelSelected) {
                foreach (GameObject evidence in evidenceList.evidenceToPlace) {
                    evidence.SetActive(true);
                }
            }
        }
    }

    //Subscribe to events that change game flow and state
    private void Start() {
        InformationReportUI.Instance.OnClickReadFinishReport += InformationReportUI_OnClickReadFinishReport;

        LabSceneMessageUI.OnMessageClickFinish += LabSceneMessageUI_OnMessageClickFinish;
        exitDoor.OnExitLab += ExitDoor_OnExitLab;
    }

    private void LabSceneMessageUI_OnMessageClickFinish(object sender, EventArgs e) {
        state = State.LabPlaying;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void ExitDoor_OnExitLab(object sender, EventArgs e) {
        state = State.LabEnd;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void InformationReportUI_OnClickReadFinishReport(object sender, System.EventArgs e) {
        state = State.GameStartCountdown;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        switch (state) {
            case State.ReadingReport:
                break;
            case State.GameStartCountdown:
                HandleGameStartCountdown();
                break;
            case State.GamePlaying:
                HandleGamePlaying();
                break;
            case State.GameEnd:
                break;
            case State.LabMessage:
                break;
            case State.LabPlaying:
                break;
            case State.LabEnd:
                break;
            default:
                break;
        }
    }

    private void HandleGameStartCountdown() {
        gameStartCountdownTime -= Time.deltaTime;

        if (gameStartCountdownTime < 0f) {
            state = State.GamePlaying;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleGamePlaying() {
        gameTime -= Time.deltaTime;
        gameTimerMin = Mathf.FloorToInt(gameTime / 60);
        gameTimerSeconds = Mathf.FloorToInt(gameTime % 60);

        if (gameTime < 0f) {
            state = State.GameEnd;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //For other classes to access game states
    public bool IsGameStartCountingDown() {
        return state == State.GameStartCountdown;
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsGameOver() {
        return state == State.GameEnd;
    }

    public bool IsLabStarted() {
        return state == State.LabMessage;
    }

    public bool IsLabPlaying() {
        return state == State.LabPlaying;
    }

    public bool IsLabEnded() {
        return state == State.LabEnd;
    }


    //For UI timers to get time
    public float GetCountdownTime() {
        return gameStartCountdownTime;
    }

    public float GetGamePlayTimeLeft(out float secondsTime) {
        secondsTime = gameTimerSeconds;
        return gameTimerMin; 
    }

    public void SkipAhead() {
        gameTime = 0f;
    }

    public void MoveToLab() {
        Player.Instance.gameObject.transform.position = labSpawnLocation.position;
        Player.Instance.gameObject.transform.rotation = labSpawnLocation.rotation;
        state = State.LabMessage;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
}
