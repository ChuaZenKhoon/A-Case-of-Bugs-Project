using UnityEngine;
using UnityEngine.EventSystems;

/**
 * A component of hover UI elements that handle its sound effects.
 */
public class InformationHoverSFX : SFX, IPointerEnterHandler {

    private void Awake() {
        volMultiplier = 0.8f;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        UISFXPlayer.Instance.PlayInformationHoverSound(volMultiplier);
    }
}




