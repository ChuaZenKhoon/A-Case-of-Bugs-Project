using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the inventory bar.
 */
public class InventoryBarUI : MonoBehaviour {

    [SerializeField] private Image[] inventoryBarImages;

    private const int INVENTORY_BAR_SLOTS = 5;

    //Subscribe to swapping event
    private void Start() {
        InventoryManager.Instance.OnSuccessfulSwapInvolvesInventoryBar += InventoryManager_OnSuccessfulSwapInventory;
        UpdateVisual();
    }

    //Updates the inventory bar slot if affected by the swap
    private void InventoryManager_OnSuccessfulSwapInventory(object sender, InventorySingleUI.OnSuccessfulDragDropItemEventArgs e) {
        InventoryObject[] inventoryObjectsArray = InventoryManager.Instance.GetInventoryObjectArray();

        if (e.newIndex < 5) {
            UpdateSprite(e.newIndex, inventoryObjectsArray[e.newIndex]);
        }

        if (e.oldIndex < 5) {
            UpdateSprite(e.oldIndex, inventoryObjectsArray[e.oldIndex]);
        }
    }

    /**
     * Updates the inventory bar slot sprite.
     * 
     * @param index The index of the inventory bar slot to be updated.
     * @param inventoryObject The inventoryObject used for updating.
     */
    private void UpdateSprite(int index, InventoryObject inventoryObject) {
        if (inventoryObject == null) {
            inventoryBarImages[index].sprite = null;
            inventoryBarImages[index].color = new Color(1, 1, 1, 0);  
        } else {
            inventoryBarImages[index].color = new Color(1, 1, 1, 1);
            inventoryBarImages[index].sprite = inventoryObject.GetInventoryObjectSO().sprite;
        }
    }

    //Updates the inventory bar visuals on startup
    public void UpdateVisual() {
        InventoryObject[] inventoryObjectsArray = InventoryManager.Instance.GetInventoryObjectArray();
        for (int i = 0; i < INVENTORY_BAR_SLOTS; i++) {
            if (inventoryObjectsArray[i] != null) {
                Sprite sprite = inventoryObjectsArray[i].GetInventoryObjectSO().sprite;
                inventoryBarImages[i].sprite = sprite;
            } else {
                inventoryBarImages[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    /**
     * Updates the inventory bar slot visual when a new item is added to the inventory.
     * The item is in one of the inventory bar slots (first 5)
     * 
     * @param index The index of the inventory slot to update
     * @param inventoryObjectSO The blueprint of the inventoryObject to add in
     */
    public void AddToInventoryBarVisual(int index, InventoryObjectSO inventoryObjectSO) {
        Sprite sprite = inventoryObjectSO.sprite;
        inventoryBarImages[index].color = new Color(1, 1, 1, 1);
        inventoryBarImages[index].sprite = sprite;
    }

    /**
     * Updates the inventory bar slot visual when an item is removed from the inventory.
     * The item is in one of the inventory bar slots (first 5)
     * 
     * @param index The index of the inventory slot to update
     */
    public void RemoveFromInventoryBarVisual(int index) {
        inventoryBarImages[index].color = new Color(1, 1, 1, 0);
        inventoryBarImages[index].sprite = null;
    }
}
