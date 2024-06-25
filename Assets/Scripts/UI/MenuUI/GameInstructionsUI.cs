using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInstructionsUI : MonoBehaviour {

    public static event EventHandler OnSkipAhead;

    [SerializeField] private Button closeButton;

    [SerializeField] private Button skipAheadButton;
    [SerializeField] private GameObject confirmSkipAheadScreen;
    [SerializeField] private Button cancelSkipAheadButton;
    [SerializeField] private Button confirmSkipAheadButton;

    public static void ResetStaticData() {
        OnSkipAhead = null;
    } 

    private void Awake() {
        closeButton.onClick.AddListener(() => {
            Hide();
        });

        cancelSkipAheadButton.onClick.AddListener(() => {
            confirmSkipAheadScreen.SetActive(false);
        });

        confirmSkipAheadButton.onClick.AddListener(() => {
            CrimeSceneLevelManager.Instance.SkipAhead();
            gameObject.SetActive(false);
            confirmSkipAheadScreen.SetActive(false);
            OnSkipAhead?.Invoke(this, EventArgs.Empty);
            skipAheadButton.interactable = false;
        });

        skipAheadButton.interactable = false;
    }

    private void Start() {
        Hide();
        confirmSkipAheadScreen.SetActive(false);
        CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
    }

    private void CrimeSceneLevelManager_OnStateChange(object sender, EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGamePlaying()) {
            skipAheadButton.onClick.AddListener(() => {
                confirmSkipAheadScreen.SetActive(true);
            });
            skipAheadButton.interactable = true;
        }
            
        
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }
}
