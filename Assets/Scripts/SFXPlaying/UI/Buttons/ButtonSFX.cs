using UnityEngine;
using UnityEngine.UI;

/**
 * A component of UI buttons that handles its sound effects.
 */
public class ButtonSFX : SFX {

    [SerializeField] protected Button button;

    protected virtual void Awake() {
        volMultiplier = 1f;
        button.onClick.AddListener(() => {
            PlaySpecificSound();
        });
    }

    protected virtual void PlaySpecificSound() {
        UISFXPlayer.Instance.PlayButtonSound(volMultiplier);
    }
}
