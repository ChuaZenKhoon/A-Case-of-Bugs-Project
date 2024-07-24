/**
 * A component of the Inventory that handles its sound effects.
 */
public class InventorySFX : SFX {

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        InventoryManager.Instance.OnSuccessfulSwapWithItem += InventoryManager_OnSuccessfulSwapWithItem;
        InventoryManager.Instance.OnSuccessfulSwapWithEmptySpace += InventoryManager_OnSuccessfulSwapWithEmptySpace;
        InventoryManager.Instance.OnInventoryOpenStateChange += InventoryManager_OnInventoryOpenStateChange;
        InventoryDropItemUI.OnConfirmedDropItem += InventoryDropItemUI_OnConfirmedDropItem;
    }

    private void InventoryDropItemUI_OnConfirmedDropItem(object sender, int e) {
        SFXPlayer.Instance.PlayInventoryDropItemSound(volMultiplier * 1.5f);
    }

    private void InventoryManager_OnInventoryOpenStateChange(object sender, bool open) {
        if (open) {
            SFXPlayer.Instance.PlayInventoryOpenSound(volMultiplier);
        } else {
            SFXPlayer.Instance.PlayInventoryCloseSound(volMultiplier);
        }
    }

    private void InventoryManager_OnSuccessfulSwapWithEmptySpace(object sender, System.EventArgs e) {
        SFXPlayer.Instance.PlayInventorySwapWithEmptySpaceSound(volMultiplier);
    }

    private void InventoryManager_OnSuccessfulSwapWithItem(object sender, System.EventArgs e) {
        SFXPlayer.Instance.PlayInventorySwapWithItemSound(volMultiplier);
    }
    
}
