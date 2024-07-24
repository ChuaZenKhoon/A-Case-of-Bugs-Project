using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButtonSFX : ButtonSFX {

    [SerializeField] private int difficultyNum;

    protected override void Awake() {
        volMultiplier = 0.5f;
        button.onClick.AddListener(() => {
            PlaySpecificSound();
        });
    }
    protected override void PlaySpecificSound() {
        SFXPlayer.Instance.PlayDifficultyButtonSound(difficultyNum, volMultiplier);
    }


}
