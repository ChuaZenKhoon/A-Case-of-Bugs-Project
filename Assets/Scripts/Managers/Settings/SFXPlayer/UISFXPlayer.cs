/**
 * A Manager subclass in charge of playing audio clips for UI elements.
 */
public class UISFXPlayer : SFXPlayer {

    public static UISFXPlayer Instance {  get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }
    }

    //UI
    public void PlayButtonSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.buttonClicks, volumeMultiplier);
    }

    public void PlayPaperFlipButtonSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.paperFlipButtonClicks, volumeMultiplier);
    }

    public void PlayDifficultyButtonSound(int difficulty, float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.difficultyButtonClicks[difficulty], volumeMultiplier);
    }

    public void PlayInformationHoverSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.informationHovers, volumeMultiplier);
    }

    //Inventory
    public void PlayInventorySlotHoverSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventorySlotHovers, volumeMultiplier);
    }

    public void PlayInventorySwapWithItemSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventorySwapSuccessWithItem, volumeMultiplier);
    }

    public void PlayInventorySwapWithEmptySpaceSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventorySwapSuccessWithEmptySpace, volumeMultiplier);
    }

    public void PlayEquipItemSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventoryEquipItem, volumeMultiplier);
    }
    public void PlayInventoryOpenSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventoryOpen, volumeMultiplier);
    }
    public void PlayInventoryCloseSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventoryClose, volumeMultiplier);
    }

    public void PlayInventoryDropItemSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventoryDropItem, volumeMultiplier);
    }

    //Pause Menu
    public void PlayPauseGameSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.pauseGame, volumeMultiplier);
    }

    public void PlayResumeGameSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.resumeGame, volumeMultiplier);
    }

    
}
