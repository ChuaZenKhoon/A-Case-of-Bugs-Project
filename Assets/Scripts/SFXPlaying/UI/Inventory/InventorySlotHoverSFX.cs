using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotHoverSFX :SFX, IPointerEnterHandler {

    private void Awake() {
        volMultiplier = 0.4f;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        SFXPlayer.Instance.PlayInventorySlotHoverSound(volMultiplier);
    }
}
