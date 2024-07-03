using System;
using UnityEngine;

/**
 * A manager in charge of handling the logic of the inventory.
 */
public class InventoryManager : MonoBehaviour {

    public static InventoryManager Instance { get; private set; }

    //Event for when inventory UI is to open/close
    public event EventHandler OnInventoryOpenStateChange;

    private const int INVENTORY_SLOTS = 20;

    private const int EQUIP_SLOT_ONE = 0;
    private const int EQUIP_SLOT_TWO = 1;
    private const int EQUIP_SLOT_THREE = 2;
    private const int EQUIP_SLOT_FOUR = 3;
    private const int EQUIP_SLOT_FIVE = 4;

    private const int NON_EVIDENCE_INTERACTING_EQUIPMENT_ID = 0;

    [SerializeField] private InventoryScreenUI inventoryScreenUI;
    [SerializeField] private InventoryBarUI inventoryBarUI;

    //Allow putting in of equipment prefabs from assets
    [SerializeField] private InventoryObject[] inventoryObjectsArray;
    [SerializeField] private int[] equipmentIDArray;

    private bool isInventoryOpened;

    private int currentBarSlotSelected;
    private int currentEmptyInventorySlot;


    private void Awake() {
        Instance = this;
        isInventoryOpened = false;
        currentBarSlotSelected = 0;
        currentEmptyInventorySlot = 0;
        equipmentIDArray = new int[INVENTORY_SLOTS];
    }

    //Subscribe to game state change event
    private void Start() {
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
            DifficultySO difficultySO = DifficultySettingManager.difficultyLevelSelected;

            if (difficultySO != null) {
                foreach (EquipmentSO equipmentSO in difficultySO.equipmentListToAdd) {
                    this.AddToInventory(equipmentSO);
                }
            }

            AssignEquipmentID();
        }

        if (Loader.targetScene == Loader.Scene.TutorialScene) {
            TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
        }

        //Update UI
        Sprite[] inventorySprites = new Sprite[INVENTORY_SLOTS];
 
        for (int i = 0; i < inventoryObjectsArray.Length; i++) {
            if (inventoryObjectsArray[i] != null) {
                inventorySprites[i] = inventoryObjectsArray[i].GetInventoryObjectSO().sprite;
            } else {
                inventorySprites[i] = null;
            }
        }
        inventoryBarUI.UpdateVisual(inventorySprites);
        inventoryScreenUI.UpdateVisual(inventorySprites);
        UpdateFreeInventorySlot();
    }

    private void TutorialLevelManager_OnStateChange(object sender, EventArgs e) {
        if (TutorialLevelManager.Instance.IsStartingInventory()) {
            GameInput.Instance.OnInventoryKeyAction += GameInput_OnInventoryOpen;
            GameInput.Instance.OnInventoryBarSelect += GameInput_OnInventoryBarSelect;
            InventoryDropItemUI.OnConfirmedDropItem += InventoryDropItemUI_OnConfirmedDropItem;
        }

        if (TutorialLevelManager.Instance.GetState() == TutorialLevelManager.State.Equipment_Swab) {
            AssignEquipmentID();
        }
    }

    //Only subscribe to inventory actions when game is playing
    private void CrimeSceneLevelManager_OnStateChange(object sender, EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGamePlaying()) {
            GameInput.Instance.OnInventoryKeyAction += GameInput_OnInventoryOpen;
            GameInput.Instance.OnInventoryBarSelect += GameInput_OnInventoryBarSelect;
            InventoryDropItemUI.OnConfirmedDropItem += InventoryDropItemUI_OnConfirmedDropItem;
        }
    }

    private void InventoryDropItemUI_OnConfirmedDropItem(object sender, int e) {
        DropFromInventory(e);
    }

    private void GameInput_OnInventoryBarSelect(object sender, GameInput.OnInventoryBarSelectEventArgs e) {
        //Guard clause to deny swapping items if equipment is in use
        if (Equipment.isInAction) {
            MessageLogManager.Instance.LogMessage("Equipment in use. Exit equipment usage before swapping held item.");
            return;
        }
        
        GameInput.InventoryBarSlot num = e.inventoryBarSlot;

        switch (num) {
            default:
            case GameInput.InventoryBarSlot.First:
                currentBarSlotSelected = EQUIP_SLOT_ONE;
                break;
            case GameInput.InventoryBarSlot.Second:
                currentBarSlotSelected = EQUIP_SLOT_TWO;
                break;
            case GameInput.InventoryBarSlot.Third:
                currentBarSlotSelected = EQUIP_SLOT_THREE;
                break;
            case GameInput.InventoryBarSlot.Fourth:
                currentBarSlotSelected = EQUIP_SLOT_FOUR;
                break;
            case GameInput.InventoryBarSlot.Fifth:
                currentBarSlotSelected = EQUIP_SLOT_FIVE;
                break;

        }

        //When player selects the inventory bar slot, the item should appear and be held
        if (inventoryObjectsArray[currentBarSlotSelected] != null) {
            Player.Instance.UpdateHeldItem(inventoryObjectsArray[currentBarSlotSelected].GetInventoryObjectSO(),
                    equipmentIDArray[currentBarSlotSelected]);
        } else {
            Player.Instance.UpdateHeldItem(null, NON_EVIDENCE_INTERACTING_EQUIPMENT_ID);
        }

    }


    public bool IsInventoryOpen() {
        return isInventoryOpened;
    }

    //Tell inventory screen UI to open/close
    private void GameInput_OnInventoryOpen(object sender, System.EventArgs e) {
        //Guard clause to deny opening inventory if equipment is in use
        if (Equipment.isInAction) {
            MessageLogManager.Instance.LogMessage("Equipment in use. Exit equipment usage before opening Inventory.");
            return;
        }

        isInventoryOpened = !isInventoryOpened;
        if (isInventoryOpened ) {
            inventoryScreenUI.Show();
        } else {
            inventoryScreenUI.Hide();
        }

        OnInventoryOpenStateChange?.Invoke(this, EventArgs.Empty);
    }
    
    public InventoryObject[] GetInventoryObjectArray() {
        return inventoryObjectsArray;
    }

    //Abstracted Method call by inventorySingleUI when swap is successful
    public void SuccessfulSwap(int oldIndex, int newIndex) {
        SwapInventoryItems(oldIndex, newIndex);
    }

    /**
     * Following 5 methods deal with the logic of the inventory
     * 1) Swaps two items in the inventory
     * 2) Updates the free inventory slot whenever a change occurs to the inventory
     * 3) Check if there is a free inventory slot for item pick up
     * 4) Adding an item to the inventory
     * 5) Removing an item from the inventory
     */

    private void SwapInventoryItems(int oldIndex, int newIndex) {
        //Simple swap logic
        InventoryObject temp = inventoryObjectsArray[oldIndex];
        inventoryObjectsArray[oldIndex] = inventoryObjectsArray[newIndex];
        inventoryObjectsArray[newIndex] = temp;

        int tempInt = equipmentIDArray[oldIndex];
        equipmentIDArray[oldIndex] = equipmentIDArray[newIndex];
        equipmentIDArray[newIndex] = tempInt;

        //Update UI
        Sprite oldIndexSprite = null;
        Sprite newIndexSprite = null;
        if (inventoryObjectsArray[oldIndex] != null) {
            oldIndexSprite = inventoryObjectsArray[oldIndex].GetInventoryObjectSO().sprite;
        }

        if (inventoryObjectsArray[newIndex] != null) {
            newIndexSprite = inventoryObjectsArray[newIndex].GetInventoryObjectSO().sprite;
        }

        inventoryBarUI.SwapInventoryBarVisual(oldIndex, newIndex, oldIndexSprite, newIndexSprite);
        
        //If swap involves current held item, need to update it
        if (oldIndex == currentBarSlotSelected || newIndex == currentBarSlotSelected) {

            if (inventoryObjectsArray[currentBarSlotSelected] != null) {
                Player.Instance.UpdateHeldItem(inventoryObjectsArray[currentBarSlotSelected].GetInventoryObjectSO(),
                    equipmentIDArray[currentBarSlotSelected]);
            } else {
                Player.Instance.UpdateHeldItem(null, NON_EVIDENCE_INTERACTING_EQUIPMENT_ID);
            }
        }

        UpdateFreeInventorySlot();
    }

    public void UpdateFreeInventorySlot() {
        int currentIndex;
        
        for (currentIndex = 0; currentIndex < inventoryObjectsArray.Length; currentIndex++) {
            //break after assignment to prevent reassignment
            if (inventoryObjectsArray[currentIndex] == null) {
                currentEmptyInventorySlot = currentIndex;
                break;
            }
        }
        
        //No assignment to value -> no more free space in inventory
        if (currentIndex >= inventoryObjectsArray.Length) {
            currentEmptyInventorySlot = currentIndex;
        }
    }

    public bool HasSpaceInInventory() {
        return currentEmptyInventorySlot < inventoryObjectsArray.Length;
    }

    /**
     * Adds an item to the inventory.
     * 
     * @param inventoryObjectSO the blueprint of the inventoryObject to add 
     */
    public void AddToInventory(InventoryObjectSO inventoryObjectSO) {
        InventoryObject newInventoryObjectComponent = inventoryObjectSO.prefab.GetComponent<InventoryObject>();
        inventoryObjectsArray[currentEmptyInventorySlot] = newInventoryObjectComponent;

        Sprite newSprite = newInventoryObjectComponent.GetInventoryObjectSO().sprite;

        //Tell relevant UI to update
        inventoryScreenUI.AddToInventoryVisual(currentEmptyInventorySlot, newSprite);
        if (currentEmptyInventorySlot <= EQUIP_SLOT_FIVE) {
            inventoryBarUI.AddToInventoryBarVisual(currentEmptyInventorySlot, newSprite);
        }
        UpdateFreeInventorySlot();
    }

    /**
     * Removes an item from the inventory when it is dropped.
     * 
     * @param index The index of the item in the inventory that needs to be dropped 
     */
    public void DropFromInventory(int index) {
        GameObject newInventoryObjectToDrop = Instantiate(inventoryObjectsArray[index].GetInventoryObjectSO().prefab);

        if (newInventoryObjectToDrop.TryGetComponent<Evidence>(out Evidence evidence)) {
            evidence.SealEvidence();
        }

        newInventoryObjectToDrop.transform.position = Player.Instance.GetStareAtPosition();

        inventoryObjectsArray[index] = null;

        //Tell relevant UI to update
        inventoryScreenUI.RemoveFromInventoryVisual(index);
        
        
        if (index <= EQUIP_SLOT_FIVE) {
            inventoryBarUI.RemoveFromInventoryBarVisual(index);
        }
        if (index == currentBarSlotSelected) {
            Player.Instance.UpdateHeldItem(null, NON_EVIDENCE_INTERACTING_EQUIPMENT_ID);
        }
        
        UpdateFreeInventorySlot();
    }

    /**
     * Assigns unique equipment ID to evidenceInteractingEquipment for persistent data storage and retrieval
     */
    private void AssignEquipmentID() {
        int id = 1;
        for (int index = 0; index < inventoryObjectsArray.Length; index++) {
            if (inventoryObjectsArray[index] != null && inventoryObjectsArray[index] is EvidenceInteractingEquipment) {
                equipmentIDArray[index] = id;
                id++;
            } else {
                equipmentIDArray[index] = NON_EVIDENCE_INTERACTING_EQUIPMENT_ID;
            }
        }
    }
}
