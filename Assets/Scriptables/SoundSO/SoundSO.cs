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
    public AudioClip[] buttonClicks;
    public AudioClip[] paperFlipButtonClicks;
    public AudioClip[] difficultyButtonClicks;
    public AudioClip[] informationHovers;


    [Header("Inventory")]
    public AudioClip[] inventorySlotHovers;
    public AudioClip inventorySwapSuccessWithItem;
    public AudioClip inventorySwapSuccessWithEmptySpace;
    public AudioClip[] inventoryEquipItem;
    public AudioClip inventoryOpen;
    public AudioClip inventoryClose;
    public AudioClip inventoryDropItem;

    [Header("Pause Menu")]
    public AudioClip pauseGame;
    public AudioClip resumeGame;

    [Header("Evidence")]
    public AudioClip sealEvidence;

    [Header("Equipment")]
    [Space]
    public AudioClip cameraTakePicture;
    public AudioClip cameraRefocus;
    public AudioClip[] GalleryShufflePics;

    [Space]
    public AudioClip[] sketchPlanFlippingPapers;
    public AudioClip sketchPlanToggleSketchView;
    public AudioClip sketchPlanClearSketch;
    public AudioClip sketchPlanUndoLine;
    public AudioClip[] sketchPlanDrawing;

    [Space]
    public AudioClip placardPutDown;
    public AudioClip placardPickUp;

    [Space]
    public AudioClip fingerprintDusting;

    [Space]
    public AudioClip measuringToolStart;
    public AudioClip measuringToolStop;

    [Space]
    public AudioClip phoneOpenClose;
    public AudioClip[] phoneButtonTaps;

    [Space]
    public AudioClip fingerprintLifterUse;

    [Space]
    public AudioClip swabUse;

    [Space]
    public AudioClip TweezerPickup;

    [Space]
    public AudioClip[] flyNetWaving;

    [Space]
    public AudioClip hotWaterCupPourWater;

    [Space]
    public AudioClip[] dripLiquid;
    public AudioClip wrongTest;
    public AudioClip correctTest;

}
