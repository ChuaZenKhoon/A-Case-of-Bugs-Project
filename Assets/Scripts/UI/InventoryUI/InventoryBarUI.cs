using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the inventory bar.
 */
public class InventoryBarUI : MonoBehaviour {

    [SerializeField] private Image[] inventoryBarImages;

    private const int INVENTORY_BAR_SLOTS = 5;

    private static Color INVISIBLE_SPRITE_IMAGE = new Color(1, 1, 1, 0);
    private static Color VISIBLE_SPRITE_IMAGE = new Color(1, 1, 1, 1);

    //Updates the inventory bar slot if affected by the swap
    public void SwapInventoryBarVisual(int oldIndex, int newIndex, Sprite oldSprite, Sprite newSprite) {
        if (newIndex < INVENTORY_BAR_SLOTS) {
            UpdateSprite(newIndex, newSprite);
        }

        if (oldIndex < INVENTORY_BAR_SLOTS) {
            UpdateSprite(oldIndex, oldSprite);
        }
    }

    /**
     * Updates the inventory bar slot sprite.
     * 
     * @param index The index of the inventory bar slot to be updated.
     * @param inventoryObject The inventoryObject used for updating.
     */
    private void UpdateSprite(int index, Sprite sprite) {
        if (sprite == null) {
            inventoryBarImages[index].sprite = null;
            inventoryBarImages[index].color = INVISIBLE_SPRITE_IMAGE;  
        } else {
            inventoryBarImages[index].sprite = sprite;
            inventoryBarImages[index].color = VISIBLE_SPRITE_IMAGE;
        }
    }

    //Updates the inventory bar visuals on startup
    public void UpdateVisual(Sprite[] inventorySpritesArray) {
        for (int i = 0; i < INVENTORY_BAR_SLOTS; i++) {
            UpdateSprite(i, inventorySpritesArray[i]);
        }
    }

    /**
     * Updates the inventory bar slot visual when a new item is added to the inventory.
     * The item is in one of the inventory bar slots (first 5)
     * 
     * @param index The index of the inventory slot to update
     * @param sprite The Sprite that represents the inventory object
     */
    public void AddToInventoryBarVisual(int index, Sprite sprite) {
        UpdateSprite(index, sprite);
    }

    /**
     * Updates the inventory bar slot visual when an item is removed from the inventory.
     * The item is in one of the inventory bar slots (first 5)
     * 
     * @param index The index of the inventory slot to update
     */
    public void RemoveFromInventoryBarVisual(int index) {
        UpdateSprite(index, null);
    }
}
