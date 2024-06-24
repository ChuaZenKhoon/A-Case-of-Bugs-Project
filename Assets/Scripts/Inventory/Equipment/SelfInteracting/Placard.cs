using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placard : InventoryObject {

    [SerializeField] private int placardNumber;

    public int GetPlacardNumber() {
        return placardNumber;
    }

    public override void Interact() {
        MessageLogManager.Instance.LogMessage("Use the placard holder to pick this up.");
    }
}
