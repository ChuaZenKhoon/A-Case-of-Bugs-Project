using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the options menu
 */
public class OptionsMenuUI : MonoBehaviour {

    //Events for when settings are changed by player
    public event EventHandler<float> OnMasterVolumeChange;
    public event EventHandler<float> OnSFXVolumeChange;
    public event EventHandler<float> OnBGMVolumeChange;

    public event EventHandler<int> OnGraphicsQualityChange;
    public event EventHandler<bool> OnFullscreenChange;
    public event EventHandler<bool> OnVSyncChange;

    public event EventHandler<float> OnMouseSensitivityChange;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button closeButton;

    //Tabs
    [SerializeField] private Button soundsTab;
    [SerializeField] private Button graphicsTab;
    [SerializeField] private Button gameSettingsTab;
    [SerializeField] private GameObject[] settingTabs;

    //Sounds
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider bgmVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    //Graphics
    [SerializeField] private Button applyGraphicsButton;
    
    [SerializeField] private TMP_Dropdown graphicsQualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Toggle vSyncToggle;

    //Game Settings
    [SerializeField] private Slider mouseSensitivitySlider;

    //Add listeners to buttons and sliders
    private void Awake() {

        masterVolSlider.onValueChanged.AddListener((float volume) => {
            OnMasterVolumeChange?.Invoke(this, volume);
        });
        sfxVolSlider.onValueChanged.AddListener((float volume) => {
            OnSFXVolumeChange?.Invoke(this, volume);
        });
        bgmVolSlider.onValueChanged.AddListener((float volume) => {
            OnBGMVolumeChange?.Invoke(this, volume);
        });

        closeButton.onClick.AddListener(() => {
            Hide();
        });

        fullScreenToggle.onValueChanged.AddListener((bool isOn) => {
            OnFullscreenChange?.Invoke(this, isOn);
        });

        vSyncToggle.onValueChanged.AddListener((bool isOn) => {
            OnVSyncChange?.Invoke(this, isOn);
        });


        applyGraphicsButton.onClick.AddListener(() => {
            GraphicsManager.Instance.ApplyGraphicSettings();
        });

        mouseSensitivitySlider.onValueChanged.AddListener((float sensitivity) => {
            OnMouseSensitivityChange?.Invoke(this, sensitivity);
        });

        soundsTab.onClick.AddListener(() => {
            SwitchTab(0);
        });

        graphicsTab.onClick.AddListener(() => {
            SwitchTab(1);
        });

        gameSettingsTab.onClick.AddListener(() => {
            SwitchTab(2);
        });
    }

    private void Start() {
        //Handle Player preferences if any

        //Sounds set up
        masterVolSlider.value = SoundManager.Instance.GetMasterVolume();
        sfxVolSlider.value = SoundManager.Instance.GetSFXVolume();
        bgmVolSlider.value = SoundManager.Instance.GetBGMVolume();

        //Graphics set up
        InitialiseGraphicsQualityDropDown();
        fullScreenToggle.isOn = GraphicsManager.Instance.IsFullscreen();
        vSyncToggle.isOn = GraphicsManager.Instance.IsVsyncOn();
        
        //In game, QOL effect of closing options menu and pause screen together
        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager != null) {
            PauseManager levelManagerComponent = levelManager.GetComponent<PauseManager>();
            if (levelManagerComponent != null) {
                PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;
            }
        }

        //Game Settings Set up
        mouseSensitivitySlider.value = GameSettingsManager.Instance.GetSensitivity();

        Hide();
    }

    private void PauseManager_OnGameUnpause(object sender, EventArgs e) {
        if(gameObject.activeSelf) {
            Hide();
        }
    }

    public void Show() {
        //Show UI not enable, due to needing to find it by settingManagers
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        SwitchTab(0);
    }
    private void Hide() {
        //Hide UI not disable, due to needing to find it by settingManagers
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void InitialiseGraphicsQualityDropDown() {
        graphicsQualityDropDown.ClearOptions();
        List<string> graphicsQualityOptions = GraphicsManager.fixedQuality;
        graphicsQualityDropDown.AddOptions(graphicsQualityOptions);
        graphicsQualityDropDown.value = GraphicsManager.Instance.GetGraphicsQuality();
        graphicsQualityDropDown.RefreshShownValue();
        graphicsQualityDropDown.onValueChanged.AddListener((int value) => {
            OnGraphicsQualityChange?.Invoke(this, value);
        });
    }

    /**
     * Enables player to switch between different menu tabs of settings.
     * 
     * @param index The index of the tab 
     */
    private void SwitchTab(int index) {
        for (int i = 0; i< settingTabs.Length; i++) {
            if (i == index) {
                settingTabs[i].SetActive(true);
            } else {
                settingTabs[i].SetActive(false);
            }
        }
    }

}
