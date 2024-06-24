using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour {

    public static GameSettingsManager Instance { get; private set; }

    public event EventHandler<float> OnMouseSensitivityChange;

    private const string PLAYER_PREFS_MOUSE_SENSITIVITY = "Mouse Sensitivity";

    private float mouseSensitivityValue;

    private void Awake() {
        Instance = this;

        mouseSensitivityValue = PlayerPrefs.GetFloat(PLAYER_PREFS_MOUSE_SENSITIVITY, 0.9f/4.9f);
        SetSensitivity(mouseSensitivityValue);
    }

    private void Start() {
        OptionsMenuUI.Instance.OnMouseSensitivityChange += OptionsMenuUI_OnMouseSensitivityChange;
    }

    private void OptionsMenuUI_OnMouseSensitivityChange(object sender, float e) {
        SetSensitivity(e);
    }

    private void SetSensitivity(float sensitivity) {
        mouseSensitivityValue = sensitivity;
        float scaledMouseSensitivity = (sensitivity * 4.9f) + 0.1f;
        OnMouseSensitivityChange?.Invoke(this, scaledMouseSensitivity);
        PlayerPrefs.SetFloat(PLAYER_PREFS_MOUSE_SENSITIVITY, mouseSensitivityValue);
        PlayerPrefs.Save();
    }

    public float GetSensitivity() {
        return mouseSensitivityValue;
    }

    public float GetScaledSensitivity() {
        return (mouseSensitivityValue * 4.9f) + 0.1f; ;
    }
}
