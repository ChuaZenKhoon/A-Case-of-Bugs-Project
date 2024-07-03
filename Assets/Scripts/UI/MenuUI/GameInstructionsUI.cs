using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the game instructions menu in the main game
 */
public class GameInstructionsUI : MonoBehaviour {

    [SerializeField] private Button closeButton;

    [SerializeField] private Button skipAheadButton;
    [SerializeField] private GameObject confirmSkipAheadScreen;
    [SerializeField] private Button cancelSkipAheadButton;
    [SerializeField] private Button confirmSkipAheadButton;

    private void Awake() {
        closeButton.onClick.AddListener(() => {
            Hide();
        });

        cancelSkipAheadButton.onClick.AddListener(() => {
            confirmSkipAheadScreen.SetActive(false);
        });

        confirmSkipAheadButton.onClick.AddListener(() => {
            GameInstructionsManager.Instance.SkipAhead();
            confirmSkipAheadScreen.SetActive(false);
            skipAheadButton.interactable = false;
            Hide();
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
