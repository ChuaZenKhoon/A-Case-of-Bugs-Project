using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeSFX : SFX {

    [SerializeField] private Button NextPicButton;
    [SerializeField] private Button PrevPicButton;

    private void Awake() {
        volMultiplier = 1f;

        NextPicButton.onClick.AddListener(() => {
            PlayShufflePicSound();
        });
        PrevPicButton.onClick.AddListener(() => {
            PlayShufflePicSound();
        });
    }

    private void PlayShufflePicSound() {
        GameElementSFXPlayer.Instance.PlayNextPictureSound(volMultiplier);
    }

}
