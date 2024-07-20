using System;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the drop item pop up in the inventory.
 */
public class InventoryDropItemUI : MonoBehaviour {

    //Event when an item drop is confirmed
    public static event EventHandler<int> OnConfirmedDropItem;

    //Reset static events on scene load
    public static void ResetStaticData() {
        OnConfirmedDropItem = null;
    }

    [SerializeField] private Button dropItemButton;
    [SerializeField] private GameObject[] dropItemUI;


    private int dropItemIndex;
    private bool isOpened;

    //Button to execute drop item function when clicked
    private void Awake() {
        dropItemButton.onClick.AddListener(() => {
            DropItem();
        });
    }

    //Subscribe to right click in inventory screen event
    private void Start() {
        InventorySingleUI.OnRightClickInventorySlot += InventorySingleUI_OnRightClickInventorySlot;
        CloseDropItemUI();
    }
    
    //Close the UI element, then invoke the event for logic to handle
    private void DropItem() {
        CloseDropItemUI();
        OnConfirmedDropItem?.Invoke(this, this.dropItemIndex);
    }


    private void InventorySingleUI_OnRightClickInventorySlot(object sender, InventorySingleUI.OnRightClickInventorySlotEventArgs e) {
        //Equipment cannot be dropped
        if (InventoryManager.Instance.GetInventoryObjectArray()[e.inventorySlotIndex] is Evidence) {
            this.gameObject.transform.position = e.mousePosition;
            this.dropItemIndex = e.inventorySlotIndex;

            this.gameObject.SetActive(true);
            
            isOpened = true;
        }
    }

    public bool IsDropItemUIOpen() {
        return isOpened;
    }

    public void CloseDropItemUI() {
        this.gameObject.SetActive(false);
        isOpened= false;
    }
}

