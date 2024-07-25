using UnityEngine.EventSystems;

/**
 * A component of the inventory slots that handles its sound effects when hovered over.
 */
public class InventorySlotHoverSFX :SFX, IPointerEnterHandler {

    private void Awake() {
        volMultiplier = 0.4f;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        UISFXPlayer.Instance.PlayInventorySlotHoverSound(volMultiplier);
    }
}
