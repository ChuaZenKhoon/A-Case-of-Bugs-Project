using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * A manager in charge of handling the logic of game settings.
 */
public class GameSettingsManager : SettingsManager {

    public static GameSettingsManager Instance { get; private set; }

    public event EventHandler<float> OnMouseSensitivityChange;

    private const string PLAYER_PREFS_MOUSE_SENSITIVITY = "Mouse Sensitivity";
    private const float MAX_SENSITIVITY = 4.9f;

    private float mouseSensitivityValue;

    //Persistent Singleton
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }

        mouseSensitivityValue = PlayerPrefs.GetFloat(PLAYER_PREFS_MOUSE_SENSITIVITY, 0.9f/MAX_SENSITIVITY);
        SetSensitivity(mouseSensitivityValue);
    }

    //Find Options Menu UI when scene loads
    private void Start() {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SetUpOptionsMenuUI();
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        SetUpOptionsMenuUI();
    }

    protected override void SubscribeToEvents(OptionsMenuUI optionsMenuUI) {
        optionsMenuUI.OnMouseSensitivityChange += OptionsMenuUI_OnMouseSensitivityChange1;
    }

    private void OptionsMenuUI_OnMouseSensitivityChange1(object sender, float e) {
        SetSensitivity(e);
    }

    private void SetSensitivity(float sensitivity) {
        mouseSensitivityValue = sensitivity;
        float scaledMouseSensitivity = (sensitivity * MAX_SENSITIVITY) + 0.1f;
        OnMouseSensitivityChange?.Invoke(this, scaledMouseSensitivity);
        PlayerPrefs.SetFloat(PLAYER_PREFS_MOUSE_SENSITIVITY, mouseSensitivityValue);
        PlayerPrefs.Save();
    }

    public float GetSensitivity() {
        return mouseSensitivityValue;
    }

    public float GetScaledSensitivity() {
        return (mouseSensitivityValue * MAX_SENSITIVITY) + 0.1f; ;
    }
}
