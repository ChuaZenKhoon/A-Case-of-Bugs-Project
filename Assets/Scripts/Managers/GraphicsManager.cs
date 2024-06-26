using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A manager in charge of handling the logic of graphic settings of the game.
 */
public class GraphicsManager : MonoBehaviour {

    public static GraphicsManager Instance { get; private set; }

    //Player Preferences keys for storing information
    private const string PLAYER_PREFS_GRAPHICSQUALITY_VALUE = "GraphicsQuality";
    private const string PLAYER_PREFS_FULLSCREEN_VALUE = "Fullscreen";
    private const string PLAYER_PREFS_VSYNC_VALUE = "Vsync";
    
    //Graphic settings to take from saved settings, temp settings not confirmed set
    private int graphicsQualityValue;
    private bool isFullscreenOn;
    private bool isVsyncOn;

    private int savedGraphicsQualityValue;
    private bool savedIsFullscreenOn;
    private bool savedIsVsyncOn;

    //Custom List for graphic quality
    public static List<string> fixedQuality = new List<string>
    {
        "Low",
        "Medium",
        "High"
    };

    private void Awake() {
        Instance = this;

        //Get Player Prefs else default value
        //Resolutions will be based on what the player's device can use
        savedGraphicsQualityValue = PlayerPrefs.GetInt(PLAYER_PREFS_GRAPHICSQUALITY_VALUE, 1);
        savedIsFullscreenOn = PlayerPrefs.GetInt(PLAYER_PREFS_FULLSCREEN_VALUE, 0) == 1;
        savedIsVsyncOn = PlayerPrefs.GetInt(PLAYER_PREFS_VSYNC_VALUE, 0) == 1;

        SetGraphicsQuality(savedGraphicsQualityValue);
        SetFullscreen(savedIsFullscreenOn);
        SetVsync(savedIsVsyncOn);

        ApplyGraphicSettings();
    }


    //Subscribe to graphic settings change events from the options menu UI
    private void Start() {
        if (OptionsMenuUI.Instance != null) {
            OptionsMenuUI.Instance.OnGraphicsQualityChange += OptionsMenuUI_OnGraphicsQualityChange;
            OptionsMenuUI.Instance.OnFullscreenChange += OptionsMenuUI_OnFullscreenChange;
            OptionsMenuUI.Instance.OnVSyncChange += OptionsMenuUI_OnVsyncChange;
        }
    }

    private void OptionsMenuUI_OnVsyncChange(object sender, bool e) {
        SetVsync(e);
    }

    private void OptionsMenuUI_OnFullscreenChange(object sender, bool e) {
        SetFullscreen(e);
    }

    private void OptionsMenuUI_OnGraphicsQualityChange(object sender, int e) {
        SetGraphicsQuality(e);
    }

    private void SetGraphicsQuality(int graphicsQualityValue) {
        this.graphicsQualityValue = graphicsQualityValue;
    }

    private void SetVsync(bool isOn) {
        this.isVsyncOn = isOn;
    }

    private void SetFullscreen(bool isOn) {
        this.isFullscreenOn = isOn;
    }

    public int GetGraphicsQuality() {
        return savedGraphicsQualityValue;
    }

    public bool IsFullscreen() {
        return savedIsFullscreenOn;
    }

    public bool IsVsyncOn() {
        return savedIsVsyncOn;
    }
    

    /**
     * Applies the graphic settings and saves them
     * Any graphic setting changes done but not saved will not be recorded
     * and their effects not updated.
     */
    public void ApplyGraphicSettings() {
        savedGraphicsQualityValue = graphicsQualityValue;
        savedIsFullscreenOn = isFullscreenOn;
        savedIsVsyncOn = isVsyncOn;

        QualitySettings.SetQualityLevel(savedGraphicsQualityValue);

        if (isVsyncOn) {
            QualitySettings.vSyncCount = 1;
        } else {
            QualitySettings.vSyncCount = 0;
        }

        Screen.fullScreen = savedIsFullscreenOn;

        PlayerPrefs.SetInt(PLAYER_PREFS_GRAPHICSQUALITY_VALUE, this.savedGraphicsQualityValue);

        int fullscreenValue = 0;
        int vysncValue = 0; 
        
        if (savedIsFullscreenOn) {
            fullscreenValue = 1;
        }

        if (savedIsVsyncOn) {
            vysncValue = 1;
        }

        PlayerPrefs.SetInt(PLAYER_PREFS_FULLSCREEN_VALUE, fullscreenValue);
        PlayerPrefs.SetInt(PLAYER_PREFS_VSYNC_VALUE, vysncValue);
        
        PlayerPrefs.Save();
    }

    
}
