using UnityEngine;

/**
 * A class representing a piece of evidence that can be collected.
 */
public class Evidence : InventoryObject {
    public override void Interact() {
        //Add to inventory, then delete this object instance in the world
        if (InventoryManager.Instance.HasSpaceInInventory()) {
            InventoryManager.Instance.AddToInventory(this.GetInventoryObjectSO());
            Destroy(this.gameObject);
        }
    }
    

}
