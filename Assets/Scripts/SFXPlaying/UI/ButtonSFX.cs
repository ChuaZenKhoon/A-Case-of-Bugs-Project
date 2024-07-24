using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : SFX {

    [SerializeField] protected Button button;

    protected virtual void Awake() {
        volMultiplier = 1f;
        button.onClick.AddListener(() => {
            PlaySpecificSound();
        });
    }

    protected virtual void PlaySpecificSound() {
        SFXPlayer.Instance.PlayButtonSound(volMultiplier);
    }
}
