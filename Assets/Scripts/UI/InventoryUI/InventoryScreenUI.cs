using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/**
 * A UI element that represents the inventory screen.
 */
public class InventoryScreenUI : MonoBehaviour, IPointerClickHandler {

    public static bool isInAction;
    //Array of individual inventory slots
    [SerializeField] private InventorySingleUI[] inventorySingleUIslots;

    //For Item examination on the left of the inventory screen
    [SerializeField] private TextMeshProUGUI currentHoverInventoryItemName;
    [SerializeField] private TextMeshProUGUI currentHoverInventoryItemDescription;
    [SerializeField] private Image currentHoverInventoryItemImage;

    //For drop item functionality
    [SerializeField] private InventoryDropItemUI dropItemUI;
    [SerializeField] private LayerMask dropItemLayerMask;


    //Subscribe to item hover event and inventory open/close event
    //For UI elements, hide on start
    private void Start() {
        InventoryManager.Instance.OnInventoryUIOpenStateChange += InventoryManager_OnOpenInventoryUI;
        InventorySingleUI.OnHoverEnterInventorySlot += InventorySingleUI_OnHoverEnterInventorySlot;
        InventorySingleUI.OnHoverLeaveInventorySlot += InventorySingleUI_OnHoverLeaveInventorySlot;
        Hide();
        UpdateVisual();
    }


    //Reset item examination on the left
    private void InventorySingleUI_OnHoverLeaveInventorySlot(object sender, int e) {
        currentHoverInventoryItemImage.sprite = null;
        currentHoverInventoryItemName.text = null;
        currentHoverInventoryItemDescription.text = null;
    }

    //Set item examination on the left
    private void InventorySingleUI_OnHoverEnterInventorySlot(object sender, int indexHovered) {
        InventoryObject[] inventoryObjectsArray = InventoryManager.Instance.GetInventoryObjectArray();

        if (inventoryObjectsArray[indexHovered] != null) {
            InventoryObjectSO inventoryObjectSOToDisplay = inventoryObjectsArray[indexHovered].GetInventoryObjectSO();
            currentHoverInventoryItemImage.sprite = inventoryObjectSOToDisplay.sprite;
            currentHoverInventoryItemName.text = inventoryObjectSOToDisplay.objectName;
            currentHoverInventoryItemDescription.text = inventoryObjectSOToDisplay.objectDescription;
        }
    }

    //Toggle between showing the inventory screen or hiding it
    private void InventoryManager_OnOpenInventoryUI(object sender, System.EventArgs e) {
        if (InventoryManager.Instance.IsInventoryOpen()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
        isInAction = true;
    }

    private void Hide() {
        gameObject.SetActive(false);
        isInAction = false;
    }

    /**
     * Updates the individual inventory slots on start up
     */
    public void UpdateVisual() {
        InventoryObject[] inventoryObjectsArray = InventoryManager.Instance.GetInventoryObjectArray();
        for (int i = 0; i < inventoryObjectsArray.Length; i++) {
            if (inventoryObjectsArray[i] != null) {
                Sprite sprite = inventoryObjectsArray[i].GetInventoryObjectSO().sprite;
                inventorySingleUIslots[i].SetInventorySlotDragDropSprite(sprite);
            } else {
                inventorySingleUIslots[i].GetInventoryDragDrop().GetIconImage().color = new UnityEngine.Color(1,1,1,0);
            }
        }
        InventoryManager.Instance.UpdateFreeInventorySlot();
    }

    /**
     * Updates the inventory slot visual when a new item is added to the inventory.
     * 
     * @param index The index of the inventory slot to update
     * @param inventoryObjectSO The blueprint of the inventoryObject to add in
     */
    public void AddToInventoryVisual(int index, InventoryObjectSO inventoryObjectSO) {
        Sprite sprite = inventoryObjectSO.sprite;
        inventorySingleUIslots[index].SetInventorySlotDragDropSprite(sprite);
    }

    /**
     * Updates the inventory slot visual when an item is removed from the inventory.
     * 
     * @param index The index of the inventory slot to update
     */
    public void RemoveFromInventoryVisual(int index) {
        inventorySingleUIslots[index].SetInventorySlotDragDropSprite(null);
    }

    /**
     * Closes the drop item UI when anywhere in the inventory screen is clicked.
     */
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left 
            && eventData.pointerClick.layer != dropItemLayerMask
            && dropItemUI.IsDropItemUIOpen()) {

            dropItemUI.CloseDropItemUI();
        }
    }
}
