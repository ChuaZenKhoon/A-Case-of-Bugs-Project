using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the options menu
 */
public class OptionsMenuUI : MonoBehaviour {

    public static OptionsMenuUI Instance {  get; private set; }

    //Event for when master volume slider is dragged
    public event EventHandler<float> OnMasterVolumeChange;

    //Event for when SFX volume slider is dragged
    public event EventHandler<float> OnSFXVolumeChange;

    //Event for when BGM volume slider is dragged
    public event EventHandler<float> OnBGMVolumeChange;

    //Event for when graphics quality option is changed
    public event EventHandler<int> OnGraphicsQualityChange;

    //Event for when resolution option is changed
    public event EventHandler<int> OnResolutionChange;

    //Event for when Fullscreen toggle is changed
    public event EventHandler<bool> OnFullscreenChange;

    //Event for when VSync toggle is changed
    public event EventHandler<bool> OnVSyncChange;

    //Event for when mouse sensitivity is changed
    public event EventHandler<float> OnMouseSensitivityChange;


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
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Toggle vSyncToggle;

    //Game Settings
    [SerializeField] private Slider mouseSensitivitySlider;

    //Add listeners to buttons and sliders
    private void Awake() {
        Instance = this;

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
        InitaliseResolutionDropDown();
        fullScreenToggle.isOn = GraphicsManager.Instance.IsFullscreen();
        vSyncToggle.isOn = GraphicsManager.Instance.IsVsyncOn();
        

        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager != null) {
            PauseManager levelManagerComponent = levelManager.GetComponent<PauseManager>();
            if (levelManagerComponent != null) {
                //Is in game
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
        gameObject.SetActive(true);
        SwitchTab(0);
    }
    private void Hide() {
        gameObject.SetActive(false);
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

    private void InitaliseResolutionDropDown() {
        resolutionDropDown.ClearOptions();
        List<Resolution> resolutionOptions = GraphicsManager.resolutions;
        List<string> resolutionStrings = resolutionOptions.Select(res => res.width + " x " + res.height).ToList();
        resolutionDropDown.AddOptions(resolutionStrings);
        resolutionDropDown.value = GraphicsManager.Instance.GetResolution();
        resolutionDropDown.RefreshShownValue();
        resolutionDropDown.onValueChanged.AddListener((int value) => {
            OnResolutionChange?.Invoke(this, value);
        });
    }

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
