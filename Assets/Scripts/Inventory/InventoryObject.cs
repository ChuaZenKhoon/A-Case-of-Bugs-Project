using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : MonoBehaviour {

    [SerializeField] private InventoryObjectSO inventoryObjectSO;  
    public virtual void Interact() { }

    public InventoryObjectSO GetInventoryObjectSO() {
        return inventoryObjectSO;
    }

}
