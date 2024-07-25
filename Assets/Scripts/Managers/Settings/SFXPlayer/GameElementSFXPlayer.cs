using UnityEngine;

/**
 * A Manager subclass in charge of playing audio clips for game elements.
 */
public class GameElementSFXPlayer : SFXPlayer {

    public static GameElementSFXPlayer Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }
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


    //Evidence and Equipment

    public void PlaySealEvidenceSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.sealEvidence, position, volumeMultiplier);
    }

    //Photography Camera
    public void PlayTakePictureSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.cameraTakePicture, position, volumeMultiplier);
    }

    public void PlayCameraRefocusSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.cameraRefocus, position, volumeMultiplier);
    }

    public void PlayCameraGalleryShufflePicsSound(Vector3 position, float volumeMultiplier) {
        PlaySound(soundSO.GalleryShufflePics, position, volumeMultiplier);
    }

    //Sketch Plan
    public void PlaySketchPlanSelectSectionSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.sketchPlanFlippingPapers, volumeMultiplier);
    }

    public void PlaySketchClearSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.sketchPlanClearSketch, volumeMultiplier);
    }

    public void PlaySketchPlanUndoLineSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.sketchPlanUndoLine, volumeMultiplier);
    }
    public void PlaySketchPlanToggleViewSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.sketchPlanToggleSketchView, volumeMultiplier);
    }

    public void PlaySketchDrawingSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.sketchPlanDrawing, volumeMultiplier);
    }
    
    //Placard holder
    public void PlayPlacardPickUpSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.placardPickUp, position, volumeMultiplier);
    }
    public void PlayPlacardPutDownSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.placardPutDown, position, volumeMultiplier);
    }

    //Fingerprint Duster
    public void PlayFingerprintDustingSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.fingerprintDusting, volumeMultiplier);
    }

    //Measuring Tool
    public void PlayMeasuringToolStartSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.measuringToolStart, volumeMultiplier);
    }
    public void PlayMeasuringToolStopSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.measuringToolStop, volumeMultiplier);
    }

    //Phone
    public void PlayPhoneOpenCloseSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.phoneOpenClose, position, volumeMultiplier);
    }

    public void PlayPhoneTapSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.phoneButtonTaps, position, volumeMultiplier);
    }

    //FingerprintLifter
    public void PlayFingerprintLifterUseSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.fingerprintLifterUse, position, volumeMultiplier);
    }

    //Swab
    public void PlaySwabUseSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.swabUse, position, volumeMultiplier);
    }

    //Container
    public void PlayTweezerUseSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.TweezerPickup, position, volumeMultiplier);
    }

    //Insect Net
    public void PlayFlyNetUseSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.flyNetWaving, position, volumeMultiplier);
    }

    //HotWaterCup
    public void PlayPourWaterSound(Vector3 position, float volumeMultiplier = 1f) {
        PlaySound(soundSO.hotWaterCupPourWater, position, volumeMultiplier);
    }
    
    //BloodTestStation
    public void PlayDripLiquidSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.dripLiquid, volumeMultiplier);
    }

    public void PlayCorrectTestSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.correctTest, volumeMultiplier);
    }
    public void PlayWrongTestSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.wrongTest, volumeMultiplier);
    }
    //Microscope
    public void PlayNextPictureSound(float volumeMultiplier = 1f) {
        PlaySoundOneShot(soundSO.GalleryShufflePics, volumeMultiplier);
    }
}
