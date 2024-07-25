using UnityEngine;

/**
 * A component of the difficulty UI buttons that handles its sound effects.
 */
public class DifficultyButtonSFX : ButtonSFX {

    [SerializeField] private int difficultyNum;

    protected override void Awake() {
        volMultiplier = 0.5f;
        button.onClick.AddListener(() => {
            PlaySpecificSound();
        });
    }
    protected override void PlaySpecificSound() {
        UISFXPlayer.Instance.PlayDifficultyButtonSound(difficultyNum, volMultiplier);
    }


}
