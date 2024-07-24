using UnityEngine;

/**
 * Contains all audio clips to be played
 */

[CreateAssetMenu()]
public class SoundSO : ScriptableObject {

    [Header("Main Game")]
    public AudioClip[] footsteps;
    public AudioClip countdownTimerTick;
    public AudioClip countdonwTimerEnd;


    [Header("Tutorial")]
    public AudioClip nextArrowTouch;

    [Header("UI")]
    public AudioClip[] buttonClick;
    public AudioClip[] paperFlipButtonClick;
    public AudioClip[] difficultyButtonClick;
    public AudioClip[] informationHover;


    [Header("Inventory")]
    public AudioClip[] inventorySlotHover;
    public AudioClip inventorySwapSuccessWithItem;
    public AudioClip inventorySwapSuccessWithEmptySpace;
    public AudioClip inventoryOpen;
    public AudioClip inventoryClose;
    public AudioClip inventoryDropItem;

    [Header("Pause Menu")]
    public AudioClip pauseGame;
    public AudioClip resumeGame;

    [Header("Evidence and Equipment")]
    public AudioClip sealEvidence;

    public AudioClip cameraTakePicture;
    public AudioClip cameraRefocus;
    public AudioClip[] photoGalleryShufflePics;
}
