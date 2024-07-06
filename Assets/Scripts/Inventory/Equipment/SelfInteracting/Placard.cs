using UnityEngine;

/**
 * A component of the placardHolder equipment, representing a singluar placard to be placed.
 */

public class Placard : InventoryObject {

    [SerializeField] private int placardNumber;

    public int GetPlacardNumber() {
        return placardNumber;
    }

    public override void Interact() {
        MessageLogManager.Instance.LogMessage("Use the placard holder to pick this up.");
    }
}
