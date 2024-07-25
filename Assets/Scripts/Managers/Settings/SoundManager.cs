using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/**
 * A manager in charge of handling the logic of audio settings of the game.
 */
public class SoundManager : SettingsManager {

    public static SoundManager Instance { get; private set; }

    //Player Preferences keys for storing information
    private const string PLAYER_PREFS_MASTER_VOL = "Master";
    private const string PLAYER_PREFS_SFX_VOL = "SFX";
    private const string PLAYER_PREFS_BGM_VOL = "BGM";

    [SerializeField] private AudioMixer audioMixer;

    private float masterVolume;
    private float sfxVolume;
    private float bgmVolume;

    //Persistent Singleton
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }

        //Get Player Prefs else default value
        masterVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOL, 0.5f);
        sfxVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOL, 0.5f);
        bgmVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_BGM_VOL, 0.5f);
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetBGMVolume(bgmVolume);
    }

    //Find Options Menu UI when scene loads
    private void Start() {
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        SetUpOptionsMenuUI();
    }

    private void SceneManager_OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        SetUpOptionsMenuUI();
    }

    protected override void SubscribeToEvents(OptionsMenuUI optionsMenuUI) {
        optionsMenuUI.OnMasterVolumeChange += OptionsMenuUI_OnMasterVolumeChange;
        optionsMenuUI.OnSFXVolumeChange += OptionsMenuUI_OnSFXVolumeChange;
        optionsMenuUI.OnBGMVolumeChange += OptionsMenuUI_OnBGMVolumeChange;
    }

    protected override void UnsubscribeFromEvents(OptionsMenuUI optionsMenuUI) {
        optionsMenuUI.OnMasterVolumeChange -= OptionsMenuUI_OnMasterVolumeChange;
        optionsMenuUI.OnSFXVolumeChange -= OptionsMenuUI_OnSFXVolumeChange;
        optionsMenuUI.OnBGMVolumeChange -= OptionsMenuUI_OnBGMVolumeChange;
    }

    private void OptionsMenuUI_OnBGMVolumeChange(object sender, float e) {
        SetBGMVolume(e);
    }

    private void OptionsMenuUI_OnSFXVolumeChange(object sender, float e) {
        SetSFXVolume(e);
    }

    private void OptionsMenuUI_OnMasterVolumeChange(object sender, float e) {
        SetMasterVolume(e);
    }

    /**
     * No apply sound options button to allow user to slide bar and
     * more easily determine comfortable sound volume for themselves
     * Due to no apply sound options button, PlayerPrefs to save after each designation of volume
     */
    
    private void SetMasterVolume(float volume) {

        masterVolume = volume;

        if (masterVolume == 0f) {
            audioMixer.SetFloat("Master", -80f);
        } else {
            audioMixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20);
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOL, masterVolume);
        PlayerPrefs.Save();

    }

    private void SetSFXVolume(float volume) {

        sfxVolume = volume;

        if (sfxVolume == 0f) {
            audioMixer.SetFloat("SFX", -80f);
        } else {
            audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        }

        UISFXPlayer.Instance.SetSFXVolume(sfxVolume);
        GameElementSFXPlayer.Instance.SetSFXVolume(sfxVolume);

        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOL, sfxVolume);
        PlayerPrefs.Save();

    }

    private void SetBGMVolume(float volume) {

        bgmVolume = volume;

        if (bgmVolume == 0f) {
            audioMixer.SetFloat("BGM", -80f);
        } else {
            audioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20);
        }


        PlayerPrefs.SetFloat(PLAYER_PREFS_BGM_VOL, bgmVolume);
        PlayerPrefs.Save();

    }

    public float GetMasterVolume() {
        return masterVolume;
    }

    public float GetSFXVolume() {
        return sfxVolume;
    }
    public float GetBGMVolume() {
        return bgmVolume;
    }

    
}