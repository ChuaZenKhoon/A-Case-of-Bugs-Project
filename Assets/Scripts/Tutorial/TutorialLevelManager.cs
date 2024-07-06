using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : MonoBehaviour {

    public static TutorialLevelManager Instance { get; private set; }

    public event EventHandler OnStateChange;

    [SerializeField] private IntroMessageUI introMessageUI;
    [SerializeField] private InstructionsUI instructionUI;
    [SerializeField] private ExitTutorialUI exitTutorialUI;
    [SerializeField] private List<NextLocationArrow> nextLocationArrows;

    private State state;

    private bool hasHitNextArrow;

    public enum State {
        IntroMessage,
        Movement,
        Inventory,
        Interaction,
        Equipment_Camera,
        Equipment_Clipboard,
        Equipment_PlacardHolder,
        Equipment_MeasuringTool,
        Equipment_Phone,
        Equipment_FingerprintDuster_FingerprintLifter,
        Equipment_Swab,
        Equipment_Container,
        Equipment_FlyNet_KillJar,
        Equipment_HotWaterCup_EthanolContainer,
        Exit
    }

    private void Awake() {
        Instance = this;
        state = State.IntroMessage;
    }

    private void Start() {
        introMessageUI.OnFinishReading += IntroMessageUI_OnFinishReading;
        NextLocationArrow.OnTouchArrow += NextLocationArrow_OnTouchArrow;
    }

    private void NextLocationArrow_OnTouchArrow(object sender, int e) {
        if (e < nextLocationArrows.Count - 1) {
            nextLocationArrows[e + 1].gameObject.SetActive(true);
        }
        
        state++;

        instructionUI.DisplayMessage(state);
        
        if (state == TutorialLevelManager.State.Exit) {
            exitTutorialUI.Show();
        }

        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void IntroMessageUI_OnFinishReading(object sender, EventArgs e) {
        state = State.Movement;
        instructionUI.DisplayMessage(state);
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    public bool IsStartingMovement() {
        return state == State.Movement;
    }

    public bool IsGamePlaying() {
        return state != State.IntroMessage && state != State.Exit;
    }

    public bool IsStartingInventory() {
        return state == State.Inventory;
    }

    public bool IsStartingInteraction() {
        return state == State.Interaction;
    }

    public State GetState() {
        return state;
    }
}
