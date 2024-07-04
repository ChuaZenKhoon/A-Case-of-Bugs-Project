
/**
 * A manager that handles the player actions based on the sub state of the game.
 */

public class GameSubStateManager : PlayerActionsManager {

    public static GameSubStateManager Instance { get; private set; }
    private void Awake() {
        Instance = this;
        canCameraMove = false;
        canPlayerMove = false;
        canPlayerInteract = false;
        canCursorMove = true;
    }

    private void Start() {
        SubscribeToGameSubStates();
    }

    private void SubscribeToGameSubStates() {
        
        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            TutorialItemsManager.Instance.OnEquipmentGuideOpenClose += TutorialItemsManager_OnEquipmentGuideOpenClose;
        }

        //Game Inventory SubState
        InventoryManager.Instance.OnInventoryOpenStateChange += InventoryManager_OnInventoryUIOpenStateChange;

        //Game Equipment Use SubState
        PhotographyCamera.OnOpenPhotoGallery += PhotographyCamera_OnOpenPhotoGallery;
        SketchPlan.OnOpenSketchPlan += SketchPlanUI_OnOpenSketchPlan;
        Phone.OnPhoneOpen += Phone_OnPhoneOpen;

        //Game Lab Equipment Use SubState
        Microscope.OnUseMicroscope += Microscope_OnUseMicroscope;
        BloodTestStation.OnUseBloodTestStation += BloodTestStation_OnUseBloodTestStation;
    }

    /**
     * Checks the current game substate and updates the boolean values for setting later.
     */
    public void CheckGameSubState() {
        bool isInventoryInUse = InventoryManager.Instance.IsInventoryOpen();
        bool isEquipmentInUse = Equipment.isInAction;
        bool isLabEquipmentInUse = LabEquipment.isInAction;
        bool isTutorial = FindAnyObjectByType<TutorialItemsManager>();
        bool isEquipmentGuideScreenShowing = false;

        if (isTutorial) {
            isEquipmentGuideScreenShowing = TutorialItemsManager.Instance.IsEquipmentGuideOpen();
        }

        if (isInventoryInUse || isEquipmentInUse || isLabEquipmentInUse || isEquipmentGuideScreenShowing) {
            canCameraMove = false;
            canPlayerMove = false;
            canPlayerInteract = true;
            canCursorMove = true;
        } else {
            canCameraMove = true;
            canPlayerMove = true;
            canPlayerInteract = true;
            canCursorMove = false;
        }

        UpdatePlayerActionSettings();
    }

    //SubState change
    private void InventoryManager_OnInventoryUIOpenStateChange(object sender, System.EventArgs e) {
        CheckGameSubState();
    }
    private void BloodTestStation_OnUseBloodTestStation(object sender, System.EventArgs e) {
        CheckGameSubState();
    }

    private void Microscope_OnUseMicroscope(object sender, System.EventArgs e) {
        CheckGameSubState();
    }

    private void TutorialItemsManager_OnEquipmentGuideOpenClose(object sender, System.EventArgs e) {
        CheckGameSubState();
    }

    private void Phone_OnPhoneOpen(object sender, System.EventArgs e) {
        CheckGameSubState();
    }

    private void SketchPlanUI_OnOpenSketchPlan(object sender, System.EventArgs e) {
        CheckGameSubState();
    }

    private void PhotographyCamera_OnOpenPhotoGallery(object sender, System.EventArgs e) {
        CheckGameSubState();
    }
}
