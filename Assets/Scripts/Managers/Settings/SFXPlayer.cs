using UnityEngine;

/**
 * A Manager in charge of playing audio clips for sound effects.
 */
public class SFXPlayer : MonoBehaviour {

    public static SFXPlayer Instance {  get; private set; }

    [SerializeField] private AudioSource audioSource;

    //SoundSO stores all audioclips, just reference it to get audioclip needed
    [SerializeField] private SoundSO soundSO;

    private float sfxVolume;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        sfxVolume = SoundManager.Instance.GetSFXVolume();
    }

    public void SetSFXVolume(float volume) {
        sfxVolume = volume;
    }

    //Following functions handle audioclip playing
    private void PlaySound(AudioClip[] audioClipArray, Vector3 pos, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], pos, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 pos, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, pos, volumeMultiplier * sfxVolume);
    }

    private void PlaySoundOneShot(AudioClip audioClip, float volumeMultiplier = 1f) {
        audioSource.PlayOneShot(audioClip, volumeMultiplier * sfxVolume);
    }

    private void PlaySoundOneShot(AudioClip[] audioClips, float volumeMultiplier = 1f) {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], volumeMultiplier * sfxVolume);
    }


    //Specific calls from game elements
    public void PlayFootstepSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.footsteps, position, volumeMultiplier);
    }

    public void PlayCountdownTickSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.countdownTimerTick, volumeMultiplier);
    }

    public void PlayCountdownEndSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.countdonwTimerEnd, volumeMultiplier);
    }

    public void PlayNextArrowReachedSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.nextArrowTouch, volumeMultiplier);
    }

    //UI
    public void PlayButtonSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.buttonClick, volumeMultiplier);
    }

    public void PlayPaperFlipButtonSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.paperFlipButtonClick, volumeMultiplier);
    }

    public void PlayDifficultyButtonSound(int difficulty, float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.difficultyButtonClick[difficulty], volumeMultiplier);
    }

    public void PlayInformationHoverSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.informationHover, volumeMultiplier);
    }

    //Inventory
    public void PlayInventorySlotHoverSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventorySlotHover, volumeMultiplier);
    }

    public void PlayInventorySwapWithItemSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventorySwapSuccessWithItem, volumeMultiplier);
    }

    public void PlayInventorySwapWithEmptySpaceSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.inventorySwapSuccessWithEmptySpace, volumeMultiplier);
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

    //Evidence and Equipment

    public void PlaySealEvidenceSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.sealEvidence, position, volumeMultiplier);
    }

    public void PlayTakePictureSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.cameraTakePicture, position, volumeMultiplier);
    }

    public void PlayCameraRefocusSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.cameraRefocus, position, volumeMultiplier);
    }

    public void PlayCameraGalleryShufflePicsSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.photoGalleryShufflePics, position, volumeMultiplier);
    }

}
