using System;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * A UI element that represents an individual inventory slot.
 */
public class InventorySingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerClickHandler{

    [SerializeField] private GameObject hoveredInventorySlotVisual;
    
    [SerializeField] private int index;

    //Event for when cursor hovers over the inventory slot UI element
    public static event EventHandler<int> OnHoverEnterInventorySlot;

    //Event for when cursors leaves the inventory slot UI element
    public static event EventHandler<int> OnHoverLeaveInventorySlot;

    //Event for when an item is selected to be dropped.
    public static event EventHandler<OnRightClickInventorySlotEventArgs> OnRightClickInventorySlot;

    //Reset static events on scene load
    public static void ResetStaticData() {
        OnHoverEnterInventorySlot = null;
        OnHoverLeaveInventorySlot = null;
        OnRightClickInventorySlot = null;
    }

    public class OnRightClickInventorySlotEventArgs {
        public Vector2 mousePosition;
        public int inventorySlotIndex;
    }

    public class OnSuccessfulDragDropItemEventArgs {
        public int oldIndex;
        public int newIndex;
    }

    //Drag drop logic component attached to it
    [SerializeField] private InventoryDragDrop inventoryDragDrop;

    //Hide UI hover element on startup
    private void Start () {
        Hide();
    }

    private void Show() {
        hoveredInventorySlotVisual.SetActive(true);
    }

    private void Hide() {
        hoveredInventorySlotVisual.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Show();
        OnHoverEnterInventorySlot?.Invoke(this, this.index);
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        Hide();
        OnHoverLeaveInventorySlot?.Invoke(this, this.index);
    }

    //Tell drop item UI element that is it being called
    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Right && inventoryDragDrop.GetIconSprite() != null) {
            OnRightClickInventorySlot?.Invoke(this, new OnRightClickInventorySlotEventArgs {
                mousePosition = eventData.position,
                inventorySlotIndex = this.index,
            });
        }
    }

    public int GetIndex() {
        return this.index;
    }

    public InventoryDragDrop GetInventoryDragDrop() {
        return this.inventoryDragDrop;
    }

    public void SetInventorySlotDragDropSprite(Sprite sprite) {
        inventoryDragDrop.SetIconSprite(sprite);
    }

    //When an inventory slot sprite is dropped onto this inventory slot, check it
    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null && eventData.button == PointerEventData.InputButton.Left) {
            //Successful drop, swap inventory items
            InventoryDragDrop incomingInventoryDragDrop = eventData.pointerDrag.GetComponent<InventoryDragDrop>();
            
            Sprite incomingSprite = incomingInventoryDragDrop.GetIconSprite();
            int incomingIndex = incomingInventoryDragDrop.GetParent().GetIndex();

            Sprite currentSprite = this.inventoryDragDrop.GetIconSprite();
            int currentIndex = this.index;

            incomingInventoryDragDrop.SetIconSprite(currentSprite);
            this.inventoryDragDrop.SetIconSprite(incomingSprite);

            InventoryManager.Instance.SuccessfulSwap(this.index, incomingIndex);
        }
    }


}
