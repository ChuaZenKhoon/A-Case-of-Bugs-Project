using UnityEngine;

/**
 * A super class representing all objects that can be interacted with and be
 * part of the inventory system.
 */
public class InventoryObject : InteractableObject {

    //Each object has an scriptable object containing unique info.
    [SerializeField] private InventoryObjectSO inventoryObjectSO;  
    
    public InventoryObjectSO GetInventoryObjectSO() {
        return inventoryObjectSO;
    }

}
