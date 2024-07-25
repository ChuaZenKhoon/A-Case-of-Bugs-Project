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
        InventoryManager.Instance.OnEquipItem += InventoryManager_OnEquipItem;
    }

    private void InventoryManager_OnEquipItem(object sender, System.EventArgs e) {
        UISFXPlayer.Instance.PlayEquipItemSound(volMultiplier * 3f);
    }

    private void InventoryDropItemUI_OnConfirmedDropItem(object sender, int e) {
        UISFXPlayer.Instance.PlayInventoryDropItemSound(volMultiplier * 1.5f);
    }

    private void InventoryManager_OnInventoryOpenStateChange(object sender, bool open) {
        if (open) {
            UISFXPlayer.Instance.PlayInventoryOpenSound(volMultiplier);
        } else {
            UISFXPlayer.Instance.PlayInventoryCloseSound(volMultiplier);
        }
    }

    private void InventoryManager_OnSuccessfulSwapWithEmptySpace(object sender, System.EventArgs e) {
        UISFXPlayer.Instance.PlayInventorySwapWithEmptySpaceSound(volMultiplier);
    }

    private void InventoryManager_OnSuccessfulSwapWithItem(object sender, System.EventArgs e) {
        UISFXPlayer.Instance.PlayInventorySwapWithItemSound(volMultiplier);
    }
    
}
