using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/**
 * A manager in charge of handling the logic of audio settings of the game.
 */
public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    //Player Preferences keys for storing information
    private const string PLAYER_PREFS_MASTER_VOL = "Master";
    private const string PLAYER_PREFS_SFX_VOL = "SFX";
    private const string PLAYER_PREFS_BGM_VOL = "BGM";

    [SerializeField] private AudioMixer audioMixer;

    //SoundSO stores all audioclips, just reference it to get audioclip needed
    [SerializeField] private SoundSO soundSO;

    private float masterVolume;
    private float sfxVolume;
    private float bgmVolume;

    private void Awake() {
        Instance = this;

        //Get Player Prefs else default value
        masterVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOL, 0.5f);
        sfxVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOL, 0.5f);
        bgmVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_BGM_VOL, 0.5f);

        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetBGMVolume(bgmVolume);
    }

    //Subscribe to audio volume change events from the options menu UI
    private void Start() {
        if (OptionsMenuUI.Instance != null) {
            OptionsMenuUI.Instance.OnMasterVolumeChange += OptionsMenuUI_OnMasterVolumeChange;
            OptionsMenuUI.Instance.OnSFXVolumeChange += OptionsMenuUI_OnSFXVolumeChange;
            OptionsMenuUI.Instance.OnBGMVolumeChange += OptionsMenuUI_OnBGMVolumeChange;
        }
        
    }

    private void Instance_OnEventCallTest(object sender, System.EventArgs e) {
        Debug.Log("Hi");
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

    //Following functions handle audioclip playing
    private void PlaySound(AudioClip[] audioClipArray, Vector3 pos, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], pos, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 pos, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, pos, volumeMultiplier * sfxVolume);
    }

    public void PlayFootstepSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.footsteps, position, volumeMultiplier);
    }


}