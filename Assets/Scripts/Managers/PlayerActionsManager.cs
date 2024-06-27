using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A class that handles Player movement, Camera movement and cursor lock 
 * based on current game state.
 */
public class PlayerActionsManager : MonoBehaviour {

    public static PlayerActionsManager Instance { get; private set; }

    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Texture2D cursorCrossHairSprite;

    private bool canCameraMove;
    private bool canPlayerMove;
    private bool canCursorMove;
    private void Awake() {
        Instance = this;
        canCameraMove = false;
        canPlayerMove = false;
        canCursorMove = true;
    }

    private void Start() {
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
        }

        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
            TutorialItemsManager.Instance.OnEquipmentGuideUIOpenClose += TutorialItemsManager_OnEquipmentGuideUIOpenClose;
        }

        PauseManager.Instance.OnGamePause += PauseManager_OnGamePause;
        PauseManager.Instance.OnGameUnpause += PauseManager_OnGameUnpause;

        InventoryManager.Instance.OnInventoryUIOpenStateChange += InventoryManager_OnInventoryUIOpenStateChange;
        PhotographyCameraUI.OnOpenPhotoGallery += PhotographyCameraUI_OnOpenPhotoGallery;
        SketchPlanUI.OnOpenSketchPlan += SketchPlanUI_OnOpenSketchPlan;
        Phone.OnPhoneOpen += Phone_OnPhoneOpen;
    }



    private void UpdatePlayerActions() {

        //Most impt

        bool isGamePaused = PauseManager.Instance.IsGamePaused();
        bool isGamePlaying = false;
        
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            isGamePlaying = CrimeSceneLevelManager.Instance.IsGamePlaying();
        }

        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            isGamePlaying = TutorialLevelManager.Instance.IsGamePlaying();
        }

        if (isGamePaused) {
            canCameraMove = false;
            canPlayerMove = false;
            canCursorMove = true;
        
        } else if (isGamePlaying) {
            
            bool isInventoryInUse = InventoryManager.Instance.IsInventoryOpen();
            bool isEquipmentInUse = Equipment.isInAction;
            bool isTutorial = FindAnyObjectByType<TutorialItemsManager>();
            bool isEquipmentGuideScreenShowing = false;
            if (isTutorial) {
                isEquipmentGuideScreenShowing = TutorialItemsManager.Instance.IsEquipmentGuideOpen();
            }

            if (isInventoryInUse || isEquipmentInUse || isEquipmentGuideScreenShowing) {
                canCameraMove = false;
                canPlayerMove = false;
                canCursorMove = true;
            } else {
                canCameraMove = true;
                canPlayerMove = true;
                canCursorMove = false;
            }    
        } else {
            canCameraMove = false;
            canPlayerMove = false;
            canCursorMove = true;
        }

        UpdatePlayerActionSettings();
    }

    private void UpdatePlayerActionSettings() {
        if (canCursorMove) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset to default cursor
        } else {
            Vector2 cursorHotspot = new Vector2(cursorCrossHairSprite.width / 2f, cursorCrossHairSprite.height / 2f);
            Cursor.SetCursor(cursorCrossHairSprite, cursorHotspot, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        if (canCameraMove) {
            playerCamera.ToggleActivationState(true);
        } else {
            playerCamera.ToggleActivationState(false);
        }

        if (canPlayerMove) {
            Player.Instance.ToggleActivationState(true);
        } else {
            Player.Instance.ToggleActivationState(false);
        }
    }

    private void TutorialItemsManager_OnEquipmentGuideUIOpenClose(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }
    private void Phone_OnPhoneOpen(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }
    private void TutorialLevelManager_OnStateChange(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }
    private void SketchPlanUI_OnOpenSketchPlan(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }

    private void PhotographyCameraUI_OnOpenPhotoGallery(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }

    /**
     * Updates camera movement when game is unpaused. 
     * Time is unfrozen when game is unpaused, but handled by LevelManager.
     */
    private void PauseManager_OnGameUnpause(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }

    /**
     * Updates camera movement when game is paused. 
     * Time is frozen when game is paused, but handled by LevelManager.
     */
    private void PauseManager_OnGamePause(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }

    /**
     * Updates camera movement when game state changes.
     * Prevents camera movement before or after game.
     */
    private void CrimeSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }

    /**
     * Updates camera movement when inventory is opened, 
     * preventing players from cheating and looking around, and time is not frozen.
     */
    private void InventoryManager_OnInventoryUIOpenStateChange(object sender, System.EventArgs e) {
        UpdatePlayerActions();
    }
}
