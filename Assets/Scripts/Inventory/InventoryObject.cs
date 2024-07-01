using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : InteractableObject {

    [SerializeField] private InventoryObjectSO inventoryObjectSO;  
    
    public InventoryObjectSO GetInventoryObjectSO() {
        return inventoryObjectSO;
    }

}
